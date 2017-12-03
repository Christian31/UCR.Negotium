using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using UCR.Negotium.Domain.Enums;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_ProgresoProyecto.xaml
    /// </summary>
    public partial class ctrl_ProgresoProyecto : UserControl, INotifyPropertyChanged
    {
        private Dictionary<ProgresoStep, bool> statusSteps;

        public ctrl_ProgresoProyecto()
        {
            InitializeComponent();
            statusSteps = new Dictionary<ProgresoStep, bool>(12)
            {
                { ProgresoStep.InformacionGeneral, false },
                { ProgresoStep.Proponente, false },
                { ProgresoStep.Caracterizacion, false },
                { ProgresoStep.Inversiones, false },
                { ProgresoStep.Reinversiones, false },
                { ProgresoStep.ProyeccionVentas, false },
                { ProgresoStep.Costos, false },
                { ProgresoStep.CapitalTrabajo, false },
                { ProgresoStep.Depreciaciones, false },
                { ProgresoStep.Financiamiento, false },
                { ProgresoStep.FlujoCaja, false },
                { ProgresoStep.AnalisisAmbiental, false },
            };

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region InternalMethods
        public void Reload(Dictionary<ProgresoStep, bool> statusSteps)
        {
            StatusSteps = statusSteps;
        }
        #endregion

        #region Properties
        public Dictionary<ProgresoStep, bool> StatusSteps
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
        #endregion
    }
}
