using System;
using System.Collections.Generic;
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
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for CapitalTrabajo.xaml
    /// </summary>
    public partial class CapitalTrabajo : UserControl
    {
        private Proyecto proyectoSelected;

        //public static readonly DependencyProperty ProyectoProperty = DependencyProperty.Register(
        //    "ProyectoSelected", typeof(Proyecto), typeof(InformacionGeneral), new PropertyMetadata(null));

        public CapitalTrabajo()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            //SetBinding(ProyectoProperty,
            //        new Binding { Path = new PropertyPath("_proyectoSelected"), Mode = BindingMode.TwoWay });
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
    }
}
