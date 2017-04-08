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
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for Reinversiones.xaml
    /// </summary>
    public partial class Reinversiones : UserControl, INotifyPropertyChanged
    {
        private RequerimientoReinversion reinversionSelected;
        private List<RequerimientoReinversion> reinversiones;
        private Proyecto proyectoSelected;

        public static readonly DependencyProperty ProyectoProperty = DependencyProperty.Register(
            "ProyectoSelected", typeof(Proyecto), typeof(Reinversiones), new PropertyMetadata(null));

        public Reinversiones()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            SetBinding(ProyectoProperty,
                    new Binding { Path = new PropertyPath("proyectoSelected"), Mode = BindingMode.TwoWay });

            reinversionSelected = new RequerimientoReinversion();
            reinversiones = new List<RequerimientoReinversion>();
            ReinversionSelected = reinversiones.FirstOrDefault();
            proyectoSelected = new Proyecto();
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

        public RequerimientoReinversion ReinversionSelected
        {
            get
            {
                return reinversionSelected;
            }
            set
            {
                reinversionSelected = value;
            }
        }

        public List<RequerimientoReinversion> ReinversionesList
        {
            get
            {
                return reinversiones;
            }
            set
            {
                reinversiones = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ReinversionesList"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void menuItemEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemImprimir_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
