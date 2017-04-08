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
    /// Interaction logic for RegistrarInversionDialog.xaml
    /// </summary>
    public partial class RegistrarInversionDialog : MetroWindow
    {
        public RegistrarInversionDialog(int idProyecto, int idInversion=0)
        {
            InitializeComponent();
        }

        private void tbCostoUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {
            //tbCostoUnitario.Text = tbCostoUnitario.Text.ToString("N0");
        }
    }
}
