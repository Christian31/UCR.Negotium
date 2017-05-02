using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for Proponente.xaml
    /// </summary>
    public partial class Proponente : UserControl, INotifyPropertyChanged
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private Domain.Proponente proponenteSelected;
        private List<TipoOrganizacion> tipoOrganizaciones;
        private int codProyecto { get; set; }

        private ProponenteData proponenteData;
        private TipoOrganizacionData tipoOrganizacionData;
        private OrganizacionProponenteData organizacionProponenteData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public Proponente()
        {
            InitializeComponent();
            DataContext = this;

            proponenteData = new ProponenteData();
            proponenteSelected = new Domain.Proponente();

            tipoOrganizacionData = new TipoOrganizacionData();
            organizacionProponenteData = new OrganizacionProponenteData();
            tipoOrganizaciones = new List<TipoOrganizacion>();

            tipoOrganizaciones = tipoOrganizacionData.GetTiposDeOrganizacionAux();
        }

        public void Reload()
        {
            ProponenteSelected = proponenteData.GetProponente(CodProyecto);
        }

        #region Properties
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

        public Domain.Proponente ProponenteSelected
        {
            get
            {
                return proponenteSelected;
            }
            set
            {
                proponenteSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ProponenteSelected"));
            }
        }
        #endregion

        #region Events
        private void btnGuardarProponente(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (ProponenteSelected.IdProponente.Equals(0))
                {
                    int codOrganizacion = organizacionProponenteData.InsertarOrganizacionProponente(ProponenteSelected.Organizacion);
                    if (!codOrganizacion.Equals(-1))
                    {
                        ProponenteSelected.Organizacion.CodOrganizacion = codOrganizacion;
                        int codProponente = proponenteData.InsertarProponente(ProponenteSelected, CodProyecto);
                        if (!codProponente.Equals(-1))
                        {
                            //success
                            ProponenteSelected.IdProponente = codProponente;
                            RegistrarProyectoWindow mainWindow = (RegistrarProyectoWindow)Application.Current.Windows[0];
                            mainWindow.ReloadUserControls(CodProyecto);

                            MessageBox.Show("El proponente del proyecto se ha insertado correctamente", "Proponente Insertado", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            //error
                            MessageBox.Show("Ha ocurrido un error al insertar el proponente del proyecto, verifique que los datos ingresados sean correctos", "Proponente Insertado", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar el proponente del proyecto, verifique que los datos ingresados sean correctos", "Proponente Insertado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (proponenteData.ActualizarProponente(ProponenteSelected) &&
                        organizacionProponenteData.ActualizarOrganizacionProponente(ProponenteSelected.Organizacion))
                    {
                        //success
                        MessageBox.Show("El proponente del proyecto se ha actualizado correctamente", "Proponente Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar el proponente del proyecto, verifique que los datos ingresados sean correctos", "Proponente Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
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

        private void cbTipoOrganizaciones_Loaded(object sender, RoutedEventArgs e)
        {
            TipoOrganizacion tipoSelected = (TipoOrganizacion)cbTipoOrganizaciones.SelectedItem;
            if (tipoSelected == null || tipoSelected.CodTipo.Equals(0))
            {
                tipoSelected = TipoOrganizaciones.FirstOrDefault();
                cbTipoOrganizaciones.SelectedItem = tipoSelected;
            }
        }
        #endregion

        #region InternalMethods
        public bool ValidateRequiredFields()
        {
            bool validationResult = false;

            return validationResult;
        }

        #endregion

        private void cbSoyRepresentanteIndividual_Checked(object sender, RoutedEventArgs e)
        {
            cbTipoOrganizaciones.SelectedItem = TipoOrganizaciones.LastOrDefault();
            cbTipoOrganizaciones.IsEnabled = false;
            cbNoSoyRepresentanteIndividual.IsChecked = false;
        }

        private void cbSoyRepresentanteIndividual_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbSoyRepresentanteIndividual.IsChecked.Value.Equals(false))
            {
                cbNoSoyRepresentanteIndividual.IsChecked = true;
            }
        }

        private void cbNoSoyRepresentanteIndividual_Checked(object sender, RoutedEventArgs e)
        {
            cbTipoOrganizaciones.SelectedItem = TipoOrganizaciones.FirstOrDefault();
            cbTipoOrganizaciones.IsEnabled = true;
            cbSoyRepresentanteIndividual.IsChecked = false;
        }
    }
}
