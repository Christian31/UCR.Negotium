using MahApps.Metro.Controls;
using UCR.Negotium.Utils;
using System.Data;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for RegistrarProyectoWindow.xaml
    /// </summary>
    public partial class RegistrarProyectoWindow : MetroWindow, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private DataView dtFlujoCaja;
        private ProyectoData proyectoData;
        private ProponenteData proponenteData;
        private EncargadoData encargadoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public RegistrarProyectoWindow(int codProyecto = 0, int codEncargado=0)
        {
            DataContext = this;
            InitializeComponent();

            proyectoData = new ProyectoData();         
            proponenteData = new ProponenteData();
            encargadoData = new EncargadoData();

            dtFlujoCaja = new DataView();
            proyecto = new Proyecto();

            if (!codProyecto.Equals(0))
            {
                proyecto = proyectoData.GetProyecto(codProyecto);
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

        public int CodProyecto { get; set; }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            if(System.Windows.MessageBox.Show("Esta seguro que desea cerrar esta ventana?", "Confirmar", 
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
            proyecto.Proponente = proponenteData.GetProponente(codProyecto);

            if (!proyecto.ConFinanciamiento)
            {
                ((TabItem)tcRegistrarProyecto.Items[9]).IsEnabled = false;
            }
            else
            {
                ((TabItem)tcRegistrarProyecto.Items[9]).IsEnabled = true;
            }
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
                //else if (indice.Equals(10))
                //{
                //    //llenar flujo de caja 
                //    CastToDataGridView(capitalTrabajo.DTCapitalTrabajo.ToTable(),
                //financiamientoUc.DTFinanciamiento.ToTable(), reinversiones.DTTotalesReinversiones.ToTable());

                //    DTFlujoCaja = DatatableBuilder.GenerarDTFlujoCaja(proyecto, gridView[0], gridView[1], gridView[2],
                //        inversiones.InversionesTotal, capitalTrabajo.RecuperacionCT).AsDataView();
                //}
            }
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
    }
}
