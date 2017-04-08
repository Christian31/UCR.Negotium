using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        private List<Proyecto> proyectos, proyectosFiltrados;
        private Proyecto proyectoSelected;
        private ProyectoData proyectoData;

        public MainWindow()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            proyectoData = new ProyectoData();
            proyectos = proyectosFiltrados = new List<Proyecto>();
            proyectos = proyectosFiltrados = proyectoData.GetProyectosAux();
            proyectoSelected = new Proyecto();
            proyectoSelected = proyectosFiltrados.FirstOrDefault();
        }

        private void tbBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoBusqueda = tbBusqueda.Text.ToLower();
            Proyectos = proyectos.Where(proy => proy.NombreProyecto.ToLower().Contains(textoBusqueda)
            || proy.Proponente.ToString().ToLower().Contains(textoBusqueda)
            || proy.Proponente.Organizacion.NombreOrganizacion.ToLower().Contains(textoBusqueda)
            ).ToList();
        }

        public Proyecto ProyectoSelected
        {
            get
            {
                return proyectoSelected;
            }
            set
            {
                proyectoSelected = value;
            }
        }

        public List<Proyecto> Proyectos
        {
            get
            {
                return proyectosFiltrados;
            }
            set
            {
                proyectosFiltrados = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Proyectos"));
            }
        }

        private void menuItemEditar_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void menuItemImprimir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow();
            Close();
            registrarProyecto.Show();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow(ProyectoSelected.CodProyecto);
            Close();
            registrarProyecto.Show();
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            //GenerarReporte reporte = new GenerarReporte(ProyectoSelected);
            //reporte.CrearReporte();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
