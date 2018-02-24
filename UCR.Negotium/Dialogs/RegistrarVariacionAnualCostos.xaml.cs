using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

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
                //load default values
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    int anoActual = proyecto.AnoInicial + i;
                    variacionCostos.Add(new VariacionAnualCosto() { Ano = anoActual });
                }
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
            foreach (VariacionAnualCosto variacionAnual in VariacionCostos)
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

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        bool tbNumeroChngEvent = true;
        private void tbNumerosPositivos_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox val = (TextBox)sender;
            if (val.Text.Equals("0") || val.Text.Equals("0.00"))
            {
                tbNumeroChngEvent = false;
                val.Text = "";
            }
        }

        private void tbNumerosPositivos_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox val = (TextBox)sender;
            if (val.Text.Equals(""))
            {
                tbNumeroChngEvent = false;
                val.Text = "0.00";
            }
        }

        private void tbDatosPositivos_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNumeroChngEvent)
            {
                TextBox val = (TextBox)sender;
                val.Text = val.Text.CheckStringFormat();
            }
            else
            {
                tbNumeroChngEvent = true;
            }
        }
        #endregion
    }
}
