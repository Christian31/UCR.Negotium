using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ProgresoProyectoUserControl.xaml
    /// </summary>
    public partial class ProgresoProyecto : UserControl, INotifyPropertyChanged
    {
        private List<bool> statusSteps;

        public ProgresoProyecto()
        {
            InitializeComponent();
            statusSteps = new List<bool>(11);

            DataContext = this;
        }

        public void Reload(List<bool> statusSteps)
        {
            StatusSteps = statusSteps;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public List<bool> StatusSteps
        {
            get
            {
                return statusSteps;
            }
            set
            {
                statusSteps = value;
                PropertyChanged(this, new PropertyChangedEventArgs("StatusSteps"));
            }
        }
    }
}
