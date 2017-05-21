using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
using UCR.Negotium.Utils;

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
            proyectoSelected = new Proyecto();
            proyectos = proyectoData.GetProyectos().OrderByDescending(proy => proy.CodProyecto).ToList();
            ProyectoSelected = Proyectos.FirstOrDefault();

            cbFiltro.SelectedValue = Estados.First();

            Reload();
        }

        private void Reload()
        {
            Proyectos = proyectos;
            ProyectoSelected = Proyectos.FirstOrDefault();
        }

        private void tbBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarProyectos();
        }

        private void FiltrarProyectos()
        {
            List<Proyecto> newFilter = new List<Proyecto>();

            if (cbFiltro.SelectedValue.Equals("Activos"))
                newFilter = proyectos.Where(proy => proy.Archivado.Equals(false)).ToList();

            else if (cbFiltro.SelectedValue.Equals("Archivados"))
                newFilter = proyectos.Where(proy => proy.Archivado.Equals(true)).ToList();
            else
                newFilter = proyectos;

            string textoBusqueda = tbBusqueda.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(textoBusqueda))
                Proyectos = newFilter.Where(proy => proy.NombreProyecto.ToLower().Contains(textoBusqueda)
                    || proy.Proponente.Nombre != null && proy.Proponente.ToString().ToLower().Contains(textoBusqueda)
                    || proy.Proponente.Nombre != null && proy.Proponente.Organizacion.NombreOrganizacion.ToLower().Contains(textoBusqueda)
                    ).ToList();
            else
                Proyectos = newFilter;

            ProyectoSelected = Proyectos.FirstOrDefault();
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
                PropertyChanged(this, new PropertyChangedEventArgs("ProyectoSelected"));
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

        public List<string> Estados
        {
            get { return new List<string>() { "Todos", "Activos", "Archivados" }; }
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            PoseeEncargadoConfirm poseeEncargadoConfirm = new PoseeEncargadoConfirm();
            Close();
            poseeEncargadoConfirm.Show();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ProyectoSelected != null)
            {
                if (ProyectoSelected.Archivado)
                {
                    MessageBox.Show("Este proyecto no puede ser abierto, verifique que el proyecto sea Activo", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow(ProyectoSelected.CodProyecto);
                    Close();
                    registrarProyecto.Show();
                }
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if(ProyectoSelected != null)
            {
                //GenerarReporte reporte = new GenerarReporte(ProyectoSelected);
                //reporte.CrearReporte();
            }
        }

        private void btnArchivar_Click(object sender, RoutedEventArgs e)
        {
            if(ProyectoSelected != null && !ProyectoSelected.Archivado)
            {
                if (MessageBox.Show("Esta seguro que desea archivar este Proyecto?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if(proyectoData.ArchivarProyecto(ProyectoSelected.CodProyecto, true))
                        Reload();
                }
            }
        }

        private void btnReabrir_Click(object sender, RoutedEventArgs e)
        {
            if (ProyectoSelected != null && ProyectoSelected.Archivado)
            {
                if (proyectoData.ArchivarProyecto(ProyectoSelected.CodProyecto, false))
                    Reload();
            }
        }

        private void cbFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarProyectos();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
