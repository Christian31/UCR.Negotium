using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Enums;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_InformacionGeneral.xaml
    /// </summary>
    public partial class ctrl_InformacionGeneral : UserControl, INotifyPropertyChanged
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private Proyecto proyectoSelected;
        private List<UnidadMedida> unidadMedidas;
        private List<TipoProyecto> tipoProyectos;
        private List<Provincia> provincias;
        private Provincia provinciaSelected;
        private Canton cantonSelected;
        private int codProyecto;

        private UnidadMedidaData unidadMedidaData;
        private ProvinciaData provinciaData;
        private TipoProyectoData tipoProyectoData;
        private ProyectoData proyectoData;

        public ctrl_InformacionGeneral()
        {
            InitializeComponent();
            DataContext = this;
            proyectoSelected = new Proyecto();
            provinciaSelected = new Provincia();
            cantonSelected = new Canton();

            unidadMedidaData = new UnidadMedidaData();
            provinciaData = new ProvinciaData();
            tipoProyectoData = new TipoProyectoData();
            proyectoData = new ProyectoData();

            unidadMedidas = new List<UnidadMedida>();
            unidadMedidas = unidadMedidaData.GetUnidadesMedidas();

            provincias = new List<Provincia>();
            provincias = provinciaData.GetProvincias();

            tipoProyectos = new List<TipoProyecto>();
            tipoProyectos = tipoProyectoData.GetTipoProyectos();

            proyectoSelected.AnoInicial = 2000;
            proyectoSelected.HorizonteEvaluacionEnAnos = 2;
        }

        #region Properties
        public int CodProyecto
        {
            get { return codProyecto; }
            set { codProyecto = value; Reload(); }
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
            get { return provincias; }
            set { provincias = value; }
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
            get { return unidadMedidas; }
            set { unidadMedidas = value; }
        }

        public List<TipoProyecto> TipoProyectos
        {
            get { return tipoProyectos; }
            set { tipoProyectos = value; }
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
                        LocalContext.ReloadUserControls(idProyecto, Modulo.InformacionGeneral);
                        MessageBox.Show(Constantes.ACTUALIZARPROYECTOMSG, Constantes.ACTUALIZARPROYECTOTLT,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(Constantes.ACTUALIZARPROYECTOERROR, Constantes.ACTUALIZARPROYECTOTLT,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (proyectoData.EditarProyecto(ProyectoSelected))
                    {
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.InformacionGeneral);
                        MessageBox.Show(Constantes.ACTUALIZARPROYECTOMSG, Constantes.ACTUALIZARPROYECTOTLT,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(Constantes.ACTUALIZARPROYECTOERROR, Constantes.ACTUALIZARPROYECTOTLT,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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

        private void cbTipoAnalisis_Loaded(object sender, RoutedEventArgs e)
        {
            TipoProyecto tipoProyectoSelected = (TipoProyecto)cbTipoAnalisis.SelectedItem;
            if (tipoProyectoSelected == null || tipoProyectoSelected.CodTipo.Equals(0))
            {
                cbTipoAnalisis.SelectedIndex = 0;
            }
        }
        #endregion

        #region InternalMethods
        public void AddEvaluador(int codEvaluador)
        {
            ProyectoSelected.Encargado.IdEncargado = codEvaluador;
        }

        private void Reload()
        {
            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);

            if (codProyecto != 0)
            {
                nudAnoInicial.IsEnabled = nudHorizonteEvaluacion.IsEnabled = 
                    cbTipoAnalisis.IsEnabled = false;
            }

            if (ProyectoSelected.TipoProyecto.CodTipo != 1)
            {
                cbNoPaga.IsEnabled = cbSiPaga.IsEnabled = nudPorcentaje.IsEnabled = false;
            }

            if (ProyectoSelected.TipoProyecto.CodTipo == 2)
            {
                cbNoPoseeFinan.IsEnabled = cbSiPoseeFinan.IsEnabled = false;
            }
        }

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

            //Si analisis es completo
            if (ProyectoSelected.TipoProyecto.CodTipo.Equals(1))
            {
                if (string.IsNullOrWhiteSpace(tbBienServicio.Text))
                {
                    tbBienServicio.ToolTip = CAMPOREQUERIDO;
                    tbBienServicio.BorderBrush = Brushes.Red;
                    validationResult = true;
                }
            }
            
            return validationResult;
        }
        #endregion

        private void cbTipoAnalisis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTipoAnalisis.SelectedValue != null)
            {
                switch ((int)cbTipoAnalisis.SelectedValue)
                {
                    case 1:
                        nudAnoInicial.IsEnabled = nudHorizonteEvaluacion.IsEnabled =
                            cbNoPaga.IsEnabled = cbSiPaga.IsEnabled =
                            nudPorcentaje.IsEnabled = cbNoPoseeFinan.IsEnabled =
                            cbSiPoseeFinan.IsEnabled = true;
                        break;
                    case 2:
                        nudAnoInicial.IsEnabled = nudHorizonteEvaluacion.IsEnabled =
                            cbNoPaga.IsEnabled = cbSiPaga.IsEnabled =
                            nudPorcentaje.IsEnabled = cbNoPoseeFinan.IsEnabled =
                            cbSiPoseeFinan.IsEnabled = false;
                        break;
                    case 3:
                        nudAnoInicial.IsEnabled = nudHorizonteEvaluacion.IsEnabled =
                            cbSiPoseeFinan.IsEnabled = cbNoPoseeFinan.IsEnabled = true;

                        cbNoPaga.IsEnabled = cbSiPaga.IsEnabled = nudPorcentaje.IsEnabled = false;
                        break;
                }
            }
        }
    }
}
