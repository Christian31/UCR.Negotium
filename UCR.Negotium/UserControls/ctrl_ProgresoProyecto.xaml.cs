﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_ProgresoProyecto.xaml
    /// </summary>
    public partial class ctrl_ProgresoProyecto : UserControl, INotifyPropertyChanged
    {
        private List<bool> statusSteps;

        public ctrl_ProgresoProyecto()
        {
            InitializeComponent();
            statusSteps = new List<bool>(11);

            DataContext = this;
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region InternalMethods
        public void Reload(List<bool> statusSteps)
        {
            StatusSteps = statusSteps;
        }
        #endregion

        #region Properties
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
        #endregion
    }
}
