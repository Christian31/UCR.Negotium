using MahApps.Metro.Controls;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarEncargado.xaml
    /// </summary>
    public partial class RegistrarEncargado :MetroWindow
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";
        private Regex emailExpresion = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        private Regex numbers = new Regex(@"([^\d]*\d){8,}");

        private bool successValue = false;
        private bool redirectToMain = false;

        private EncargadoData encargadoData;
        private Encargado encargado;

        public RegistrarEncargado(int codEncargado=-1)
        {
            InitializeComponent();
            DataContext = this;

            redirectToMain = codEncargado == -1;
            encargadoData = new EncargadoData();
            encargado = new Encargado();

            if (codEncargado > 0)
            {
                encargado = encargadoData.GetEncargado(codEncargado);
            }
        }

        #region Properties
        public Encargado EncargadoSelected
        {
            get
            {
                return encargado;
            }
            set
            {
                encargado = value;
            }
        }
        #endregion

        #region Events
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                if (EncargadoSelected.IdEncargado.Equals(0))
                {
                    int idEncargado = encargadoData.InsertarEncargado(EncargadoSelected);
                    if (!idEncargado.Equals(-1))
                    {
                        //success
                        successValue = true;
                        EncargadoSelected.IdEncargado = idEncargado;

                        if (redirectToMain)
                        {
                            RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow(codEncargado: EncargadoSelected.IdEncargado);
                            this.Close();
                            registrarProyecto.Show();
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar el Encargado del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (encargadoData.EditarEncargado(EncargadoSelected))
                    {
                        //success
                        successValue = true;

                        if (redirectToMain)
                        {
                            RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow(codEncargado: EncargadoSelected.IdEncargado);
                            this.Close();
                            registrarProyecto.Show();
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar el Encargado del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!successValue && redirectToMain)
            {
                successValue = false;
                MainWindow main = new MainWindow();
                main.Show();
            }
        }

        private void tbNombreEncargado_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tbNombreEncargado.BorderBrush == Brushes.Red)
            {
                tbNombreEncargado.BorderBrush = Brushes.Gray;
                tbNombreEncargado.ToolTip = "";
            }
        }

        private void tbApellidos_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tbApellidos.BorderBrush == Brushes.Red)
            {
                tbApellidos.BorderBrush = Brushes.Gray;
                tbApellidos.ToolTip = "";
            }
        }

        private void tbNombreOrganizacion_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tbNombreOrganizacion.BorderBrush == Brushes.Red)
            {
                tbNombreOrganizacion.BorderBrush = Brushes.Gray;
                tbNombreOrganizacion.ToolTip = "";
            }
        }

        private void tbTelefono_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tbTelefono.BorderBrush == Brushes.Red)
            {
                tbTelefono.BorderBrush = Brushes.Gray;
                tbTelefono.ToolTip = "";
            }
        }

        private void tbEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tbEmail.BorderBrush == Brushes.Red)
            {
                tbEmail.BorderBrush = Brushes.Gray;
                tbEmail.ToolTip = "";
            }
        }
        #endregion

        #region InternalMethods
        private bool ValidateFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbNombreEncargado.Text))
            {
                tbNombreEncargado.ToolTip = CAMPOREQUERIDO;
                tbNombreEncargado.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (string.IsNullOrWhiteSpace(tbApellidos.Text))
            {
                tbApellidos.ToolTip = CAMPOREQUERIDO;
                tbApellidos.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (string.IsNullOrWhiteSpace(tbNombreOrganizacion.Text))
            {
                tbNombreOrganizacion.ToolTip = CAMPOREQUERIDO;
                tbNombreOrganizacion.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (!numbers.IsMatch(tbTelefono.Text))
            {
                tbTelefono.ToolTip = CAMPOREQUERIDO;
                tbTelefono.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (!emailExpresion.IsMatch(tbEmail.Text))
            {
                tbEmail.ToolTip = CAMPOREQUERIDO;
                tbEmail.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            return validationResult;
        }
        #endregion
    }
}
