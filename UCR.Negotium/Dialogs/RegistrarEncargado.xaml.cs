using MahApps.Metro.Controls;
using System.Windows;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarEncargado.xaml
    /// </summary>
    public partial class RegistrarEncargado :MetroWindow
    {
        private EncargadoData encargadoData;
        private Encargado encargado;

        public RegistrarEncargado(int codEncargado=0)
        {
            InitializeComponent();
            DataContext = this;

            encargadoData = new EncargadoData();
            encargado = new Encargado();

            if (codEncargado != 0)
            {
                encargado = encargadoData.GetEncargado(codEncargado);
            }
        }

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
                        //Reload = true;
                        EncargadoSelected.IdEncargado = idEncargado;
                        Close();
                        RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow();
                        Close();
                        registrarProyecto.ShowDialog();
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
                        //Reload = true;
                        Close();
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

        private bool ValidateFields()
        {
            return true;
        }
    }
}
