using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_CapitalTrabajo.xaml
    /// </summary>
    public partial class ctrl_CapitalTrabajo : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyectoSelected;
        private int codProyecto;
        private DataView capitalTrabajo;
        private double recuperacionCT;
        private string signoMoneda;

        private ProyectoData proyectoData;
        private CostoData costoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_CapitalTrabajo()
        {
            InitializeComponent();
            DataContext = this;

            proyectoData = new ProyectoData();
            costoData = new CostoData();

            proyectoSelected = new Proyecto();
            capitalTrabajo = new DataView();
        }

        #region Methods
        public void Reload()
        {
            DTCapitalTrabajo = new DataView();
            recuperacionCT = 0;
            signoMoneda = MonedaActual.GetSignoMoneda(CodProyecto);

            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);
            ProyectoSelected.Costos = costoData.GetCostos(CodProyecto);

            if (!ProyectoSelected.Costos.Count.Equals(0))
            {
                DatatableBuilder.GenerarCapitalTrabajo(ProyectoSelected, out capitalTrabajo, out recuperacionCT);
            }

            PropertyChanged(this, new PropertyChangedEventArgs("RecuperacionCT"));
            PropertyChanged(this, new PropertyChangedEventArgs("DTCapitalTrabajo"));
        }
        #endregion

        #region Properties
        public string RecuperacionCT
        {
            get
            {
                return signoMoneda +" "+ recuperacionCT.ToString("#,##0.##");
            }
            set
            {
                recuperacionCT = Convert.ToDouble(value);
            }
        }

        public DataView DTCapitalTrabajo
        {
            get
            {
                return capitalTrabajo;
            }
            set
            {
                capitalTrabajo = value;
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

        public Proyecto ProyectoSelected
        {
            get
            {
                return proyectoSelected;
            }
            set
            {
                proyectoSelected = value;
            }
        }
        #endregion

        #region Events
        private void datagridCapitalTrabajo_Loaded(object sender, RoutedEventArgs e)
        {
            if (datagridCapitalTrabajo.Columns.Count >0)
            {
                datagridCapitalTrabajo.Columns[0].Width = 130;
            }
        }
        #endregion
    }
}
