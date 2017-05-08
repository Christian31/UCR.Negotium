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
    /// Interaction logic for RegistrarTasaInteresFinanciamiento.xaml
    /// </summary>
    public partial class RegistrarTasaInteresFinanciamiento : MetroWindow
    {
        public RegistrarTasaInteresFinanciamiento( )
        {
            InitializeComponent();
            DataContext = this;
        }

        public bool Reload { get; set; }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            //if (!ValidateRequiredFields())
            //{
            //    foreach (VariacionAnualCosto variacionAnual in VariacionCostos)
            //    {
            //        if (variacionAnual.CodVariacionCosto.Equals(0))
            //        {
            //            VariacionAnualCosto variacionTemp = variacionCostoData.InsertarVariacionAnualCosto(variacionAnual, proyecto.CodProyecto);
            //            if (!variacionTemp.CodVariacionCosto.Equals(0))
            //            {
            //                //success
            //                Reload = true;
            //            }
            //            else
            //            {
            //                //error
            //                MessageBox.Show("Ha ocurrido un error al insertar la variacion anual de costos del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            if (variacionCostoData.EditarVariacionAnualCosto(variacionAnual))
            //            {
            //                //success
            //                Reload = true;
            //            }
            //            else
            //            {
            //                //error
            //                MessageBox.Show("Ha ocurrido un error al actualizarla variacion anual de costos del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
            //                break;
            //            }
            //        }
            //    }

            //    if (Reload)
            //        Close();
            //}
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbDatosPositivos_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox val = (TextBox)sender;
            if (!val.Text.Equals(string.Empty) && !ValidaNumeros(val.Text))
            {
                val.Text = 0.ToString();
            }
            else if (dgPorcentajes.BorderBrush == Brushes.Red)
            {
                dgPorcentajes.BorderBrush = Brushes.Gray;
                dgPorcentajes.ToolTip = string.Empty;
            }
        }

        private bool ValidaNumeros(string valor)
        {
            double n;
            if (double.TryParse(valor, out n))
            {
                if (n >= 0)
                    return true;
            }

            return false;
        }
    }
}
