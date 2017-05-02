using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Utils;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for Depreciaciones.xaml
    /// </summary>
    public partial class Depreciaciones : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyectoSelected;
        private int codProyecto;
        private DataView depreciaciones;

        private ProyectoData proyectoData;
        private RequerimientoInversionData requerimientoInversionData;
        private RequerimientoReinversionData requerimientoReinversionData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public Depreciaciones()
        {
            InitializeComponent();
            DataContext = this;

            proyectoSelected = new Proyecto();
            depreciaciones = new DataView();

            proyectoData = new ProyectoData();
            requerimientoInversionData = new RequerimientoInversionData();
            requerimientoReinversionData = new RequerimientoReinversionData();
        }

        public void Reload()
        {
            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);
            ProyectoSelected.RequerimientosInversion = requerimientoInversionData.GetRequerimientosInversion(CodProyecto);
            ProyectoSelected.RequerimientosReinversion = requerimientoReinversionData.GetRequerimientosReinversion(CodProyecto);
            DTDepreciaciones = DatatableBuilder.GenerarDTDepreciaciones(ProyectoSelected).AsDataView();
        }

        public DataView DTDepreciaciones
        {
            get
            {
                return depreciaciones;
            }
            set
            {
                depreciaciones = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DTDepreciaciones"));
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

        private void datagridDepreciaciones_Loaded(object sender, RoutedEventArgs e)
        {
            if (datagridDepreciaciones.Columns.Count > 0)
            {
                datagridDepreciaciones.Columns[0].Width = 130;
            }
        }
    }
}
