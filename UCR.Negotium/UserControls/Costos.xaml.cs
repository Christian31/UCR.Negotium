using UCR.Negotium.Dialogs;
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

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for Costos.xaml
    /// </summary>
    public partial class Costos : UserControl
    {
        public Costos()
        {
            InitializeComponent();
        }

        private void btnCrearCosto_Click(object sender, RoutedEventArgs e)
        {
            RegistrarCostoDialog registarCosto = new RegistrarCostoDialog(26);
            registarCosto.Show();
        }
    }
}
