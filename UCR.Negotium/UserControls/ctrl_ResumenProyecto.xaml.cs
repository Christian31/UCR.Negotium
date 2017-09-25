using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void Reload()
        {
            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);
            ProyectoSelected.OrganizacionProponente = orgProponenteData.GetOrganizacionProponente(CodProyecto);
            PropertyChanged(this, new PropertyChangedEventArgs("ProyectoSelected"));
        }
    }
}
