using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UCR.Negotium.Extensions
{
    public partial class ResourceDictionaryEvents:ResourceDictionary
    {
        bool tbValorAgregartxtChngEvent = true;
        private void tbAgregarValor_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tbAgregarValor = (TextBox)e.Source;
            if (tbValorAgregartxtChngEvent)
            {
                if (tbAgregarValor.BorderBrush == Brushes.Red)
                {
                    tbAgregarValor.BorderBrush = Brushes.Gray;
                }

                tbAgregarValor.Text = tbAgregarValor.Text.CheckStringFormat();
            }
            else
            {
                tbValorAgregartxtChngEvent = true;
            }
        }

        private void tbAgregarValor_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbAgregarValor = (TextBox)e.Source;
            if (tbAgregarValor.Text.Equals("0") || tbAgregarValor.Text.Equals("0.00"))
            {
                tbValorAgregartxtChngEvent = false;
                tbAgregarValor.Text = "";
            }
        }

        private void tbAgregarValor_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbAgregarValor = (TextBox)e.Source;
            if (tbAgregarValor.Text.Equals(""))
            {
                tbValorAgregartxtChngEvent = false;
                tbAgregarValor.Text = "0.00";
            }
        }
    }
}
