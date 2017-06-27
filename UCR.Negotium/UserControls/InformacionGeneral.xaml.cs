using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for InformacionGeneral.xaml
    /// </summary>
    public partial class InformacionGeneral : UserControl, INotifyPropertyChanged
    {

        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private Proyecto proyectoSelected;
        private List<UnidadMedida> unidadMedidas;
        private List<Provincia> provincias;
        private Provincia provinciaSelected;
        private Canton cantonSelected;
        private int codProyecto;

        private UnidadMedidaData unidadMedidaData;
        private ProvinciaData provinciaData;
        private ProyectoData proyectoData;

        public InformacionGeneral()
        {
            InitializeComponent();
            DataContext = this;
            proyectoSelected = new Proyecto();
            provinciaSelected = new Provincia();
            cantonSelected = new Canton();

            unidadMedidaData = new UnidadMedidaData();
            provinciaData = new ProvinciaData();
            proyectoData = new ProyectoData();

            unidadMedidas = new List<UnidadMedida>();
            unidadMedidas = unidadMedidaData.GetUnidadesMedidaAux();

            provincias = new List<Provincia>();
            provincias = provinciaData.GetProvincias();

            proyectoSelected.AnoInicial = 2000;
            proyectoSelected.HorizonteEvaluacionEnAnos = 2;
        }

        public void Reload()
        {
            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);
        }

        #region Properties
        public int CodProyecto
        {
            get
            {
                return codProyecto;
            }
            set
            {
                codProyecto = value;
                Reload();
            }
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

        public List<Provincia> Provincias
        {
            get
            {
                return provincias;
            }
            set
            {
                provincias = value;
            }
        }

        public Provincia ProvinciaSelected
        {
            get
            {
                return provinciaSelected;
            }
            set
            {
                provinciaSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ProvinciaSelected"));
            }
        }

        public Canton CantonSelected
        {
            get
            {
                return cantonSelected;
            }
            set
            {
                cantonSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CantonSelected"));
            }
        }

        public List<UnidadMedida> UnidadMedidas
        {
            get
            {
                return unidadMedidas;
            }
            set
            {
                unidadMedidas = value;
            }
        }
        #endregion

        #region Events
        private void btnGuardarInfoGeneral(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (ProyectoSelected.CodProyecto.Equals(0))
                {
                    int idProyecto = proyectoData.InsertarProyecto(ProyectoSelected);

                    if (idProyecto != -1)
                    {
                        ProyectoSelected.CodProyecto = idProyecto;
                        RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                        mainWindow.ReloadUserControls(idProyecto);

                        MessageBox.Show("El proyecto se ha insertado correctamente", "Proyecto Insertado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error al insertar el proyecto, verifique que los datos ingresados sean correctos", "Proyecto Insertado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (proyectoData.ActualizarProyecto(ProyectoSelected))
                    {
                        RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                        mainWindow.ReloadUserControls(CodProyecto);
                        MessageBox.Show("El proyecto se ha actualizado correctamente", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error al actualizar el proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //private void cbConIngresos_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (cbConIngresos.IsChecked.Value.Equals(false))
        //    {
        //        cbSinIngresos.IsChecked = true;
        //    }
        //}

        //private void cbConIngresos_Checked(object sender, RoutedEventArgs e)
        //{
        //    cbSinIngresos.IsChecked = false;
        //}

        //private void cbSinIngresos_Checked(object sender, RoutedEventArgs e)
        //{
        //    cbConIngresos.IsChecked = false;
        //}

        private void cbPagaImpuestos_Checked(object sender, RoutedEventArgs e)
        {
            cbNoPagaImpuestos.IsChecked = false;
        }

        private void cbNoPagaImpuestos_Checked(object sender, RoutedEventArgs e)
        {
            cbPagaImpuestos.IsChecked = false;
        }

        private void cbPagaImpuestos_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbPagaImpuestos.IsChecked.Value.Equals(false))
            {
                cbNoPagaImpuestos.IsChecked = true;
            }
        }

        private void cbProvincias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Provincia provinciaSelected = (Provincia)cbProvincias.SelectedItem;
            if (provinciaSelected == null || provinciaSelected.CodProvincia.Equals(0))
            {
                cbProvincias.SelectedIndex = 0;
                provinciaSelected = Provincias.FirstOrDefault();
            }
            ProvinciaSelected = provinciaSelected;
            LoadCantones();
        }

        private void cbCanton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCantones();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void cbProvincias_Loaded(object sender, RoutedEventArgs e)
        {
            Provincia provinciaSelected = (Provincia)cbProvincias.SelectedItem;
            if (provinciaSelected == null || provinciaSelected.CodProvincia.Equals(0))
            {
                cbProvincias.SelectedIndex = 0;
                provinciaSelected = Provincias.FirstOrDefault();
            }
            ProvinciaSelected = provinciaSelected;
            LoadCantones();
        }

        private void tbNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNombre.BorderBrush == Brushes.Red)
            {
                tbNombre.BorderBrush = Brushes.Gray;
                tbNombre.ToolTip = "Ingrese en este campo el nombre del Proyecto";
            }
        }

        private void tbBienServicio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbBienServicio.BorderBrush == Brushes.Red)
            {
                tbBienServicio.BorderBrush = Brushes.Gray;
                tbBienServicio.ToolTip = "Ingrese en este campo el Bien o Servicio del Proyecto";
            }
        }
        #endregion

        #region InternalMethods
        private void LoadCantones()
        {
            Canton cantonSelected = (Canton)cbCanton.SelectedItem;
            if (cantonSelected == null || cantonSelected.CodCanton.Equals(0))
            {
                cbCanton.SelectedIndex = 0;
                cantonSelected = ProvinciaSelected.Cantones.FirstOrDefault();
            }
            CantonSelected = cantonSelected;

            Distrito distritoSelected = (Distrito)cbDistrito.SelectedItem;
            if (distritoSelected == null || distritoSelected.CodDistrito.Equals(0))
            {
                cbDistrito.SelectedIndex = 0;
            }
        }

        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbNombre.Text))
            {
                tbNombre.ToolTip = CAMPOREQUERIDO;
                tbNombre.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            if (string.IsNullOrWhiteSpace(tbBienServicio.Text))
            {
                tbBienServicio.ToolTip = CAMPOREQUERIDO;
                tbBienServicio.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            
            return validationResult;
        }
        #endregion

        private void cbConFinanciamiento_Checked(object sender, RoutedEventArgs e)
        {
            cbSinFinanciamiento.IsChecked = false;
        }

        private void cbConFinanciamiento_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbConFinanciamiento.IsChecked.Value.Equals(false))
            {
                cbSinFinanciamiento.IsChecked = true;
            }
        }

        private void cbSinFinanciamiento_Checked(object sender, RoutedEventArgs e)
        {
            cbConFinanciamiento.IsChecked = false;
        }
    }
}
