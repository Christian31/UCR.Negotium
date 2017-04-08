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
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for Proponente.xaml
    /// </summary>
    public partial class Proponente : UserControl
    {
        private UCR.Negotium.Domain.Proponente proponenteSelected;
        private List<TipoOrganizacion> tipoOrganizaciones;

        public static readonly DependencyProperty ProponenteProperty = DependencyProperty.Register(
            "ProponenteSelected", typeof(UCR.Negotium.Domain.Proponente), typeof(Proponente), new PropertyMetadata(null));

        public Proponente()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
            SetBinding(ProponenteProperty,
                    new Binding { Path = new PropertyPath("proponenteSelected"), Mode = BindingMode.TwoWay });

            TipoOrganizacionData tipoOrganizacionData = new TipoOrganizacionData();
            tipoOrganizaciones = new List<TipoOrganizacion>();
            tipoOrganizaciones = tipoOrganizacionData.GetTiposDeOrganizacionAux();
        }

        public List<TipoOrganizacion> TipoOrganizaciones
        {
            get
            {
                return tipoOrganizaciones;
            }
            set
            {
                tipoOrganizaciones = value;
            }
        }

        public UCR.Negotium.Domain.Proponente ProponenteSelected
        {
            get
            {
                return proponenteSelected;
            }
            set
            {
                proponenteSelected = value;
            }
        }

        private void btnGuardarProponente(object sender, RoutedEventArgs e)
        {

        }

        private void cbMasculino_Checked(object sender, RoutedEventArgs e)
        {
            cbFemenino.IsChecked = false;
        }

        private void cbMasculino_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbMasculino.IsChecked.Value.Equals(false))
            {
                cbFemenino.IsChecked = true;
            }
        }

        private void cbFemenino_Checked(object sender, RoutedEventArgs e)
        {
            cbMasculino.IsChecked = false;
        }

        private void cbEsRepresentanteIndividual_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked.Value.Equals(true))
            {
                cbTipoOrganizaciones.SelectedItem = TipoOrganizaciones.LastOrDefault();
                cbTipoOrganizaciones.IsEnabled = false;
            }
            else
            {
                cbTipoOrganizaciones.SelectedItem = TipoOrganizaciones.FirstOrDefault();
                cbTipoOrganizaciones.IsEnabled = true;
            }
        }

        private void cbTipoOrganizaciones_Loaded(object sender, RoutedEventArgs e)
        {
            TipoOrganizacion tipoSelected = (TipoOrganizacion)cbTipoOrganizaciones.SelectedItem;
            if (tipoSelected == null || tipoSelected.CodTipo.Equals(0))
            {
                tipoSelected = TipoOrganizaciones.FirstOrDefault();
                cbTipoOrganizaciones.SelectedItem = tipoSelected;
            }
        }
    }
}
