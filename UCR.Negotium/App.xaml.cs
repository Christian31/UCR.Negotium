using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UCR.Negotium.Utils;

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
            if (!Installation.IsInstalled())
            {
                if (Installation.IsRunAsAdmin())
                {
                    if (Installation.RunInstallation())
                    {
                        var application = new App();
                        application.InitializeComponent();
                        application.Run();
                    }
                }
                else
                {
                    MessageBox.Show("Usted necesita ejecutar este programa en Modo Administrador para poder utilizarlo por primera vez"+ Environment.NewLine +"Por favor ejecute el programa en Modo Administrador",
                        "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();
            }
        }
    }
}
