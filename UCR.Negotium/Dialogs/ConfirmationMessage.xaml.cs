using MahApps.Metro.Controls;
using System.Windows;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmationMessage.xaml
    /// </summary>
    public partial class ConfirmationMessage : MetroWindow
    {
        public ConfirmationMessage(string titulo, string mensaje)
        {
            InitializeComponent();

            this.Title = titulo;
            this.tbMensaje.Text = mensaje;
        }

        public bool Resultado { get; set; }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSi_Click(object sender, RoutedEventArgs e)
        {
            this.Resultado = true;
            Close();
        }
    }
}
