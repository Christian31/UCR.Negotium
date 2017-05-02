using MahApps.Metro.Controls;
using UCR.Negotium.Utils;
using System.Data;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for RegistrarProyectoWindow.xaml
    /// </summary>
    public partial class RegistrarProyectoWindow : MetroWindow, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private ProyectoData proyectoData;
        private ProponenteData proponenteData;
        private EncargadoData encargadoData;
        
        private ProyeccionVentaArticuloData proyeccionData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public RegistrarProyectoWindow(int codProyecto = 0, int codEncargado=0)
        {
            DataContext = this;
            InitializeComponent();

            proyectoData = new ProyectoData();         
            proyeccionData = new ProyeccionVentaArticuloData();
            proponenteData = new ProponenteData();
            encargadoData = new EncargadoData();

            proyecto = new Proyecto();
            if (!codProyecto.Equals(0))
            {
                proyecto = proyectoData.GetProyecto(codProyecto);
                ReloadUserControls(proyecto.CodProyecto);

                proyecto.Proyecciones = proyeccionData.GetProyeccionesVentaArticulo(codProyecto);
            }

            //Que hacer con el encargado
            //TODDO HERE
            if (!codEncargado.Equals(0))
            {
                proyecto.Encargado = encargadoData.GetEncargado(codEncargado);
            }
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

        public int CodProyecto { get; set; }        

        public DataView DTProyeccionesVentas
        {
            get
            {
                return DatatableBuilder.GenerarDTIngresosGenerados(ProyectoSelected).AsDataView();
            }
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            if(MessageBox.Show("Esta seguro que desea cerrar esta ventana?", "Confirmar", 
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
                    costos.CodProyecto = codProyecto;

            proyecto = proyectoData.GetProyecto(codProyecto);
            proyecto.Proponente = proponenteData.GetProponente(codProyecto);
        }

        private void tcRegistrarProyecto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tcRegistrarProyecto.IsLoaded)
            {
                int indice = tcRegistrarProyecto.SelectedIndex;
                if (proyecto.CodProyecto.Equals(0) && !indice.Equals(0))
                {
                    MessageBox.Show("Por favor ingrese todos los datos de Información General y guardelos para poder avanzar a la siguiente pestaña",
                    "Datos vacios", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tcRegistrarProyecto.SelectedIndex = 0;
                }
                else if (indice > 1 && proyecto.Proponente.IdProponente.Equals(0))
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
                else if (indice.Equals(11))
                {
                    //llenar flujo de caja 
                }
            }
        }
    }
}
