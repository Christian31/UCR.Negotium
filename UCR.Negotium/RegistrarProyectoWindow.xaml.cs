using MahApps.Metro.Controls;
using System.Data;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Forms;
using UCR.Negotium.Utils;
using System;
using Microsoft.VisualBasic;
using UCR.Negotium.Dialogs;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for RegistrarProyectoWindow.xaml
    /// </summary>
    public partial class RegistrarProyectoWindow : MetroWindow, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private DataView dtFlujoCaja;
        private string tir;
        private string van;

        private double montoInicial;
        private double[] flujoCaja;

        private ProyectoData proyectoData;
        private EncargadoData encargadoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public RegistrarProyectoWindow(int codProyecto = 0, int codEncargado = 0)
        {
            DataContext = this;
            InitializeComponent();

            proyectoData = new ProyectoData();
            encargadoData = new EncargadoData();

            dtFlujoCaja = new DataView();
            proyecto = new Proyecto();

            if (!codProyecto.Equals(0))
            {
                ProyectoSelected = proyectoData.GetProyecto(codProyecto);
                ReloadUserControls(proyecto.CodProyecto);
            }

            //Que hacer con el encargado
            //TODDO HERE
            if (!codEncargado.Equals(0))
            {
                proyecto.Encargado = encargadoData.GetEncargado(codEncargado);
            }

            InitializeComponent();
        }

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

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Esta seguro que desea cerrar esta ventana?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }

        public void ReloadUserControls(int codProyecto)
        {
            proponente.CodProyecto = infoGeneral.CodProyecto = caracterizacion.CodProyecto =
                    inversiones.CodProyecto = reinversiones.CodProyecto =
                    capitalTrabajo.CodProyecto = depreciaciones.CodProyecto =
                    costos.CodProyecto = proyeccionVentas.CodProyecto =
                    financiamientoUc.CodProyecto = codProyecto;

            proyecto = proyectoData.GetProyecto(codProyecto);
            proyecto.Proponente = proponente.ProponenteSelected;
            proyecto.Proyecciones = proyeccionVentas.ProyeccionesList;
            proyecto.Financiamiento = financiamientoUc.FinanciamientoSelected;
            proyecto.Costos = costos.CostosList;
            proyecto.RequerimientosInversion = inversiones.InversionesList;
            proyecto.RequerimientosReinversion = reinversiones.ReinversionesList;

            if (!proyecto.ConFinanciamiento)
            {
                ((TabItem)tcRegistrarProyecto.Items[9]).IsEnabled = false;
            }
            else
            {
                ((TabItem)tcRegistrarProyecto.Items[9]).IsEnabled = true;
            }

            ReloadProgress();
        }

        public void ReloadProgress()
        {
            List<bool> stepsProgress = new List<bool>(11);

            stepsProgress.Add(!proyecto.CodProyecto.Equals(0));
            stepsProgress.Add(!proponente.ProponenteSelected.IdProponente.Equals(0));
            stepsProgress.Add(!string.IsNullOrWhiteSpace(proyecto.CaraterizacionDelBienServicio));
            stepsProgress.Add(!inversiones.InversionesList.Count.Equals(0));
            stepsProgress.Add(!reinversiones.ReinversionesList.Count.Equals(0));
            stepsProgress.Add(!proyeccionVentas.ProyeccionesList.Count.Equals(0));
            stepsProgress.Add(!costos.CostosList.Count.Equals(0));
            stepsProgress.Add(!costos.CostosList.Count.Equals(0));

            if (!inversiones.InversionesList.Count.Equals(0) || !reinversiones.ReinversionesList.Count.Equals(0))
            {
                stepsProgress.Add(true);
            }
            else
            {
                stepsProgress.Add(false);
            }

            stepsProgress.Add(!financiamientoUc.FinanciamientoSelected.CodFinanciamiento.Equals(0));

            if (!inversiones.InversionesList.Count.Equals(0) || !reinversiones.ReinversionesList.Count.Equals(0) ||
                !proyeccionVentas.ProyeccionesList.Count.Equals(0) || !costos.CostosList.Count.Equals(0) ||
                !financiamientoUc.FinanciamientoSelected.CodFinanciamiento.Equals(0))
            {
                stepsProgress.Add(true);
            }
            else
            {
                stepsProgress.Add(false);
            }

            progreso.Reload(stepsProgress);
        }

        private void tcRegistrarProyecto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tcRegistrarProyecto.IsLoaded)
            {
                int indice = tcRegistrarProyecto.SelectedIndex;
                if (proyecto.CodProyecto.Equals(0) && !indice.Equals(0))
                {
                    System.Windows.MessageBox.Show("Por favor ingrese todos los datos de Información General y guardelos para poder avanzar a la siguiente pestaña",
                    "Datos vacios", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tcRegistrarProyecto.SelectedIndex = 0;
                }
                else if (indice > 1 && proyecto.Proponente.IdProponente.Equals(0))
                {
                    System.Windows.MessageBox.Show("Por favor ingrese todos los datos del Proponente y guardelos para poder avanzar a la siguiente pestaña",
                    "Datos vacios", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tcRegistrarProyecto.SelectedIndex = 1;
                }
                else if (indice > 2 && proyecto.CaraterizacionDelBienServicio.Equals(string.Empty))
                {
                    System.Windows.MessageBox.Show("Por favor ingrese todos los datos de Caracterización y guardelos para poder avanzar a la siguiente pestaña",
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

        private void LlenaFlujoCaja()
        {
            CastToDataGridView(capitalTrabajo.DTCapitalTrabajo.ToTable(),
                financiamientoUc.DTFinanciamiento.ToTable(), reinversiones.DTTotalesReinversiones.ToTable());

            DTFlujoCaja = DatatableBuilder.GenerarDTFlujoCaja(proyecto, capitalTrabajo.DTCapitalTrabajo, financiamientoUc.DTFinanciamiento, reinversiones.DTTotalesReinversiones,
                inversiones.InversionesTotal, capitalTrabajo.RecuperacionCT).AsDataView();

            LlenaCalculosFinales();
        }
        private void LlenaCalculosFinales()
        {
            double[] num = new double[ProyectoSelected.HorizonteEvaluacionEnAnos + 1];
            double[] numArray = new double[ProyectoSelected.HorizonteEvaluacionEnAnos];
            for (int i = 0; i <= ProyectoSelected.HorizonteEvaluacionEnAnos; i++)
            {
                num[i] = Convert.ToDouble(DTFlujoCaja.Table.Rows[16][i + 1].ToString().Replace("₡", string.Empty));
            }

            for (int k = 0; k < ProyectoSelected.HorizonteEvaluacionEnAnos; k++)
            {
                numArray[k] = num[k + 1];
            }

            montoInicial = num[0];
            flujoCaja = numArray;

            try
            {
                double num2 = num[0] + Financial.NPV(ProyectoSelected.TasaCostoCapital, ref numArray);
                VAN = string.Concat("₡ ", num2.ToString("#,##0.##"));
            }
            catch { VAN = "INDEFINIDA"; }

            try
            {
                double num1 = Financial.IRR(ref num, 0.3) * 100;

                TIR = string.Concat(num1.ToString("#,##0.##"), " %");
            }
            catch { TIR = "INDEFINIDA"; }
        }


        private List<DataGridView> gridView { get; set; }

        private void CastToDataGridView(DataTable capital, DataTable financiamiento, DataTable reinversiones)
        {
            gridView = new List<DataGridView>();
            DataGridView view = new DataGridView();
            view.DataSource = capital;
            gridView.Add(view);
            DataGridView view2 = new DataGridView();
            view.DataSource = financiamiento;
            gridView.Add(view2);
            DataGridView view3 = new DataGridView();
            view.DataSource = reinversiones;
            gridView.Add(view3);
        }

        private void dgFlujoCaja_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgFlujoCaja.Columns.Count > 0)
            {
                this.dgFlujoCaja.Columns[0].Width = 180;
            }
        }

        private void lbDetalleIndicadores_Click(object sender, RoutedEventArgs e)
        {
            double tir = Convert.ToDouble(TIR.Replace("%", string.Empty));
            double van = Convert.ToDouble(VAN.Replace("₡", string.Empty));
            IndicadoresFinales indicadores = new IndicadoresFinales(ProyectoSelected.CodProyecto, tir, van, montoInicial, flujoCaja);
            indicadores.ShowDialog();

            if (indicadores.IsActive == false && indicadores.Reload)
            {
                ProyectoSelected.TasaCostoCapital = proyectoData.GetProyecto(ProyectoSelected.CodProyecto).TasaCostoCapital;
                PropertyChanged(this, new PropertyChangedEventArgs("ProyectoSelected"));
                LlenaCalculosFinales();
            }
        }
    }
}
