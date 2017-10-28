using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_Reinversiones.xaml
    /// </summary>
    public partial class ctrl_Reinversiones : UserControl, INotifyPropertyChanged
    {
        private Reinversion reinversionSelected;
        private List<Reinversion> reinversiones;
        private int codProyecto;
        private Proyecto proyecto;
        private DataView totalesReinversiones;

        private string signoMoneda;

        private ReinversionData reinversionData;
        private ProyectoData proyectoData;

        public ctrl_Reinversiones()
        {
            InitializeComponent();
            DataContext = this;

            reinversionData = new ReinversionData();
            proyectoData = new ProyectoData();

            proyecto = new Proyecto();
            reinversionSelected = new Reinversion();
            reinversiones = new List<Reinversion>();
            totalesReinversiones = new DataView();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region InternalMethods
        public void Reload()
        {
            DTTotalesReinversiones = new DataView();

            SignoMoneda = LocalContext.GetSignoMoneda(CodProyecto);
            ReinversionesList = reinversionData.GetReinversiones(CodProyecto);

            ReinversionesList.All(reinv =>
            {
                reinv.CostoUnitarioFormat = SignoMoneda + " " + reinv.CostoUnitario.ToString("#,##0.##");
                reinv.SubtotalFormat = SignoMoneda + " " + reinv.Subtotal.ToString("#,##0.##");
                return true;
            });

            dgtxcCostoUnidad.Header = string.Format("Costo Unitario ({0})", SignoMoneda);
            dgtxcSubtotal.Header = string.Format("Subtotal ({0})", SignoMoneda);

            ReinversionSelected = ReinversionesList.FirstOrDefault();
            PropertyChanged(this, new PropertyChangedEventArgs("ReinversionesList"));

            proyecto = proyectoData.GetProyecto(CodProyecto);
            proyecto.RequerimientosReinversion = ReinversionesList;

            if (!proyecto.RequerimientosReinversion.Count.Equals(0))
            {
                DTTotalesReinversiones = DatatableBuilder.GenerarReinversionesTotales(proyecto).AsDataView();
            }
        }
        #endregion

        #region Properties
        public string SignoMoneda
        {
            get { return signoMoneda; }
            set { signoMoneda = value; }
        }

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

        public Reinversion ReinversionSelected
        {
            get
            {
                return reinversionSelected;
            }
            set
            {
                reinversionSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ReinversionSelected"));
            }
        }

        public List<Reinversion> ReinversionesList
        {
            get
            {
                return reinversiones;
            }
            set
            {
                reinversiones = value;
                
            }
        }

        public DataView DTTotalesReinversiones
        {
            get
            {
                return totalesReinversiones;
            }
            set
            {
                totalesReinversiones = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DTTotalesReinversiones"));
            }
        }
        #endregion

        #region Events
        private void btnAgregarReinversion_Click(object sender, RoutedEventArgs e)
        {
            if (!proyecto.TipoProyecto.CodTipo.Equals(2))
            {
                RegistrarReinversion registrarReinversion = new RegistrarReinversion(CodProyecto);
                registrarReinversion.ShowDialog();

                if (registrarReinversion.IsActive == false && registrarReinversion.Reload)
                {
                    RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                    mainWindow.ReloadUserControls(CodProyecto);
                }
            }
            else
            {
                MessageBox.Show("Este Tipo de Análisis es Ambiental, si desea realizar un Análisis Completo actualice el Tipo de Análisis del Proyecto", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditarReinversion_Click(object sender, RoutedEventArgs e)
        {
            if(ReinversionSelected != null)
            {
                RegistrarReinversion registrarReinversion = new RegistrarReinversion(CodProyecto, ReinversionSelected.CodRequerimientoReinversion);
                registrarReinversion.ShowDialog();

                if (registrarReinversion.IsActive == false && registrarReinversion.Reload)
                {
                    RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                    mainWindow.ReloadUserControls(CodProyecto);
                }
            }
        }

        private void btnEliminarReinversion_Click(object sender, RoutedEventArgs e)
        {
            if(ReinversionSelected != null)
            {
                if (MessageBox.Show("Esta seguro que desea eliminar esta reinversión?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (reinversionData.EliminarReinversion(ReinversionSelected.CodRequerimientoReinversion))
                    {
                        RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                        mainWindow.ReloadUserControls(CodProyecto);
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error al eliminar la reinversión del proyecto," +
                            "verifique que la inversión no esté vinculada a alguna otra reinversión",
                            "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion
    }
}
