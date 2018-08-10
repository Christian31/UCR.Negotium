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
        private bool usoAcademico;

        public MainWindow()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
            usoAcademico = ConfigurationHelper.EsDeUsoAcademico();

            proyectoData = new ProyectoData();
            tipoProyectoData = new TipoProyectoData();
            proyectos = proyectosFiltrados = new List<Proyecto>();
            proyectoSelected = new Proyecto();
            tipoProyectos = new List<TipoProyecto>();

            tipoProyectos.Add(new TipoProyecto() { Nombre = "Todos" });
            tipoProyectos.AddRange(tipoProyectoData.GetTipoProyectos());

            Reload();

            if (usoAcademico)
            {
                btnReabrir.Visibility = Visibility.Hidden;
                cbEstado.Visibility = Visibility.Hidden;
                lblEstado.Visibility = Visibility.Hidden;
                FiltrarProyectos(init: true);
            }
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
                    MessageBox.Show(Constantes.ABRIRPROYECTOINACTIVO, Constantes.ADVERTENCIATLT, MessageBoxButton.OK, 
                        MessageBoxImage.Warning);
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
                string message = ConfigurationHelper.EsDeUsoAcademico() ?
                    Constantes.ARCHIVARPROYECTOACADEMICO :
                    Constantes.ARCHIVARPROYECTOPROFESIONAL;
                if (CustomMessageBox.Show(Constantes.ARCHIVARPROYECTO + message))
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
            if (proyectos.Count.Equals(0))
            {
                MessageBox.Show(Constantes.IMPORTARDATOSNODISPONIBLEMSG, Constantes.IMPORTARDATOSTLT, 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string fileName = GestorDatos.ImportarDatos();
                MessageBox.Show(string.Format(Constantes.IMPORTARDATOSEXITOSO, fileName),
                    Constantes.IMPORTARDATOSTLT, MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
                        MessageBox.Show(Constantes.EXPORTARDATOSEXITOSOMSG, Constantes.EXPORTARDATOSTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(Constantes.EXPORTARDATOSERRORMSG, Constantes.EXPORTARDATOSTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(Constantes.EXPORTARDATOSINCOMPATIBLEMSG, Constantes.EXPORTARDATOSTLT, 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void menuItemAcerca_Click(object sender, RoutedEventArgs e)
        {
            AcercaNegotium acercaDe = new AcercaNegotium();
            acercaDe.ShowDialog();
        }

        private void menuItemReferencias_Click(object sender, RoutedEventArgs e)
        {
            ReferenciasNegotium refNegotium = new ReferenciasNegotium();
            refNegotium.ShowDialog();
        }

        private void menuItemCreditos_Click(object sender, RoutedEventArgs e)
        {
            CreditosNegotium creditos = new CreditosNegotium();
            creditos.ShowDialog();
        }
        #endregion

        #region InternalMethods
        private void Reload()
        {
            Proyectos = proyectos = proyectoData.GetProyectos().OrderByDescending(proy => proy.CodProyecto).ToList();
            if (!usoAcademico)
            {
                cbEstado.SelectedValue = Estados.First();
                ProyectoSelected = Proyectos.FirstOrDefault();
            }
            
            cbTipoProyecto.SelectedValue = 0;
        }

        private void FiltrarProyectos(bool init = false)
        {
            List<Proyecto> newFilter = new List<Proyecto>();
            string textoBusqueda = "";
            if (!init)
            {
                string filtroEstado = "";
                if (usoAcademico)
                    filtroEstado = "Activos";
                else
                    filtroEstado = cbEstado.SelectedValue.ToString();

                newFilter = filtroEstado.Equals("Todos") ? proyectos :
                    proyectos.Where(proy => proy.Archivado.Equals(filtroEstado.Equals("Archivados"))).ToList();

                if (!cbTipoProyecto.SelectedValue.Equals(0))
                {
                    newFilter = newFilter.Where(proy => proy.TipoProyecto.CodTipo.Equals(cbTipoProyecto.SelectedValue)).ToList();
                }

                textoBusqueda = tbBusqueda.Text.ToLower();
            }
            else
            {
                newFilter = proyectos.Where(proy => !proy.Archivado).ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(textoBusqueda))
                Proyectos = newFilter.Where(proy => proy.NombreProyecto.ToLower().Contains(textoBusqueda)
                    || proy.OrganizacionProponente.Proponente.Nombre != null && proy.OrganizacionProponente.Proponente.ToString().ToLower().Contains(textoBusqueda)
                    || proy.OrganizacionProponente.Proponente.Nombre != null && proy.OrganizacionProponente.NombreOrganizacion.ToLower().Contains(textoBusqueda)
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
