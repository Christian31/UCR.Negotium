using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Enums;
using UCR.Negotium.Domain.Extensions;
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
        private ProyectoLite proyecto;
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

            proyecto = new ProyectoLite();
            reinversionSelected = new Reinversion();
            reinversiones = new List<Reinversion>();
            totalesReinversiones = new DataView();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region InternalMethods
        private void Reload()
        {
            DTTotalesReinversiones = new DataView();

            SignoMoneda = LocalContext.GetSignoMoneda(CodProyecto);
            ReinversionesList = reinversionData.GetReinversiones(CodProyecto);

            ReinversionesList.All(reinv =>
            {
                reinv.CostoUnitarioFormat = reinv.CostoUnitario.FormatoMoneda(SignoMoneda);
                reinv.SubtotalFormat = reinv.Subtotal.FormatoMoneda(SignoMoneda);
                return true;
            });

            dgtxcCostoUnidad.Header = string.Format("Costo Unitario ({0})", SignoMoneda);
            dgtxcSubtotal.Header = string.Format("Subtotal ({0})", SignoMoneda);

            ReinversionSelected = ReinversionesList.FirstOrDefault();
            PropertyChanged(this, new PropertyChangedEventArgs("ReinversionesList"));

            proyecto = proyectoData.GetProyectoLite(CodProyecto);

            if (!ReinversionesList.Count.Equals(0))
            {
                DTTotalesReinversiones = DatatableBuilder.GenerarReinversionesTotales(proyecto, ReinversionesList).AsDataView();
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
            if (!proyecto.CodTipoProyecto.Equals(2))
            {
                RegistrarReinversion registrarReinversion = new RegistrarReinversion(proyecto);
                registrarReinversion.ShowDialog();

                if (registrarReinversion.IsActive == false && registrarReinversion.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.Reinversiones);
                }
            }
            else
            {
                MessageBox.Show(Constantes.ACTUALIZARPROYECTORESTRTIPOAMBIENTAL, Constantes.ACTUALIZARPROYECTOTLT,
                     MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditarReinversion_Click(object sender, RoutedEventArgs e)
        {
            if(ReinversionSelected != null)
            {
                RegistrarReinversion registrarReinversion = new RegistrarReinversion(proyecto, ReinversionSelected.CodReinversion);
                registrarReinversion.ShowDialog();

                if (registrarReinversion.IsActive == false && registrarReinversion.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.Reinversiones);
                }
            }
        }

        private void btnEliminarReinversion_Click(object sender, RoutedEventArgs e)
        {
            if(ReinversionSelected != null)
            {
                if (CustomMessageBox.Show(Constantes.ELIMINARREINVERSIONMSG))
                {
                    if (reinversionData.EliminarReinversion(ReinversionSelected.CodReinversion))
                    {
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.Reinversiones);
                    }
                    else
                    {
                        MessageBox.Show(Constantes.ELIMINARREINVERSIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion
    }
}
