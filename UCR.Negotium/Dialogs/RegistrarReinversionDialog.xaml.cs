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
    /// Interaction logic for RegistrarReinversionDialog.xaml
    /// </summary>
    public partial class RegistrarReinversionDialog : MetroWindow
    {
        public RegistrarReinversionDialog(int idProyecto, int idEvaluador=0, int idReinversion=0)
        {
            InitializeComponent();
        }
    }
}
