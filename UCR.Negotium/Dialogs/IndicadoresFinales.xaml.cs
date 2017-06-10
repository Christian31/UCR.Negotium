using MahApps.Metro.Controls;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

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
        }

        public bool Reload { get; set; }

        public string TIR
        {
            get { return string.Concat(tir.ToString("#,##0.##"), " %"); }
            set { }
        }

        public string VAN
        {
            get { return string.Concat("₡ ", van.ToString("#,##0.##")); }
            set { }
        }

        public string VANPersonas
        {
            get
            {
                return string.Concat("₡ ", (Math.Round(
                    van / ProyectoSelected.PersonasParticipantes, 2)).ToString("#,##0.##"));
            }
            set { }
        }

        public string VANFamilias
        {
            get
            {
                return string.Concat("₡ ", (Math.Round(
                    van / ProyectoSelected.FamiliasInvolucradas, 2)).ToString("#,##0.##"));
            }
            set { }
        }

        public string VANBeneficiarios
        {
            get
            {
                return string.Concat("₡ ", (Math.Round(
                    van / ProyectoSelected.PersonasBeneficiadas, 2)).ToString("#,##0.##"));
            }
            set { }
        }

        public Proyecto ProyectoSelected
        {
            get { return proyecto; }
            set { proyecto = value; }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (proyectoData.ActualizarProyectoFlujoCaja(ProyectoSelected))
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox val = (TextBox)sender;
            if (!val.Text.Equals(string.Empty) && !ValidaNumeros(val.Text, true))
            {
                val.Text = 0.ToString();
            }
            PropertyChanged(this, new PropertyChangedEventArgs("VANPersonas"));
            PropertyChanged(this, new PropertyChangedEventArgs("VANFamilias"));
            PropertyChanged(this, new PropertyChangedEventArgs("VANBeneficiarios"));

        }

        private bool ValidaNumeros(string valor, bool positivos = false)
        {
            double n;
            if (double.TryParse(valor, out n))
            {
                return positivos ? (n >= 0) : true;
            }

            return false;
        }

        private void tbTasaCostoCapital_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox val = (TextBox)sender;
            if (!val.Text.Equals(string.Empty) && !ValidaNumeros(val.Text, true))
            {
                val.Text = 0.ToString();
            }

            try
            {
                van = montoInicial + Financial.NPV(Convert.ToDouble(val.Text), ref flujoCaja);
            }
            catch { van = 0; }
            finally {
                PropertyChanged(this, new PropertyChangedEventArgs("VAN"));
                PropertyChanged(this, new PropertyChangedEventArgs("VANPersonas"));
                PropertyChanged(this, new PropertyChangedEventArgs("VANFamilias"));
                PropertyChanged(this, new PropertyChangedEventArgs("VANBeneficiarios"));
            }
        }
    }
}
