using System.ComponentModel;
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
    /// Interaction logic for ctrl_Caracterizacion.xaml
    /// </summary>
    public partial class ctrl_Caracterizacion : UserControl, INotifyPropertyChanged
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private Proyecto proyectoSelected;
        private int codProyecto;

        private ProyectoData proyectoData;

        public ctrl_Caracterizacion()
        {
            InitializeComponent();
            DataContext = this;

            rtbDescripcionPoblacion.ToolTip = "Ingrese en este campo la descripción de la población que se beneficia con el proyecto";
            rtbDescripcionMercado.ToolTip = "Ingrese en este campo la descripción del mercado al que va dirigido el proyecto";
            rtbCaraterizacionBienServicio.ToolTip = "Ingrese en este campo una descripción detallada del bien o servicio en el que consiste el proyecto";
            rtbDescripcionSostenibilidad.ToolTip = "Ingrese en este campo la descripción sobre cómo va mantenerse sostenible este proyecto posterior al apoyo de la Institución";

            proyectoData = new ProyectoData();
            proyectoSelected = new Proyecto();
        }

        #region InternalMethods
        private void Reload()
        {
            proyectoSelected = proyectoData.GetProyecto(CodProyecto);
        }

        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(rtbDescripcionPoblacion.Text))
            {
                rtbDescripcionPoblacion.ToolTip = CAMPOREQUERIDO;
                rtbDescripcionPoblacion.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            if (string.IsNullOrWhiteSpace(rtbDescripcionMercado.Text))
            {
                rtbDescripcionMercado.ToolTip = CAMPOREQUERIDO;
                rtbDescripcionMercado.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            if (string.IsNullOrWhiteSpace(rtbCaraterizacionBienServicio.Text))
            {
                rtbCaraterizacionBienServicio.ToolTip = CAMPOREQUERIDO;
                rtbCaraterizacionBienServicio.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            if (string.IsNullOrWhiteSpace(rtbDescripcionSostenibilidad.Text))
            {
                rtbDescripcionSostenibilidad.ToolTip = CAMPOREQUERIDO;
                rtbDescripcionSostenibilidad.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            return validationResult;
        }
        #endregion

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
        #endregion

        #region Events
        private void btnGuardarCaracterizacion(object sender, RoutedEventArgs e)
        {
            if (!ProyectoSelected.TipoProyecto.CodTipo.Equals(2))
            {
                if (!ValidateRequiredFields())
                {
                    if (proyectoData.EditarProyectoCaracterizacion(ProyectoSelected))
                    {
                        //success
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.Caracterizacion);
                        MessageBox.Show(Constantes.ACTUALIZARPROYECTOMSG, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //error
                        MessageBox.Show(Constantes.ACTUALIZARPROYECTOERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(Constantes.ACTUALIZARPROYECTORESTRTIPOAMBIENTAL, Constantes.ACTUALIZARPROYECTOTLT, 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void rtbDescripcionPoblacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rtbDescripcionPoblacion.BorderBrush == Brushes.Red)
            {
                rtbDescripcionPoblacion.BorderBrush = Brushes.Gray;
                rtbDescripcionPoblacion.ToolTip = "Ingrese en este campo la descripción de la población que se beneficia con el proyecto";
            }
        }

        private void rtbDescripcionMercado_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rtbDescripcionMercado.BorderBrush == Brushes.Red)
            {
                rtbDescripcionMercado.BorderBrush = Brushes.Gray;
                rtbDescripcionMercado.ToolTip = "Ingrese en este campo la descripción del mercado al que va dirigido el proyecto";
            }
        }

        private void rtbCaraterizacionBienServicio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rtbCaraterizacionBienServicio.BorderBrush == Brushes.Red)
            {
                rtbCaraterizacionBienServicio.BorderBrush = Brushes.Gray;
                rtbCaraterizacionBienServicio.ToolTip = "Ingrese en este campo una descripción detallada del bien o servicio en el que consiste el proyecto";
            }
        }

        private void rtbDescripcionSostenibilidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rtbDescripcionSostenibilidad.BorderBrush == Brushes.Red)
            {
                rtbDescripcionSostenibilidad.BorderBrush = Brushes.Gray;
                rtbDescripcionSostenibilidad.ToolTip = "Ingrese en este campo la descripción sobre cómo va mantenerse sostenible este proyecto posterior al apoyo de la Institución";
            }
        }

        #endregion
    }
}
