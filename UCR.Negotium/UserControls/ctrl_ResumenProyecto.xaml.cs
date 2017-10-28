using System.ComponentModel;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_Resumen.xaml
    /// </summary>
    public partial class ctrl_ResumenProyecto : UserControl, INotifyPropertyChanged
    {
        private int codProyecto;
        private Proyecto proyecto;

        private ProyectoData proyectoData;
        private OrganizacionProponenteData orgProponenteData;

        public ctrl_ResumenProyecto()
        {
            InitializeComponent();
            DataContext = this;

            proyecto = new Proyecto();
            proyectoData = new ProyectoData();
            orgProponenteData = new OrganizacionProponenteData();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region Properties
        public int CodProyecto
        {
            get { return codProyecto; }
            set { codProyecto = value; Reload(); }
        }

        public Proyecto ProyectoSelected
        {
            get { return proyecto; }
            set { proyecto = value; }
        }
        #endregion

        #region InternalMethods
        private void Reload()
        {
            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);
            ProyectoSelected.OrganizacionProponente = orgProponenteData.GetOrganizacionProponente(CodProyecto);
            PropertyChanged(this, new PropertyChangedEventArgs("ProyectoSelected"));
        }
        #endregion
    }
}
