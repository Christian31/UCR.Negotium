using MahApps.Metro.Controls;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for IndicadoresFinales.xaml
    /// </summary>
    public partial class IndicadoresFinales : MetroWindow, INotifyPropertyChanged
    {
        private double tir;
        private double van;
        private Proyecto proyecto;
        private double montoInicial;
        private double[] flujoCaja;
        private string signoMoneda;

        private ProyectoData proyectoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public IndicadoresFinales(int codProyecto, double TIR, double VAN, double montoInicial, double[] flujoCaja)
        {
            InitializeComponent();
            DataContext = this;

            tir = TIR;
            van = VAN;
            this.montoInicial = montoInicial;
            this.flujoCaja = flujoCaja;

            proyectoData = new ProyectoData();
            proyecto = proyectoData.GetProyecto(codProyecto);
            signoMoneda = LocalContext.GetSignoMoneda(codProyecto);
        }

        #region Properties
        public bool Reload { get; set; }

        public string TIR
        {
            get { return string.Concat(tir.ToString("#,##0.##"), " %"); }
            set { }
        }

        public string VAN
        {
            get { return signoMoneda +" "+ van.ToString("#,##0.##"); }
            set { }
        }

        public string VANPersonas
        {
            get
            {
                return signoMoneda +" "+ Math.Round(
                    van / ProyectoSelected.PersonasParticipantes, 2).ToString("#,##0.##");
            }
            set { }
        }

        public string VANFamilias
        {
            get
            {
                return signoMoneda + " " + Math.Round(
                    van / ProyectoSelected.FamiliasInvolucradas, 2).ToString("#,##0.##");
            }
            set { }
        }

        public string VANBeneficiarios
        {
            get
            {
                return signoMoneda + " " + Math.Round(
                    van / ProyectoSelected.PersonasBeneficiadas, 2).ToString("#,##0.##");
            }
            set { }
        }

        public Proyecto ProyectoSelected
        {
            get { return proyecto; }
            set { proyecto = value; }
        }
        #endregion

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (proyectoData.EditarProyectoFlujoCaja(ProyectoSelected))
            {
                Reload = true;
                Close();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error al actualizar el proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbTasaCostoCapital_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNumeroTextChange)
            {
                tbTasaCostoCapital.Text = tbTasaCostoCapital.Text.CheckStringFormat();
                double tasaTemp = Convert.ToDouble(tbTasaCostoCapital.Text) / 100;

                try
                {
                    van = Financial.NPV(tasaTemp, ref flujoCaja);
                    van = van + montoInicial;
                }
                catch { van = 0; }
                finally
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("VAN"));
                    PropertyChanged(this, new PropertyChangedEventArgs("VANPersonas"));
                    PropertyChanged(this, new PropertyChangedEventArgs("VANFamilias"));
                    PropertyChanged(this, new PropertyChangedEventArgs("VANBeneficiarios"));
                }
            }
            else
            {
                tbNumeroTextChange = true;
            }
        }

        bool tbNumeroTextChange = true;
        private void tbNumeros_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbTasaCostoCapital.Text.Equals("0") || tbTasaCostoCapital.Text.Equals("0.00"))
            {
                tbNumeroTextChange = false;
                tbTasaCostoCapital.Text = "";
            }
        }

        private void tbNumeros_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbTasaCostoCapital.Text.Equals(""))
            {
                tbNumeroTextChange = false;
                tbTasaCostoCapital.Text = "0.00";
            }
        }

        private void tbPersonasParticipantes_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPersonasParticipantes.Text = tbPersonasParticipantes.Text.CheckStringFormat();
            PropertyChanged(this, new PropertyChangedEventArgs("VANPersonas"));
        }

        private void tbFamiliasInvolucradas_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbFamiliasInvolucradas.Text = tbFamiliasInvolucradas.Text.CheckStringFormat();
            PropertyChanged(this, new PropertyChangedEventArgs("VANFamilias"));
        }

        private void tbPersonasBeneficiadas_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPersonasBeneficiadas.Text = tbPersonasBeneficiadas.Text.CheckStringFormat();

            PropertyChanged(this, new PropertyChangedEventArgs("VANBeneficiarios"));
        }
    }
}
