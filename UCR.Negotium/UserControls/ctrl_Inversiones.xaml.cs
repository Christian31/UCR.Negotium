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
                inv.CostoUnitarioFormat = SignoMoneda +" "+ inv.CostoUnitario.ToString("#,##0.##");
                inv.SubtotalFormat = SignoMoneda +" "+ inv.Subtotal.ToString("#,##0.##");
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
                return signoMoneda +" "+ valor.ToString("#,##0.##");
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
                MessageBox.Show("Este Tipo de Análisis es Ambiental, si desea realizar un Análisis Financiero o Social actualice el Tipo de Análisis del Proyecto", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (CustomMessageBox.Show("Esta seguro que desea eliminar esta inversión?"))
                {
                    if (inversionData.EliminarInversion(InversionSelected.CodInversion))
                    {
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.Inversiones);
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error al eliminar la inversión del proyecto," +
                            "verifique que la inversión no esté vinculada a alguna reinversión",
                            "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }           
        }
        #endregion
    }
}
