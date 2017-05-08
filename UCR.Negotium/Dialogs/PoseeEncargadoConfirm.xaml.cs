using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for PoseeEncargadoConfirm.xaml
    /// </summary>
    public partial class PoseeEncargadoConfirm : MetroWindow
    {
        public PoseeEncargadoConfirm()
        {
            InitializeComponent();
            tbMensajeConfirm.AppendText("¿Recibe usted asesoría de alguna Entidad, representante " +
                "o encargado con conocimientos técnicos para ingresar toda la información " +
                "respectiva a este proyecto?");
        }

        private void btnSi_Click(object sender, RoutedEventArgs e)
        {
            RegistrarEncargado registrarEncargado = new RegistrarEncargado();
            Close();
            registrarEncargado.ShowDialog();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow();
            Close();
            registrarProyecto.ShowDialog();
        }
    }
}
