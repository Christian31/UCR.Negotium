using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            var application = new App();
            application.InitializeComponent();
            application.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
              new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Ha ocurrido un error inesperado, favor inicie la Aplicación de nuevo. Mensaje: " + e.Exception.Message, "Negotium", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            Current.Shutdown();
        }
    }
}
