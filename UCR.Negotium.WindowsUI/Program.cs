//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Encargado evaluador = new Encargado(1, "Yordan", "apellidos", "72056090", "ycampospiedra@gmail.com", "3-0476-0003");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VentanaPrincipal(evaluador));
        }
    }
}
