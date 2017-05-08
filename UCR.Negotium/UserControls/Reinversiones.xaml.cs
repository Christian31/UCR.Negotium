using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
using UCR.Negotium.Utils;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for Reinversiones.xaml
    /// </summary>
    public partial class Reinversiones : UserControl, INotifyPropertyChanged
    {
        private RequerimientoReinversion reinversionSelected;
        private List<RequerimientoReinversion> reinversiones;
        private int codProyecto;
        private Proyecto proyecto;
        private DataView totalesReinversiones;

        private RequerimientoReinversionData reinversionData;
        private ProyectoData proyectoData;

        public Reinversiones()
        {
            InitializeComponent();
            DataContext = this;

            reinversionData = new RequerimientoReinversionData();
            proyectoData = new ProyectoData();

            proyecto = new Proyecto();
            reinversionSelected = new RequerimientoReinversion();
            reinversiones = new List<RequerimientoReinversion>();
            totalesReinversiones = new DataView();
        }

        public void Reload()
        {
            DTTotalesReinversiones = new DataView();
            ReinversionesList = reinversionData.GetRequerimientosReinversion(CodProyecto);
            proyecto = proyectoData.GetProyecto(CodProyecto);
            proyecto.RequerimientosReinversion = ReinversionesList;

            if (!proyecto.RequerimientosReinversion.Count.Equals(0))
            {
                DTTotalesReinversiones = DatatableBuilder.GenerarDTTotalesReinversiones(proyecto).AsDataView();
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

        public RequerimientoReinversion ReinversionSelected
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

        public List<RequerimientoReinversion> ReinversionesList
        {
            get
            {
                return reinversiones;
            }
            set
            {
                reinversiones = value;
                ReinversionSelected = ReinversionesList.FirstOrDefault();
                PropertyChanged(this, new PropertyChangedEventArgs("ReinversionesList"));
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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void btnAgregarReinversion_Click(object sender, RoutedEventArgs e)
        {
            RegistrarReinversion registrarReinversion = new RegistrarReinversion(CodProyecto);
            registrarReinversion.ShowDialog();

            if (registrarReinversion.IsActive == false && registrarReinversion.Reload)
            {
                RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                mainWindow.ReloadUserControls(CodProyecto);
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
                    if (reinversionData.EliminarRequerimientoReinversion(ReinversionSelected.CodRequerimientoReinversion))
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
    }
}
