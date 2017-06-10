using MahApps.Metro.Controls;
using System.Windows;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for PoseeEncargadoConfirm.xaml
    /// </summary>
    public partial class PoseeEncargadoConfirm : MetroWindow
    {
        #region PrivateProperties
        private bool redirectToAnother = false;
        #endregion

        #region Constructor
        public PoseeEncargadoConfirm()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnSi_Click(object sender, RoutedEventArgs e)
        {
            redirectToAnother = true;
            RegistrarEncargado registrarEncargado = new RegistrarEncargado();
            Close();
            registrarEncargado.ShowDialog();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            redirectToAnother = true;
            RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow();
            Close();
            registrarProyecto.ShowDialog();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!redirectToAnother)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
        #endregion
    }
}
