using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for Caracterizacion.xaml
    /// </summary>
    public partial class Caracterizacion : UserControl
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private Proyecto proyectoSelected;
        private int codProyecto;

        private ProyectoData proyectoData;

        public Caracterizacion()
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

        public void Reload()
        {
            proyectoSelected = proyectoData.GetProyecto(CodProyecto);
        }

        #region Fields
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
            }
        }
        #endregion

        #region Events
        private void btnGuardarCaracterizacion(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (proyectoData.ActualizarProyectoCaracterizacion(ProyectoSelected))
                {
                    //success
                    RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                    mainWindow.ReloadUserControls(ProyectoSelected.CodProyecto);
                    MessageBox.Show("El proyecto se ha actualizado correctamente", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //error
                    MessageBox.Show("Ha ocurrido un error al actualizar el proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbDescripcionPoblacion.Text))
            {
                rtbDescripcionPoblacion.ToolTip = CAMPOREQUERIDO;
                rtbDescripcionPoblacion.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            if (string.IsNullOrWhiteSpace(tbDescripcionMercado.Text))
            {
                rtbDescripcionMercado.ToolTip = CAMPOREQUERIDO;
                rtbDescripcionMercado.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            if (string.IsNullOrWhiteSpace(tbCaracterizacionBienServicio.Text))
            {
                rtbCaraterizacionBienServicio.ToolTip = CAMPOREQUERIDO;
                rtbCaraterizacionBienServicio.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            if (string.IsNullOrWhiteSpace(tbDescripcionSostenibilidad.Text))
            {
                rtbDescripcionSostenibilidad.ToolTip = CAMPOREQUERIDO;
                rtbDescripcionSostenibilidad.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            return validationResult;
        }

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
