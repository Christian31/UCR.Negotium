using System;
using System.ComponentModel;
using System.Windows;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (_, __) =>
            {
                //time to keep this window showed (6 seconds)
                System.Threading.Thread.Sleep(6000);
                OpenMainWindow();
            };
            backgroundWorker.RunWorkerAsync();
        }

        private void OpenMainWindow()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            });
        }
    }
}
