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
        private Dictionary<Modulo, bool> statusSteps;

        public ctrl_ProgresoProyecto()
        {
            InitializeComponent();
            statusSteps = new Dictionary<Modulo, bool>(12)
            {
                { Modulo.InformacionGeneral, false },
                { Modulo.Proponente, false },
                { Modulo.Caracterizacion, false },
                { Modulo.Inversiones, false },
                { Modulo.Reinversiones, false },
                { Modulo.ProyeccionVentas, false },
                { Modulo.Costos, false },
                { Modulo.CapitalTrabajo, false },
                { Modulo.Depreciaciones, false },
                { Modulo.Financiamiento, false },
                { Modulo.FlujoCaja, false },
                { Modulo.AnalisisAmbiental, false },
            };

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region InternalMethods
        public void Reload(Dictionary<Modulo, bool> statusSteps)
        {
            StatusSteps = statusSteps;
        }
        #endregion

        #region Properties
        public Dictionary<Modulo, bool> StatusSteps
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
