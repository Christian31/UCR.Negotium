using MahApps.Metro.Controls;
using System.Data;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using UCR.Negotium.Extensions;
using System;
using UCR.Negotium.Dialogs;
using System.Linq;
using UCR.Negotium.Base.Trace;
using UCR.Negotium.Base.Enumerados;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for RegistrarProyectoWindow.xaml
    /// </summary>
    public partial class RegistrarProyectoWindow : MetroWindow, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private DataView dtFlujoCaja;
        private IndicadorEconomico tir, pri, relacionBC, van, vac, relacionBCInvInicial;
        private string signoMoneda;
        private IndicadoresFinancieros indicFinancieros;
        private IndicadoresSociales indicSociales;

        private bool pendingSaveMoneda;

        private ProyectoData proyectoData;
        private EncargadoData encargadoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region Contructor
        public RegistrarProyectoWindow(int codProyecto = 0, int codEncargado = 0)
        {
            tir = pri = relacionBC = van = vac = relacionBCInvInicial = new IndicadorEconomico();
            InitializeComponent();
            DataContext = this;

            proyectoData = new ProyectoData();
            encargadoData = new EncargadoData();
            signoMoneda = string.Empty;

            dtFlujoCaja = new DataView();
            proyecto = new Proyecto();

            if (!codProyecto.Equals(0))
            {
                ProyectoSelected = proyectoData.GetProyecto(codProyecto);
                ProyectoSelected.OrganizacionProponente = new OrganizacionProponenteData().GetOrganizacionProponente(codProyecto);
                LocalContext.ReloadUserControls(proyecto.CodProyecto, Modulo.InformacionGeneral);
            }

            if (!codEncargado.Equals(0))
            {
                ProyectoSelected.Encargado = encargadoData.GetEncargado(codEncargado);

                if (codProyecto.Equals(0))
                    infoGeneral.AddEvaluador(codEncargado);
            }

            InitializeComponent();
        }
        #endregion

        #region Properties
        public Proyecto ProyectoSelected
        {
            get
            {
                return proyecto;
            }
            set
            {
                proyecto = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ProyectoSelected"));
            }
        }

        public DataView DTFlujoCaja
        {
            get
            {
                return dtFlujoCaja;
            }
            set
            {
                dtFlujoCaja = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DTFlujoCaja"));
            }
        }

        public string TIR
        {
            get { return tir.ToString(); }
            set { }
        }

        public string VAN
        {
            get { return van.ToString(); }
            set { }
        }

        public string VAC
        {
            get { return vac.ToString(); }
            set { }
        }

        public string PRI
        {
            get { return pri.ToString(); }
            set { }
        }

        public string RelacionBC
        {
            get { return relacionBC.ToString(); }
            set { }
        }

        public string RelacionBCInversionInicial
        {
            get { return relacionBCInvInicial.ToString(); }
            set { }
        }

        public int CodProyecto { get; set; }
        #endregion

        #region Public Methods
        public void ReloadBase(int codProyecto)
        {
            if(pendingSaveMoneda)
            {
                LocalContext.SetMoneda(codProyecto, ProyectoSelected.TipoMoneda.CodMoneda);
            }
            
            ReloadProyecto(codProyecto);

            ((TabItem)tcRegistrarProyecto.Items[9]).IsEnabled = !(proyecto.TipoProyecto.CodTipo.Equals(0) && !proyecto.ConFinanciamiento);
        }
        #endregion

        #region Private Methods
        private void ReloadProyecto(int codProyecto)
        {
            proyecto = proyectoData.GetProyecto(codProyecto);
            proyecto.OrganizacionProponente = orgProponente.OrgProponente;
            proyecto.Proyecciones = proyeccionVentas.ProyeccionesList;
            proyecto.Financiamiento = financiamientoUc.FinanciamientoSelected;
            proyecto.Costos = costos.CostosList;
            proyecto.VariacionCostos = costos.VariacionAnualCostos;
            proyecto.Inversiones = inversiones.InversionesList;
            proyecto.Reinversiones = reinversiones.ReinversionesList;
            signoMoneda = LocalContext.GetSignoMoneda(codProyecto);

            LocalContext.SetFlujoCaja(null);
            ReloadProgress();
        }

        private void ReloadProgress()
        {
            Dictionary<Modulo, bool> stepsProgress = new Dictionary<Modulo, bool>(12);

            stepsProgress.Add(Modulo.InformacionGeneral, !proyecto.CodProyecto.Equals(0));
            stepsProgress.Add(Modulo.Proponente, !orgProponente.OrgProponente.CodOrganizacion.Equals(0));
            stepsProgress.Add(Modulo.Caracterizacion, !string.IsNullOrWhiteSpace(proyecto.CaraterizacionDelBienServicio));
            stepsProgress.Add(Modulo.Inversiones, !inversiones.InversionesList.Count.Equals(0));
            stepsProgress.Add(Modulo.Reinversiones, !reinversiones.ReinversionesList.Count.Equals(0));
            stepsProgress.Add(Modulo.ProyeccionVentas, !proyeccionVentas.ProyeccionesList.Count.Equals(0));

            if (!costos.CostosList.Count.Equals(0))
            {
                stepsProgress.Add(Modulo.Costos, true);
                stepsProgress.Add(Modulo.CapitalTrabajo,true);
            }
            else
            {
                stepsProgress.Add(Modulo.Costos, false);
                stepsProgress.Add(Modulo.CapitalTrabajo, false);
            }

            if (!inversiones.InversionesList.Where(inv => inv.Depreciable).Count().Equals(0) || 
                !reinversiones.ReinversionesList.Where(reinv => reinv.Depreciable).Count().Equals(0))
            {
                stepsProgress.Add(Modulo.Depreciaciones, true);
            }
            else
            {
                stepsProgress.Add(Modulo.Depreciaciones, false);
            }

            stepsProgress.Add(Modulo.Financiamiento, !financiamientoUc.FinanciamientoSelected.CodFinanciamiento.Equals(0));

            if (!inversiones.InversionesList.Count.Equals(0) || !reinversiones.ReinversionesList.Count.Equals(0) ||
                !proyeccionVentas.ProyeccionesList.Count.Equals(0) || !costos.CostosList.Count.Equals(0) ||
                !financiamientoUc.FinanciamientoSelected.CodFinanciamiento.Equals(0))
            {
                stepsProgress.Add(Modulo.FlujoCaja, true);
            }
            else
            {
                stepsProgress.Add(Modulo.FlujoCaja, false);
            }
            stepsProgress.Add(Modulo.AnalisisAmbiental, !analisisAmbiental.FactorAmbientalList.Count.Equals(0));

            progreso.Reload(stepsProgress);
        }

        private void LlenaFlujoCaja()
        {
            DTFlujoCaja = LocalContext.GetFlujoCaja(proyecto, capitalTrabajo.DTCapitalTrabajo,
                financiamientoUc.DTFinanciamiento, reinversiones.DTTotalesReinversiones, 
                inversiones.InversionesTotal, capitalTrabajo.RecuperacionCT);

            LlenaCalculosFinales();
        }

        private void LlenaCalculosFinales()
        {
            if(ProyectoSelected.TipoProyecto.CodTipo == 1)
            {
                indicFinancieros = new IndicadoresFinancieros(ProyectoSelected.HorizonteEvaluacionEnAnos, 
                    signoMoneda, DTFlujoCaja.Table, ProyectoSelected.TasaCostoCapital);

                van = indicFinancieros.VAN;
                tir = indicFinancieros.TIR;
                pri = indicFinancieros.PRI;
                relacionBC = indicFinancieros.RelacionBC;
                relacionBCInvInicial = indicFinancieros.RelacionBCInversionInicial;

                PropertyChanged(this, new PropertyChangedEventArgs("VAN"));
                PropertyChanged(this, new PropertyChangedEventArgs("TIR"));
                PropertyChanged(this, new PropertyChangedEventArgs("PRI"));
                PropertyChanged(this, new PropertyChangedEventArgs("RelacionBC"));
                PropertyChanged(this, new PropertyChangedEventArgs("RelacionBCInversionInicial"));
            }
            else
            {
                indicSociales = new IndicadoresSociales(ProyectoSelected.HorizonteEvaluacionEnAnos,
                    signoMoneda, DTFlujoCaja.Table, ProyectoSelected.TasaCostoCapital);

                vac = indicSociales.VAC;
                PropertyChanged(this, new PropertyChangedEventArgs("VAC"));
            }
        }
        #endregion

        #region Events
        private void dgFlujoCaja_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgFlujoCajaFinanciero.Columns.Count > 0)
            {
                this.dgFlujoCajaFinanciero.Columns[0].Width = 180;
            }
        }

        bool displayWarningClosing = true;
        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            if (displayWarningClosing)
            {
                if (CustomMessageBox.ShowConfirmationMesage(Constantes.CERRARVENTANAMSG))
                {
                    if (ProyectoSelected.CodProyecto.Equals(0) && !ProyectoSelected.Encargado.IdEncargado.Equals(0))
                    {
                        encargadoData.EliminarEncargado(ProyectoSelected.Encargado.IdEncargado);
                    }
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        private void tcRegistrarProyecto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tcRegistrarProyecto.IsLoaded)
            {
                if (e.Source != financiamientoUc || !financiamientoUc.CargaDesdeCombos)
                {
                    int indice = tcRegistrarProyecto.SelectedIndex;

                    if (proyecto.CodProyecto.Equals(0) && !indice.Equals(0))
                    {
                        MessageBox.Show(Constantes.NAVEGARDATOSVACIOSINFOGENERALMSG, Constantes.NAVEGARDATOSVACIOSTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        tcRegistrarProyecto.SelectedIndex = 0;
                    }
                    else if (!proyecto.TipoProyecto.CodTipo.Equals(2))
                    {
                        if (indice > 1 && proyecto.OrganizacionProponente.CodOrganizacion.Equals(0))
                        {
                            MessageBox.Show(Constantes.NAVEGARDATOSVACIOSPROPMSG, Constantes.NAVEGARDATOSVACIOSTLT, 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            tcRegistrarProyecto.SelectedIndex = 1;
                        }
                        else if (indice > 2 && proyecto.CaraterizacionDelBienServicio.Equals(string.Empty))
                        {
                            MessageBox.Show(Constantes.NAVEGARDATOSVACIOSCARACTMSG, Constantes.NAVEGARDATOSVACIOSTLT, 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            tcRegistrarProyecto.SelectedIndex = 2;
                        }
                        else if (indice.Equals(10))
                        {
                            //llenar flujo de caja 
                            if (proyecto.TipoProyecto.CodTipo.Equals(1))
                            {
                                this.spIndicadoresSocial.Visibility = Visibility.Hidden;
                                this.spIndicadoresFinancieros.Visibility = Visibility.Visible;
                            }
                            else if (proyecto.TipoProyecto.CodTipo.Equals(3))
                            {
                                this.spIndicadoresFinancieros.Visibility = Visibility.Hidden;
                                this.spIndicadoresSocial.Visibility = Visibility.Visible;
                            }

                            LlenaFlujoCaja();
                        }

                        if (indice == 9)
                        {
                            LocalContext.ReloadUserControls(proyecto.CodProyecto, Modulo.Financiamiento);
                        }
                    }
                    else
                    {
                        this.spIndicadoresSocial.Visibility = Visibility.Hidden;
                        this.spIndicadoresFinancieros.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    financiamientoUc.CargaDesdeCombos = false;
                }
            }
        }

        private void lbDetalleIndicadores_Click(object sender, RoutedEventArgs e)
        {
            if (!ProyectoSelected.TipoProyecto.CodTipo.Equals(2))
            {
                object indicadores;
                if (ProyectoSelected.TipoProyecto.CodTipo == 1)
                    indicadores = indicFinancieros;
                else
                    indicadores = indicSociales;

                IndicadoresFinales indicFinales = new IndicadoresFinales(ProyectoSelected.CodProyecto, indicadores);
                indicFinales.ShowDialog();

                if (indicFinales.IsActive == false && indicFinales.Reload)
                {
                    ProyectoSelected.TasaCostoCapital = proyectoData.GetProyecto(ProyectoSelected.CodProyecto).TasaCostoCapital;
                    PropertyChanged(this, new PropertyChangedEventArgs("ProyectoSelected"));
                    LlenaCalculosFinales();
                }
            }
            else
            {
                MessageBox.Show(Constantes.ACTUALIZARPROYECTORESTRTIPOAMBIENTAL, Constantes.ACTUALIZARPROYECTOTLT, 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            if (ProyectoSelected != null && ProyectoSelected.CodProyecto != 0)
            {
                //cargar el objeto con toda la informacion para generar el reporte
                Proyecto proyecto = new Proyecto();
                proyecto = proyectoData.GetProyecto(ProyectoSelected.CodProyecto);

                if(proyecto.TipoProyecto.CodTipo != 2)
                {
                    proyecto.OrganizacionProponente = orgProponente.OrgProponente;
                    proyecto.Inversiones = inversiones.InversionesList;
                    proyecto.Reinversiones = reinversiones.ReinversionesList;
                    proyecto.Proyecciones = proyeccionVentas.ProyeccionesList;
                    proyecto.Costos = costos.CostosList;
                    proyecto.Financiamiento = financiamientoUc.FinanciamientoSelected;

                    LlenaFlujoCaja();

                    try
                    {
                        GenerarReporte reporte = null;
                        if (proyecto.TipoProyecto.CodTipo == 1)
                        {
                            reporte = new GenerarReporte(proyecto, inversiones.InversionesTotal, 
                                reinversiones.DTTotalesReinversiones, proyeccionVentas.DTProyeccionesTotales,
                                costos.DTCostosTotales, capitalTrabajo.DTCapitalTrabajo,
                                capitalTrabajo.RecuperacionCT, depreciaciones.DTTotalesDepreciaciones,
                                financiamientoUc.DTFinanciamiento, DTFlujoCaja, TIR, PRI, 
                                RelacionBC, van, RelacionBCInversionInicial);
                        }
                        else
                        {
                            reporte = new GenerarReporte(proyecto, inversiones.InversionesTotal,
                                reinversiones.DTTotalesReinversiones, costos.DTCostosTotales,
                                capitalTrabajo.DTCapitalTrabajo, capitalTrabajo.RecuperacionCT,
                                depreciaciones.DTTotalesDepreciaciones, financiamientoUc.DTFinanciamiento,
                                DTFlujoCaja, vac);
                        }

                        reporte.CrearReporte();
                    }
                    catch (Exception ex)
                    {
                        ex.TraceExceptionAsync();
                        MessageBox.Show(Constantes.GENERARREPORTEERRORMSG, Constantes.GENERARREPORTETLT,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(Constantes.GENERARREPORTENODISPONIBLEMSG, Constantes.GENERARREPORTETLT,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnArchivar_Click(object sender, RoutedEventArgs e)
        {
            if (ProyectoSelected != null && ProyectoSelected.CodProyecto != 0 && !ProyectoSelected.Archivado)
            {
                string message = ConfigurationHelper.EsDeUsoAcademico() ? 
                    Constantes.ARCHIVARPROYECTOACADEMICO : 
                    Constantes.ARCHIVARPROYECTOPROFESIONAL;
                if (CustomMessageBox.ShowConfirmationMesage(Constantes.ARCHIVARPROYECTO + message))
                {
                    if (proyectoData.ArchivarProyecto(ProyectoSelected.CodProyecto, true))
                    {
                        displayWarningClosing = false;
                        this.Close();
                    }
                }
            }
        }

        private void btnVerResumen_Click(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void btnVerProgreso_Click(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(1);
        }

        private void ToggleFlyout(int index)
        {
            var flyout = this.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }

        private void btnMoneda_Click(object sender, RoutedEventArgs e)
        {
            RegistrarTipoMoneda registrarMoneda = new RegistrarTipoMoneda(ProyectoSelected.CodProyecto);
            registrarMoneda.ShowDialog();

            if (!registrarMoneda.IsActive)
            {
                if (registrarMoneda.Reload)
                {
                    LocalContext.ReloadUserControls(ProyectoSelected.CodProyecto, Modulo.InformacionGeneral);
                }
                else if (registrarMoneda.PendingSave)
                {
                    ProyectoSelected.TipoMoneda = registrarMoneda.TipoMonedaSelected;
                    pendingSaveMoneda = true;
                }
            }
        }

        private void btnEncargado_Click(object sender, RoutedEventArgs e)
        {
            if (!ProyectoSelected.Encargado.IdEncargado.Equals(0))
            {
                RegistrarEncargado registrarEncargado = new RegistrarEncargado(ProyectoSelected.Encargado.IdEncargado);
                registrarEncargado.ShowDialog();
            }
            else
            {
                MessageBox.Show(Constantes.EDITAREVALUADORNODISPONIBLEMSG, Constantes.EDITAREVALUADORTLT, 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
    }
}
