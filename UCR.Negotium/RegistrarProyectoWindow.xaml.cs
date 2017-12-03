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
using Microsoft.VisualBasic;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain.Tracing;
using UCR.Negotium.Domain.Enums;
using System.Linq;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for RegistrarProyectoWindow.xaml
    /// </summary>
    public partial class RegistrarProyectoWindow : MetroWindow, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private DataView dtFlujoCaja;
        private string tir, van, signoMoneda;
        private double montoInicial;
        private double[] flujoCaja;

        private bool pendingSaveMoneda;

        private ProyectoData proyectoData;
        private EncargadoData encargadoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region Contructor
        public RegistrarProyectoWindow(int codProyecto = 0, int codEncargado = 0)
        {
            InitializeComponent();

            DataContext = this;

            proyectoData = new ProyectoData();
            encargadoData = new EncargadoData();

            dtFlujoCaja = new DataView();
            proyecto = new Proyecto();

            if (!codProyecto.Equals(0))
            {
                ProyectoSelected = proyectoData.GetProyecto(codProyecto);
                ProyectoSelected.OrganizacionProponente = new OrganizacionProponenteData().GetOrganizacionProponente(codProyecto);
                ReloadUserControls(proyecto.CodProyecto);
            }

            if (!codEncargado.Equals(0))
            {
                ProyectoSelected.Encargado = encargadoData.GetEncargado(codEncargado);

                if (codProyecto.Equals(0))
                {
                    infoGeneral.AddEvaluador(codEncargado);
                }
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
            get { return tir; }
            set
            {
                tir = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TIR"));
            }
        }

        public string VAN
        {
            get { return van; }
            set
            {
                van = value;
                PropertyChanged(this, new PropertyChangedEventArgs("VAN"));
            }
        }

        public int CodProyecto { get; set; }
        #endregion

        #region Public Methods
        public void ReloadUserControls(int codProyecto)
        {
            if(pendingSaveMoneda)
            {
                LocalContext.SetMoneda(codProyecto, ProyectoSelected.TipoMoneda.CodMoneda);
            }
            resumen.CodProyecto = orgProponente.CodProyecto = infoGeneral.CodProyecto = caracterizacion.CodProyecto =
                    inversiones.CodProyecto = reinversiones.CodProyecto =
                    capitalTrabajo.CodProyecto = depreciaciones.CodProyecto =
                    costos.CodProyecto = proyeccionVentas.CodProyecto =
                    financiamientoUc.CodProyecto = analisisAmbiental.CodProyecto = codProyecto;

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
            proyecto.RequerimientosInversion = inversiones.InversionesList;
            proyecto.RequerimientosReinversion = reinversiones.ReinversionesList;
            signoMoneda = LocalContext.GetSignoMoneda(codProyecto);

            LocalContext.SetFlujoCaja(null);
            ReloadProgress();
        }

        private void ReloadProgress()
        {
            Dictionary<ProgresoStep, bool> stepsProgress = new Dictionary<ProgresoStep, bool>(12);

            stepsProgress.Add(ProgresoStep.InformacionGeneral, !proyecto.CodProyecto.Equals(0));
            stepsProgress.Add(ProgresoStep.Proponente, !orgProponente.OrgProponente.CodOrganizacion.Equals(0));
            stepsProgress.Add(ProgresoStep.Caracterizacion, !string.IsNullOrWhiteSpace(proyecto.CaraterizacionDelBienServicio));
            stepsProgress.Add(ProgresoStep.Inversiones, !inversiones.InversionesList.Count.Equals(0));
            stepsProgress.Add(ProgresoStep.Reinversiones, !reinversiones.ReinversionesList.Count.Equals(0));
            stepsProgress.Add(ProgresoStep.ProyeccionVentas, !proyeccionVentas.ProyeccionesList.Count.Equals(0));

            if (!costos.CostosList.Count.Equals(0))
            {
                stepsProgress.Add(ProgresoStep.Costos, true);
                stepsProgress.Add(ProgresoStep.CapitalTrabajo,true);
            }
            else
            {
                stepsProgress.Add(ProgresoStep.Costos, false);
                stepsProgress.Add(ProgresoStep.CapitalTrabajo, false);
            }

            if (!inversiones.InversionesList.Where(inv => inv.Depreciable).Count().Equals(0) || 
                !reinversiones.ReinversionesList.Where(reinv => reinv.Depreciable).Count().Equals(0))
            {
                stepsProgress.Add(ProgresoStep.Depreciaciones, true);
            }
            else
            {
                stepsProgress.Add(ProgresoStep.Depreciaciones, false);
            }

            stepsProgress.Add(ProgresoStep.Financiamiento, !financiamientoUc.FinanciamientoSelected.CodFinanciamiento.Equals(0));

            if (!inversiones.InversionesList.Count.Equals(0) || !reinversiones.ReinversionesList.Count.Equals(0) ||
                !proyeccionVentas.ProyeccionesList.Count.Equals(0) || !costos.CostosList.Count.Equals(0) ||
                !financiamientoUc.FinanciamientoSelected.CodFinanciamiento.Equals(0))
            {
                stepsProgress.Add(ProgresoStep.FlujoCaja, true);
            }
            else
            {
                stepsProgress.Add(ProgresoStep.FlujoCaja, false);
            }
            stepsProgress.Add(ProgresoStep.AnalisisAmbiental, !analisisAmbiental.FactorAmbientalList.Count.Equals(0));

            progreso.Reload(stepsProgress);
        }
        
        private void LlenaFlujoCaja()
        {
            DTFlujoCaja = LocalContext.GetFlujoCaja(proyecto, capitalTrabajo.DTCapitalTrabajo, financiamientoUc.DTFinanciamiento, reinversiones.DTTotalesReinversiones,
                inversiones.InversionesTotal, capitalTrabajo.RecuperacionCT);

            LlenaCalculosFinales();
        }

        private void LlenaCalculosFinales()
        {
            double[] num = new double[ProyectoSelected.HorizonteEvaluacionEnAnos + 1];
            double[] numArray = new double[ProyectoSelected.HorizonteEvaluacionEnAnos];
            for (int i = 0; i <= ProyectoSelected.HorizonteEvaluacionEnAnos; i++)
            {
                num[i] = Convert.ToDouble(DTFlujoCaja.Table.Rows[16][i + 1].ToString().Replace(signoMoneda, string.Empty));
            }

            for (int k = 0; k < ProyectoSelected.HorizonteEvaluacionEnAnos; k++)
            {
                numArray[k] = num[k + 1];
            }

            montoInicial = num[0];
            flujoCaja = numArray;

            try
            {
                double num2 = Financial.NPV(ProyectoSelected.TasaCostoCapital, ref numArray);
                VAN = signoMoneda + " " + num2.ToString("#,##0.##");
            }
            catch { VAN = signoMoneda + " " + 0.ToString("#,##0.##"); }

            try
            {
                double num1 = Financial.IRR(ref num) * 100;

                TIR = string.Concat(num1.ToString("#,##0.##"), " %");
            }
            catch { TIR = string.Concat(0.ToString("#,##0.##"), " %"); }
        }
        #endregion

        #region Events
        private void dgFlujoCaja_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgFlujoCaja.Columns.Count > 0)
            {
                this.dgFlujoCaja.Columns[0].Width = 180;
            }
        }

        bool displayWarningClosing = true;
        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            if (displayWarningClosing)
            {
                if (MessageBox.Show("Esta seguro que desea cerrar esta ventana?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
                int indice = tcRegistrarProyecto.SelectedIndex;

                if (proyecto.CodProyecto.Equals(0) && !indice.Equals(0))
                {
                    MessageBox.Show("Por favor ingrese los datos de Información General y guardelos para poder avanzar a la siguiente pestaña",
                    "Datos vacios", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tcRegistrarProyecto.SelectedIndex = 0;
                }
                else if (proyecto.TipoProyecto.CodTipo.Equals(1))
                {
                    if (indice > 1 && proyecto.OrganizacionProponente.CodOrganizacion.Equals(0))
                    {
                        MessageBox.Show("Por favor ingrese todos los datos del Proponente y guardelos para poder avanzar a la siguiente pestaña",
                        "Datos vacios", MessageBoxButton.OK, MessageBoxImage.Warning);
                        tcRegistrarProyecto.SelectedIndex = 1;
                    }
                    else if (indice > 2 && proyecto.CaraterizacionDelBienServicio.Equals(string.Empty))
                    {
                        MessageBox.Show("Por favor ingrese todos los datos de Caracterización y guardelos para poder avanzar a la siguiente pestaña",
                        "Datos vacios", MessageBoxButton.OK, MessageBoxImage.Warning);
                        tcRegistrarProyecto.SelectedIndex = 2;
                    }
                    else if (indice.Equals(10))
                    {
                        //llenar flujo de caja 
                        LlenaFlujoCaja();
                    }
                }
            }
        }

        private void lbDetalleIndicadores_Click(object sender, RoutedEventArgs e)
        {
            if (!ProyectoSelected.TipoProyecto.CodTipo.Equals(2))
            {
                double tir = Convert.ToDouble(TIR.Replace("%", string.Empty));
                double van = Convert.ToDouble(VAN.Replace(signoMoneda, string.Empty));
                IndicadoresFinales indicadores = new IndicadoresFinales(ProyectoSelected.CodProyecto, tir, van, montoInicial, flujoCaja);
                indicadores.ShowDialog();

                if (indicadores.IsActive == false && indicadores.Reload)
                {
                    ProyectoSelected.TasaCostoCapital = proyectoData.GetProyecto(ProyectoSelected.CodProyecto).TasaCostoCapital;
                    PropertyChanged(this, new PropertyChangedEventArgs("ProyectoSelected"));
                    LlenaCalculosFinales();
                }
            }
            else
            {
                MessageBox.Show("Este Tipo de Análisis es Ambiental, si desea realizar un Análisis Completo actualice el Tipo de Análisis del Proyecto", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            if (ProyectoSelected != null)
            {
                //cargar el objeto con toda la informacion para generar el reporte
                Proyecto proyecto = new Proyecto();
                proyecto = proyectoData.GetProyecto(ProyectoSelected.CodProyecto);
                proyecto.OrganizacionProponente = orgProponente.OrgProponente;
                proyecto.RequerimientosInversion = inversiones.InversionesList;
                proyecto.RequerimientosReinversion = reinversiones.ReinversionesList;
                proyecto.Proyecciones = proyeccionVentas.ProyeccionesList;
                proyecto.Costos = costos.CostosList;
                proyecto.Financiamiento = financiamientoUc.FinanciamientoSelected;

                LlenaFlujoCaja();

                try
                {
                    GenerarReporte reporte = new GenerarReporte(proyecto, inversiones.InversionesTotal,
                    reinversiones.DTTotalesReinversiones, proyeccionVentas.DTProyeccionesTotales,
                    costos.DTCostosTotales, capitalTrabajo.DTCapitalTrabajo,
                    capitalTrabajo.RecuperacionCT, depreciaciones.DTTotalesDepreciaciones,
                    financiamientoUc.DTFinanciamiento, DTFlujoCaja, TIR, VAN);

                    reporte.CrearReporte();
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    MessageBox.Show(ex.Message);
                    //MessageBox.Show("Se ha producido un error generando el Reporte, favor asegurese que ha ingresado todos los datos del proyecto. \n Si el error persiste comunicarse con el Administrador", "Generando Reporte", 
                        //MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnArchivar_Click(object sender, RoutedEventArgs e)
        {
            if (ProyectoSelected != null && !ProyectoSelected.Archivado)
            {
                if (MessageBox.Show("Esta seguro que desea archivar este Proyecto?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
                    ReloadUserControls(ProyectoSelected.CodProyecto);
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
                MessageBox.Show("Este proyecto no posee un Evaluador asociado para editar, esta opción solo permite editar un Evaluador asociado con el Proyecto", 
                    "Encargado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
    }
}
