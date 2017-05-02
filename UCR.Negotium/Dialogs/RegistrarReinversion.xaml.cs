using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarReinversionDialog.xaml
    /// </summary>
    public partial class RegistrarReinversion : MetroWindow
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";
        private const string CAMPOREQUERIDOPOSITIVO = "Este campo es requerido y debe tener un valor mayor a 0";

        private RequerimientoReinversionData requerimientoReinversionData;
        private ProyectoData proyectoData;
        private UnidadMedidaData unidadMedidaData;
        private RequerimientoReinversion reinversion;
        private List<UnidadMedida> unidadMedidas;
        private Proyecto proyecto;

        public RegistrarReinversion(int codProyecto, int codReinversion = 0)
        {
            InitializeComponent();
            DataContext = this;
            tbDescReinversion.ToolTip = "Ingrese en este campo el Nombre de la Reinversión que desea registrar";
            tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Reinversión que desea registrar";

            proyecto = new Proyecto();

            requerimientoReinversionData = new RequerimientoReinversionData();
            proyectoData = new ProyectoData();
            unidadMedidaData = new UnidadMedidaData();

            reinversion = new RequerimientoReinversion();
            unidadMedidas = new List<UnidadMedida>();

            proyecto = proyectoData.GetProyecto(codProyecto);
            unidadMedidas = unidadMedidaData.GetUnidadesMedidaAux();
            reinversion.UnidadMedida = unidadMedidas.FirstOrDefault();
            reinversion.AnoReinversion = AnosDisponibles.FirstOrDefault();

            if (codReinversion != 0)
                reinversion = requerimientoReinversionData.GetRequerimientoReinversion(codReinversion);
        }

        public bool Reload { get; set; }

        public List<UnidadMedida> UnidadesMedida
        {
            get
            {
                return unidadMedidas;
            }
            set
            {
                unidadMedidas = value;
            }
        }

        public List<int> AnosDisponibles
        {
            get
            {
                List<int> anos = new List<int>();
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    int anoActual = proyecto.AnoInicial + i;
                    anos.Add(anoActual);
                }//for

                return anos;
            }
            set
            {
                AnosDisponibles = value;
            }
        } 

        public RequerimientoReinversion Reinversion
        {
            get
            {
                return reinversion;
            }
            set
            {
                reinversion = value;
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (!Reinversion.Depreciable)
                    Reinversion.VidaUtil = 0;

                if (Reinversion.CodRequerimientoInversion.Equals(0))
                {
                    int idInversion = requerimientoReinversionData.InsertarRequerimientosReinversion(Reinversion, proyecto.CodProyecto);
                    if (!idInversion.Equals(-1))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar la reinversión del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (requerimientoReinversionData.EditarRequerimientoReinversion(Reinversion))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar la reinversión del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void tbCostoUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCostoUnitario.BorderBrush == Brushes.Red)
            {
                tbCostoUnitario.BorderBrush = Brushes.Gray;
                tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Reinversión que desea registrar";
            }
            int costoUnitario = 0;
            if (!int.TryParse(tbCostoUnitario.Text, out costoUnitario))
            {
                tbCostoUnitario.Text = string.Empty;
            }
        }

        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbDescReinversion.Text))
            {
                tbDescReinversion.ToolTip = CAMPOREQUERIDO;
                tbDescReinversion.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (Convert.ToInt64(tbCostoUnitario.Text.ToString()) <= 0)
            {
                tbCostoUnitario.ToolTip = CAMPOREQUERIDOPOSITIVO;
                tbCostoUnitario.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            return validationResult;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cbDepreciable_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbDepreciable.IsChecked.Value.Equals(false))
            {
                cbNoDepreciable.IsChecked = true;
                nudVidaUtil.IsEnabled = false;
            }
        }

        private void cbDepreciable_Checked(object sender, RoutedEventArgs e)
        {
            cbNoDepreciable.IsChecked = false;
            nudVidaUtil.IsEnabled = true;
        }

        private void cbNoDepreciable_Checked(object sender, RoutedEventArgs e)
        {
            cbDepreciable.IsChecked = nudVidaUtil.IsEnabled = false;
        }

        private void tbDescReinversion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDescReinversion.BorderBrush == Brushes.Red)
            {
                tbDescReinversion.BorderBrush = Brushes.Gray;
                tbDescReinversion.ToolTip = "Ingrese en este campo el Nombre de la Reinversión que desea registrar";
            }
        }
    }
}
