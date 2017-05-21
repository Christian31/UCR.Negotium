using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarVariacionAnualCostos.xaml
    /// </summary>
    public partial class RegistrarVariacionAnualCostos : MetroWindow
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private List<VariacionAnualCosto> variacionCostos;
        private Proyecto proyecto;

        private VariacionAnualCostoData variacionCostoData;
        private ProyectoData proyectoData;
        #endregion

        #region Constructor
        public RegistrarVariacionAnualCostos(int codProyecto)
        {
            InitializeComponent();
            DataContext = this;

            variacionCostos = new List<VariacionAnualCosto>();
            proyecto = new Proyecto();

            variacionCostoData = new VariacionAnualCostoData();
            proyectoData = new ProyectoData();

            variacionCostos = variacionCostoData.GetVariacionAnualCostos(codProyecto);
            proyecto = proyectoData.GetProyecto(codProyecto);

            if (variacionCostos.Count.Equals(0))
            {
                LoadDefaultValues();
            }
        }
        #endregion

        #region Properties
        public List<VariacionAnualCosto> VariacionCostos
        {
            get
            {
                return variacionCostos;
            }
            set
            {
                variacionCostos = value;
            }
        }

        public bool Reload { get; set; }
        #endregion

        #region Events
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                foreach(VariacionAnualCosto variacionAnual in VariacionCostos)
                {
                    if (variacionAnual.CodVariacionCosto.Equals(0))
                    {
                        VariacionAnualCosto variacionTemp = variacionCostoData.InsertarVariacionAnualCosto(variacionAnual, proyecto.CodProyecto);
                        if (!variacionTemp.CodVariacionCosto.Equals(0))
                        {
                            //success
                            Reload = true;
                        }
                        else
                        {
                            //error
                            MessageBox.Show("Ha ocurrido un error al insertar la variacion anual de costos del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                    }
                    else
                    {
                        if (variacionCostoData.EditarVariacionAnualCosto(variacionAnual))
                        {
                            //success
                            Reload = true;
                        }
                        else
                        {
                            //error
                            MessageBox.Show("Ha ocurrido un error al actualizar la variacion anual de costos del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                    }
                }

                if (Reload)
                    Close();
            }
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
            else if (dgVariacionAnual.BorderBrush == Brushes.Red)
            {
                dgVariacionAnual.BorderBrush = Brushes.Gray;
                dgVariacionAnual.ToolTip = string.Empty;
            }
        }
        #endregion

        #region PrivateMethods
        private void LoadDefaultValues()
        {
            for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                int anoActual = proyecto.AnoInicial + i;
                variacionCostos.Add(new VariacionAnualCosto() { Ano = anoActual });
            }//for
        }

        private bool ValidateRequiredFields()
        {
            bool validationResult = false;

            foreach (VariacionAnualCosto variacionAnual in VariacionCostos)
            {
                if (variacionAnual.PorcentajeIncremento <= 0)
                {
                    dgVariacionAnual.BorderBrush = Brushes.Red;
                    dgVariacionAnual.ToolTip = CAMPOREQUERIDO;
                    validationResult = true;
                    break;
                }
            }

            return validationResult;
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
        #endregion
    }
}
