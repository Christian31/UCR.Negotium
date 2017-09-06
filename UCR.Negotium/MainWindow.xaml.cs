using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

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
        private TipoProyectoData tipoProyectoData;
        private List<TipoProyecto> tipoProyectos;

        public MainWindow()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            proyectoData = new ProyectoData();
            tipoProyectoData = new TipoProyectoData();
            proyectos = proyectosFiltrados = new List<Proyecto>();
            proyectoSelected = new Proyecto();
            tipoProyectos = new List<TipoProyecto>();

            tipoProyectos.Add(new TipoProyecto() { Nombre = "Todos" });
            tipoProyectos.AddRange(tipoProyectoData.GetTipoProyectos());

            Reload();
        }

        #region Properties
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

        public List<TipoProyecto> TiposProyectos
        {
            get { return tipoProyectos; }
            set { tipoProyectos = value; }
        }

        public List<string> Estados
        {
            get { return new List<string>() { "Todos", "Activos", "Archivados" }; }
        }
        #endregion

        #region Events
        private void tbBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarProyectos();
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
                    MessageBox.Show("Este proyecto no puede ser abierto, verifique que el proyecto esté Activo", "Advertencia",
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

        private void btnArchivar_Click(object sender, RoutedEventArgs e)
        {
            if (ProyectoSelected != null && !ProyectoSelected.Archivado)
            {
                if (MessageBox.Show("Esta seguro que desea archivar este Proyecto?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (proyectoData.ArchivarProyecto(ProyectoSelected.CodProyecto, true))
                    {
                        Reload();
                        FiltrarProyectos();
                    }
                }
            }
        }

        private void btnReabrir_Click(object sender, RoutedEventArgs e)
        {
            if (ProyectoSelected != null && ProyectoSelected.Archivado)
            {
                if (proyectoData.ArchivarProyecto(ProyectoSelected.CodProyecto, false))
                {
                    Reload();
                    FiltrarProyectos();
                }
            }
        }

        private void cbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarProyectos();
        }

        private void cbTipoProyecto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarProyectos();
        }

        private void menuItemImport_Click(object sender, RoutedEventArgs e)
        {
            string fileName = GestorDatos.ImportarDatos();
            MessageBox.Show(string.Format("El respaldo de los datos ha sido guardado en su escritorio con el siguiente nombre: {0} \n " +
                "Por favor guarde el archivo en un lugar seguro para mantener el respaldo a salvo", fileName),
                "Respaldo Creado", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void menuItemExport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog exportDialog = new OpenFileDialog();
            exportDialog.Filter = "BAK Files (.bak)|*.bak";
            exportDialog.Multiselect = false;
            bool? userClickedOK = exportDialog.ShowDialog();
            if (userClickedOK == true)
            {
                string backupContent;
                System.IO.Stream fileStream = exportDialog.OpenFile();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    backupContent = reader.ReadLine();
                }

                if (!backupContent.Equals("") && GestorDatos.ValidateBackup(backupContent))
                {
                    if (GestorDatos.ExportarDatos(backupContent))
                    {
                        this.Reload();
                    }
                }
                else
                {
                    MessageBox.Show("El respaldo de que está intentando de utilizar es incompatible con la version actual del Negotium \n " +
                "Por favor asegurese de utilizar un respaldo creado en una versión anterior o igual al Negotium instalado",
                "Respaldo Exportado", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void menuItemAcerca_Click(object sender, RoutedEventArgs e)
        {
            AcercaNegotium acercaDe = new AcercaNegotium();
            acercaDe.ShowDialog();
        }
        #endregion

        #region InternalMethods
        private void Reload()
        {
            Proyectos = proyectos = proyectoData.GetProyectos().OrderByDescending(proy => proy.CodProyecto).ToList();
            ProyectoSelected = Proyectos.FirstOrDefault();

            cbTipoProyecto.SelectedValue = 0;
            cbEstado.SelectedValue = Estados.First();
        }

        private void FiltrarProyectos()
        {
            List<Proyecto> newFilter = new List<Proyecto>();

            newFilter = cbEstado.SelectedValue.Equals("Todos") ? proyectos :
                proyectos.Where(proy => proy.Archivado.Equals(cbEstado.SelectedValue.Equals("Archivados"))).ToList();

            string textoBusqueda = tbBusqueda.Text.ToLower();

            if (!cbTipoProyecto.SelectedValue.Equals(0))
            {
                newFilter = newFilter.Where(proy => proy.TipoProyecto.CodTipo.Equals(cbTipoProyecto.SelectedValue)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(textoBusqueda))
                Proyectos = newFilter.Where(proy => proy.NombreProyecto.ToLower().Contains(textoBusqueda)
                    || proy.Proponente.Nombre != null && proy.Proponente.ToString().ToLower().Contains(textoBusqueda)
                    || proy.Proponente.Nombre != null && proy.Proponente.Organizacion.NombreOrganizacion.ToLower().Contains(textoBusqueda)
                    ).ToList();
            else
                Proyectos = newFilter;

            ProyectoSelected = Proyectos.FirstOrDefault();
        }
        #endregion

        private void menuItemDocumentacion_Click(object sender, RoutedEventArgs e)
        {

        }

        

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
