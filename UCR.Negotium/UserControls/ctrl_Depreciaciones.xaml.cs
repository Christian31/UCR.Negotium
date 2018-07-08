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

        #region InternalMethods
        private void Reload()
        {
            DTDepreciaciones = new DataView();
            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);
            ProyectoSelected.Inversiones = requerimientoInversionData.GetInversiones(CodProyecto);
            ProyectoSelected.Reinversiones = requerimientoReinversionData.GetReinversiones(CodProyecto);

            if (!ProyectoSelected.Inversiones.Where(inv => inv.Depreciable).Count().Equals(0) ||
                !ProyectoSelected.Reinversiones.Where(reinv => reinv.Depreciable).Count().Equals(0))
            {
                DTDepreciaciones = DatatableBuilder.GenerarDepreciaciones(ProyectoSelected).AsDataView();
                DTTotalesDepreciaciones = DatatableBuilder.GenerarDepreciacionesTotales(ProyectoSelected).AsDataView();
            }
        }
        #endregion

        #region Properties
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
        #endregion

        #region Events
        private void datagridDepreciaciones_Loaded(object sender, RoutedEventArgs e)
        {
            if (datagridDepreciaciones.Columns.Count > 0)
            {
                datagridDepreciaciones.Columns[0].Width = 130;
            }
        }
        #endregion

    }
}
