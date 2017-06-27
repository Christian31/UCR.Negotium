using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess.AnalisisAmbiental;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for AnalisisAmbiental.xaml
    /// </summary>
    public partial class AnalisisAmbiental : UserControl, INotifyPropertyChanged
    {
        private int codProyecto;
        private List<FactorAmbiental> factorAmbientalList;
        private FactorAmbiental factorAmbientalSelected;

        private FactorAmbientalData analisisAmbientalData;

        public AnalisisAmbiental()
        {
            InitializeComponent();
            DataContext = this;

            factorAmbientalList = new List<FactorAmbiental>();
            factorAmbientalSelected = new FactorAmbiental();
            analisisAmbientalData = new FactorAmbientalData();

            //
            //factorAmbientalList.Add(new FactorAmbiental() { CodCondicionAfectada = CondicionAfectada.Biotopos, NombreFactor = "Ejemplo", CodElementoAmbiental = 1, CodClasificacion = Clasificacion.Moderado});
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
            //FactorAmbientalList = new List<FactorAmbiental>();
        }

        private void btnCrearFactorAmbiental_Click(object sender, RoutedEventArgs e)
        {
            RegistrarFactorAmbiental registrarFactor = new RegistrarFactorAmbiental(CodProyecto);
            registrarFactor.ShowDialog();

            if (registrarFactor.IsActive == false && registrarFactor.Reload)
            {
                RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                mainWindow.ReloadUserControls(CodProyecto);
            }
        }

        private void btnEditarFactorAmbiental_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminarFactorAmbiental_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
