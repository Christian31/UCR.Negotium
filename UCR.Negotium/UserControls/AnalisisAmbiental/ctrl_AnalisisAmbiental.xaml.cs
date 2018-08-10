using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ctrl_AnalisisAmbiental.xaml
    /// </summary>
    public partial class ctrl_AnalisisAmbiental : UserControl, INotifyPropertyChanged
    {
        private int codProyecto;
        private List<FactorAmbiental> factorAmbientalList;
        private FactorAmbiental factorAmbientalSelected;

        private FactorAmbientalData factorAmbientalData;

        public ctrl_AnalisisAmbiental()
        {
            InitializeComponent();
            DataContext = this;

            factorAmbientalList = new List<FactorAmbiental>();
            factorAmbientalSelected = new FactorAmbiental();
            factorAmbientalData = new FactorAmbientalData();

            Reload();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public List<FactorAmbiental> FactorAmbientalList
        {
            get
            {
                return factorAmbientalList;
            }
            set
            {
                factorAmbientalList = value;
                FactorAmbientalSelected = FactorAmbientalList.FirstOrDefault();
                PropertyChanged(this, new PropertyChangedEventArgs("FactorAmbientalList"));
            }
        }

        public FactorAmbiental FactorAmbientalSelected
        {
            get
            {
                return factorAmbientalSelected;
            }
            set
            {
                factorAmbientalSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FactorAmbientalSelected"));
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

        private void Reload()
        {
            FactorAmbientalList = factorAmbientalData.GetFactores(CodProyecto);
        }

        private void btnCrearFactorAmbiental_Click(object sender, RoutedEventArgs e)
        {
            RegistrarFactorAmbiental registrarFactor = new RegistrarFactorAmbiental(CodProyecto);
            registrarFactor.ShowDialog();

            if (!registrarFactor.IsActive && registrarFactor.Reload)
            {
                Reload();
            }
        }

        private void btnEditarFactorAmbiental_Click(object sender, RoutedEventArgs e)
        {
            if (FactorAmbientalSelected != null)
            {
                RegistrarFactorAmbiental registrarFactor = new RegistrarFactorAmbiental(CodProyecto, FactorAmbientalSelected.CodFactorAmbiental);
                registrarFactor.ShowDialog();

                if (!registrarFactor.IsActive && registrarFactor.Reload)
                {
                    Reload();
                }
            }
        }

        private void btnEliminarFactorAmbiental_Click(object sender, RoutedEventArgs e)
        {
            if (FactorAmbientalSelected != null)
            {
                if (CustomMessageBox.Show(Constantes.ELIMINARFACTORAMBIENTALMSG))
                {
                    if (factorAmbientalData.EliminarFactorAmbiental(FactorAmbientalSelected.CodFactorAmbiental))
                    {
                        Reload();
                    }
                    else
                    {
                        MessageBox.Show(Constantes.ELIMINARFACTORAMBIENTALERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
