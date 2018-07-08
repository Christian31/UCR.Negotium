using NLog;
using System;
using System.Windows;
using System.Windows.Threading;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        DispatcherTimer timer = null;

        public StartWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(6);
            timer.Tick += new EventHandler(TimerElapsed);
            timer.Start();
        }
        
        private void TimerElapsed(object sender, EventArgs e)
        {
            timer.Stop();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
