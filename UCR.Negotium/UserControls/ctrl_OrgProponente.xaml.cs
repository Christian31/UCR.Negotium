using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_Proponente.xaml
    /// </summary>
    public partial class ctrl_OrgProponente : UserControl, INotifyPropertyChanged
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";
        private Regex emailExpresion = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        private Regex numbers = new Regex(@"([^\d]*\d){8,}");

        private OrganizacionProponente orgProponente;
        private List<TipoOrganizacion> tipoOrganizaciones;
        private int codProyecto { get; set; }
        private Proyecto proyecto;

        private OrganizacionProponenteData orgProponenteData;
        private ProponenteData proponenteData;
        private TipoOrganizacionData tipoOrganizacionData;
        private ProyectoData proyectoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_OrgProponente()
        {
            InitializeComponent();
            DataContext = this;

            orgProponenteData = new OrganizacionProponenteData();
            proponenteData = new ProponenteData();
            orgProponente = new OrganizacionProponente();
            proyecto = new Proyecto();

            tipoOrganizacionData = new TipoOrganizacionData();
            proyectoData = new ProyectoData();

            tipoOrganizaciones = new List<TipoOrganizacion>();

            tipoOrganizaciones = tipoOrganizacionData.GetTipoOrganizaciones();
        }

        public void Reload()
        {
            OrgProponente = orgProponenteData.GetOrganizacionProponente(CodProyecto);
            MantenerCambios();
            proyecto = proyectoData.GetProyecto(CodProyecto);
        }

        #region Properties
        public int CodProyecto
        {
            get { return codProyecto; }
            set { codProyecto = value; Reload(); }
        }

        public List<TipoOrganizacion> TipoOrganizaciones
        {
            get { return tipoOrganizaciones; }
            set { tipoOrganizaciones = value; }
        }

        public OrganizacionProponente OrgProponente
        {
            get { return orgProponente; }
            set { orgProponente = value; }
        }
        #endregion

        #region Events
        private void btnGuardarProponente(object sender, RoutedEventArgs e)
        {
            if (!proyecto.TipoProyecto.CodTipo.Equals(2))
            {
                if (!ValidateRequiredFields())
                {
                    if (OrgProponente.CodOrganizacion.Equals(0))
                    {
                        int codOrganizacion = orgProponenteData.InsertarOrganizacionProponente(OrgProponente, CodProyecto);
                        if (!codOrganizacion.Equals(-1))
                        {
                            OrgProponente.CodOrganizacion = codOrganizacion;
                            int codProponente = proponenteData.InsertarProponente(OrgProponente.Proponente, OrgProponente.CodOrganizacion);
                            if (!codProponente.Equals(-1))
                            {
                                //success
                                OrgProponente.Proponente.IdProponente = codProponente;
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
                        if (proponenteData.EditarProponente(OrgProponente.Proponente) &&
                            orgProponenteData.EditarOrganizacionProponente(OrgProponente))
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
            else
            {
                MessageBox.Show("Este Tipo de Análisis es Ambiental, si desea realizar un Análisis Completo actualice el Tipo de Análisis del Proyecto", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        #endregion

        #region InternalMethods
        public bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbNombreProponente.Text))
            {
                tbNombreProponente.ToolTip = CAMPOREQUERIDO;
                tbNombreProponente.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (string.IsNullOrWhiteSpace(tbApellidosProponente.Text))
            {
                tbApellidosProponente.ToolTip = CAMPOREQUERIDO;
                tbApellidosProponente.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (string.IsNullOrWhiteSpace(tbCedulaProponente.Text))
            {
                tbCedulaProponente.ToolTip = CAMPOREQUERIDO;
                tbCedulaProponente.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (!numbers.IsMatch(tbTelefonoProponente.Text))
            {
                tbTelefonoProponente.ToolTip = CAMPOREQUERIDO;
                tbTelefonoProponente.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (!emailExpresion.IsMatch(tbCorreoProponente.Text))
            {
                tbCorreoProponente.ToolTip = CAMPOREQUERIDO;
                tbCorreoProponente.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            if (!OrgProponente.Proponente.EsRepresentanteIndividual)
            {
                if (string.IsNullOrWhiteSpace(tbNombreOrganizacion.Text))
                {
                    tbNombreOrganizacion.ToolTip = CAMPOREQUERIDO;
                    tbNombreOrganizacion.BorderBrush = Brushes.Red;
                    validationResult = true;
                }
                if (string.IsNullOrWhiteSpace(tbCedulaOrganizacion.Text))
                {
                    tbCedulaOrganizacion.ToolTip = CAMPOREQUERIDO;
                    tbCedulaOrganizacion.BorderBrush = Brushes.Red;
                    validationResult = true;
                }
                if (!numbers.IsMatch(tbTelefonoOrganizacion.Text))
                {
                    tbTelefonoOrganizacion.ToolTip = CAMPOREQUERIDO;
                    tbTelefonoOrganizacion.BorderBrush = Brushes.Red;
                    validationResult = true;
                }

                if (!emailExpresion.IsMatch(tbCorreoOrganizacion.Text))
                {
                    tbCorreoOrganizacion.ToolTip = CAMPOREQUERIDO;
                    tbCorreoOrganizacion.BorderBrush = Brushes.Red;
                    validationResult = true;
                }
            }

            return validationResult;
        }

        #endregion

        int tipoOrg;
        string nombreOrg;
        string cedulaOrg;
        string telefonoOrg;
        string correoOrg;

        private void cbSoyRepresentanteIndividual_Checked(object sender, RoutedEventArgs e)
        {
            MantenerCambios(limpiarCambios:true);
            cbTipoOrganizaciones.SelectedItem = TipoOrganizaciones.LastOrDefault();
        }

        private void cbNoSoyRepresentanteIndividual_Checked(object sender, RoutedEventArgs e)
        {
            MantenerCambios(recuperarCambios:true);
        }

        private void MantenerCambios(bool recuperarCambios = false, bool limpiarCambios = false)
        {
            if (recuperarCambios)
            {
                OrgProponente.NombreOrganizacion = nombreOrg;
                OrgProponente.CedulaJuridica = cedulaOrg;
                OrgProponente.Telefono = telefonoOrg;
                OrgProponente.CorreoElectronico = correoOrg;
                OrgProponente.Tipo.CodTipo = tipoOrg;
            }
            else
            {
                tipoOrg = OrgProponente.Tipo.CodTipo;
                nombreOrg = OrgProponente.NombreOrganizacion;
                cedulaOrg = OrgProponente.CedulaJuridica;
                telefonoOrg = OrgProponente.Telefono;
                correoOrg = OrgProponente.CorreoElectronico;

                if (limpiarCambios)
                {
                    OrgProponente.NombreOrganizacion =
                    OrgProponente.CedulaJuridica =
                    OrgProponente.Telefono =
                    OrgProponente.CorreoElectronico = string.Empty;
                }
            }

            PropertyChanged(this, new PropertyChangedEventArgs("OrgProponente"));
        }

        private void tbNombreProponente_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNombreProponente.BorderBrush == Brushes.Red)
            {
                tbNombreProponente.BorderBrush = Brushes.Gray;
                tbNombreProponente.ToolTip = "";
            }
        }

        private void tbApellidosProponente_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbApellidosProponente.BorderBrush == Brushes.Red)
            {
                tbApellidosProponente.BorderBrush = Brushes.Gray;
                tbApellidosProponente.ToolTip = "";
            }
        }

        private void tbCedulaProponente_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCedulaProponente.BorderBrush == Brushes.Red)
            {
                tbCedulaProponente.BorderBrush = Brushes.Gray;
                tbCedulaProponente.ToolTip = "";
            }
        }

        private void tbTelefonoProponente_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbTelefonoProponente.BorderBrush == Brushes.Red)
            {
                tbTelefonoProponente.BorderBrush = Brushes.Gray;
                tbTelefonoProponente.ToolTip = "";
            }
        }

        private void tbNombreOrganizacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNombreOrganizacion.BorderBrush == Brushes.Red)
            {
                tbNombreOrganizacion.BorderBrush = Brushes.Gray;
                tbNombreOrganizacion.ToolTip = "";
            }
        }

        private void tbCedulaOrganizacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCedulaOrganizacion.BorderBrush == Brushes.Red)
            {
                tbCedulaOrganizacion.BorderBrush = Brushes.Gray;
                tbCedulaOrganizacion.ToolTip = "";
            }
        }

        private void tbTelefonoOrganizacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbTelefonoOrganizacion.BorderBrush == Brushes.Red)
            {
                tbTelefonoOrganizacion.BorderBrush = Brushes.Gray;
                tbTelefonoOrganizacion.ToolTip = "";
            }
        }

        private void tbCorreoProponente_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCorreoProponente.BorderBrush == Brushes.Red)
            {
                tbCorreoProponente.BorderBrush = Brushes.Gray;
                tbCorreoProponente.ToolTip = "";
            }
        }

        private void tbCorreoOrganizacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCorreoOrganizacion.BorderBrush == Brushes.Red)
            {
                tbCorreoOrganizacion.BorderBrush = Brushes.Gray;
                tbCorreoOrganizacion.ToolTip = "";
            }
        }
    }
}
