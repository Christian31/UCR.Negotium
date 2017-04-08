using UCR.Negotium.Dialogs;
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
    /// Interaction logic for Inversiones.xaml
    /// </summary>
    public partial class Inversiones : UserControl, INotifyPropertyChanged
    {
        private RequerimientoInversion inversionSelected;
        private List<RequerimientoInversion> inversiones;
        private Proyecto proyectoSelected;

        public static readonly DependencyProperty ProyectoProperty = DependencyProperty.Register(
            "ProyectoSelected", typeof(Proyecto), typeof(Inversiones), new PropertyMetadata(null));

        public Inversiones()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            SetBinding(ProyectoProperty,
                    new Binding { Path = new PropertyPath("proyectoSelected"), Mode = BindingMode.TwoWay });

            proyectoSelected = new Proyecto();
            inversionSelected = new RequerimientoInversion();
            inversiones = new List<RequerimientoInversion>();
            InversionSelected = ProyectoSelected.RequerimientosInversion.FirstOrDefault();
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

        public RequerimientoInversion InversionSelected
        {
            get
            {
                return inversionSelected;
            }
            set
            {
                inversionSelected = value;
            }
        }

        public List<RequerimientoInversion> InversionesList
        {
            get
            {
                return inversiones;
            }
            set
            {
                inversiones = value;
                PropertyChanged(this, new PropertyChangedEventArgs("InversionesList"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void btnCrearInversion_Click(object sender, RoutedEventArgs e)
        {
            RegistrarInversionDialog registrarInversion = new RegistrarInversionDialog(26);
            registrarInversion.Show();
        }

        private void btnEditarInversion_Click(object sender, RoutedEventArgs e)
        {
            RegistrarInversionDialog registrarInversion = new RegistrarInversionDialog(26, 4);
            registrarInversion.Show();
        }

        private void btnEliminarInversion_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
