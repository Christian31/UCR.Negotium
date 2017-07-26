using UCR.Negotium.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.Domain;
using UCR.Negotium.DataAccess;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_Inversiones.xaml
    /// </summary>
    public partial class ctrl_Inversiones : UserControl, INotifyPropertyChanged
    {
        private Inversion inversionSelected;
        private List<Inversion> inversiones;
        private Proyecto proyectoSelected;
        private int codProyecto;

        private InversionData inversionData;

        public ctrl_Inversiones()
        {
            InitializeComponent();
            DataContext = this;

            proyectoSelected = new Proyecto();
            inversionSelected = new Inversion();
            inversiones = new List<Inversion>();
            inversionData = new InversionData();
        }

        public void Reload()
        {
            InversionesList = inversionData.GetRequerimientosInversion(CodProyecto);
            PropertyChanged(this, new PropertyChangedEventArgs("InversionesTotal"));
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
            get
            {
                return inversiones;
            }
            set
            {
                inversiones = value;
                InversionSelected = InversionesList.FirstOrDefault();
                PropertyChanged(this, new PropertyChangedEventArgs("InversionesList"));
            }
        }

        public string InversionesTotal
        {
            get
            {
                double valor = 0;
                InversionesList.ForEach(reqInver => valor += reqInver.Subtotal);
                return "₡ " + valor.ToString("#,##0.##");
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
            RegistrarInversion registrarInversion = new RegistrarInversion(CodProyecto);
            registrarInversion.ShowDialog();

            if (registrarInversion.IsActive == false && registrarInversion.Reload)
            {
                RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                mainWindow.ReloadUserControls(CodProyecto);
            }
        }

        private void btnEditarInversion_Click(object sender, RoutedEventArgs e)
        {
            if (InversionSelected != null)
            {
                RegistrarInversion registrarInversion = new RegistrarInversion(CodProyecto, InversionSelected.CodRequerimientoInversion);
                registrarInversion.ShowDialog();

                if (registrarInversion.IsActive == false && registrarInversion.Reload)
                {
                    RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                    mainWindow.ReloadUserControls(CodProyecto);
                }
            }
        }

        private void btnEliminarInversion_Click(object sender, RoutedEventArgs e)
        {
            if (InversionSelected != null)
            {
                if (MessageBox.Show("Esta seguro que desea eliminar esta inversión?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (inversionData.EliminarRequerimientoInversion(InversionSelected.CodRequerimientoInversion))
                    {
                        RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                        mainWindow.ReloadUserControls(CodProyecto);
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
