using UCR.Negotium.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.Domain;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Extensions;
using UCR.Negotium.Domain.Enums;
using UCR.Negotium.Domain.Extensions;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_Inversiones.xaml
    /// </summary>
    public partial class ctrl_Inversiones : UserControl, INotifyPropertyChanged
    {
        private Inversion inversionSelected;
        private List<Inversion> inversiones;
        private ProyectoLite proyecto;
        private int codProyecto;
        private string signoMoneda;

        private InversionData inversionData;
        private ProyectoData proyectoData;

        public ctrl_Inversiones()
        {
            InitializeComponent();
            DataContext = this;

            proyecto = new ProyectoLite();
            inversionSelected = new Inversion();
            inversiones = new List<Inversion>();
            inversionData = new InversionData();
            proyectoData = new ProyectoData();
        }

        #region InternalMethods
        private void Reload()
        {
            SignoMoneda = LocalContext.GetSignoMoneda(CodProyecto);
            InversionesList = inversionData.GetInversiones(CodProyecto);

            InversionesList.All(inv => {
                inv.CostoUnitarioFormat = inv.CostoUnitario.FormatoMoneda(SignoMoneda);
                inv.SubtotalFormat = inv.Subtotal.FormatoMoneda(SignoMoneda);
                return true;
            });

            dgtxcCostoUnidad.Header = string.Format("Costo Unitario ({0})", SignoMoneda);
            dgtxcSubtotal.Header = string.Format("Subtotal ({0})", SignoMoneda);

            InversionSelected = InversionesList.FirstOrDefault();
            PropertyChanged(this, new PropertyChangedEventArgs("InversionesList"));
            PropertyChanged(this, new PropertyChangedEventArgs("InversionesTotal"));

            proyecto = proyectoData.GetProyectoLite(CodProyecto);
        }
        #endregion

        #region Properties
        public string SignoMoneda
        {
            get
            {
                return signoMoneda;
            }
            set
            {
                signoMoneda = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SignoMoneda"));
            }
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

        public Inversion InversionSelected
        {
            get
            {
                return inversionSelected;
            }
            set
            {
                inversionSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("InversionSelected"));
            }
        }

        public List<Inversion> InversionesList
        {
            get { return inversiones; }
            set { inversiones = value; }
        }

        public string InversionesTotal
        {
            get
            {
                double valor = 0;
                InversionesList.ForEach(reqInver => valor += reqInver.Subtotal);
                return valor.FormatoMoneda(SignoMoneda);
            }
            set
            {
                InversionesTotal = value;
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void btnCrearInversion_Click(object sender, RoutedEventArgs e)
        {
            if (!proyecto.CodTipoProyecto.Equals(2))
            {
                RegistrarInversion registrarInversion = new RegistrarInversion(proyecto);
                registrarInversion.ShowDialog();

                if (registrarInversion.IsActive == false && registrarInversion.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.Inversiones);
                }
            }
            else
            {
                MessageBox.Show(Constantes.ACTUALIZARPROYECTORESTRTIPOAMBIENTAL, Constantes.ACTUALIZARPROYECTOTLT,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditarInversion_Click(object sender, RoutedEventArgs e)
        {
            if (InversionSelected != null)
            {
                RegistrarInversion registrarInversion = new RegistrarInversion(proyecto, InversionSelected.CodInversion);
                registrarInversion.ShowDialog();

                if (registrarInversion.IsActive == false && registrarInversion.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.Inversiones);
                }
            }
        }

        private void btnEliminarInversion_Click(object sender, RoutedEventArgs e)
        {
            if (InversionSelected != null)
            {
                if (CustomMessageBox.Show(Constantes.ELIMINARINVERSIONMSG))
                {
                    if (inversionData.EliminarInversion(InversionSelected.CodInversion))
                    {
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.Inversiones);
                    }
                    else
                    {
                        MessageBox.Show(Constantes.ELIMINARINVERSIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }           
        }
        #endregion
    }
}
