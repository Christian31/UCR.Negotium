using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_Depreciaciones.xaml
    /// </summary>
    public partial class ctrl_Depreciaciones : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyectoSelected;
        private int codProyecto;
        private DataView depreciaciones;
        private DataView totalDepreciaciones;

        private ProyectoData proyectoData;
        private InversionData requerimientoInversionData;
        private ReinversionData requerimientoReinversionData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_Depreciaciones()
        {
            InitializeComponent();
            DataContext = this;

            proyectoSelected = new Proyecto();
            depreciaciones = new DataView();
            totalDepreciaciones = new DataView();

            proyectoData = new ProyectoData();
            requerimientoInversionData = new InversionData();
            requerimientoReinversionData = new ReinversionData();

            Reload();
        }

        public void Reload()
        {
            DTDepreciaciones = new DataView();
            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);
            ProyectoSelected.RequerimientosInversion = requerimientoInversionData.GetRequerimientosInversion(CodProyecto);
            ProyectoSelected.RequerimientosReinversion = requerimientoReinversionData.GetRequerimientosReinversion(CodProyecto);

            if (!ProyectoSelected.RequerimientosInversion.Where(inv => inv.Depreciable).Count().Equals(0) ||
                !ProyectoSelected.RequerimientosReinversion.Where(reinv => reinv.Depreciable).Count().Equals(0))
            {
                DTDepreciaciones = DatatableBuilder.GenerarDTDepreciaciones(ProyectoSelected).AsDataView();
                DTTotalesDepreciaciones = DatatableBuilder.GenerarDTTotalesDepreciaciones(ProyectoSelected).AsDataView();
            }
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

        public DataView DTTotalesDepreciaciones
        {
            get
            {
                return totalDepreciaciones;
            }
            set
            {
                totalDepreciaciones = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DTTotalesDepreciaciones"));
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
