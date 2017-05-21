using MahApps.Metro.Controls;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public double TIR
        {
            get { return tir; }
            set { tir = value; }
        }

        public double VAN
        {
            get { return van; }
            set
            {
                van = value;
                PropertyChanged(this, new PropertyChangedEventArgs("VAN"));
                PropertyChanged(this, new PropertyChangedEventArgs("VANPersonas"));
                PropertyChanged(this, new PropertyChangedEventArgs("VANFamilias"));
                PropertyChanged(this, new PropertyChangedEventArgs("VANBeneficiarios"));
            }
        }

        public double VANPersonas
        {
            get { return VAN / ProyectoSelected.PersonasParticipantes; }
            set { VAN = value * ProyectoSelected.PersonasParticipantes; }
        }

        public double VANFamilias
        {
            get { return VAN / ProyectoSelected.FamiliasInvolucradas; }
            set { VAN = value * ProyectoSelected.FamiliasInvolucradas; }
        }

        public double VANBeneficiarios
        {
            get { return VAN / ProyectoSelected.PersonasBeneficiadas; }
            set { VAN = value * ProyectoSelected.PersonasBeneficiadas; }
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
                VAN = montoInicial + Financial.NPV(Convert.ToDouble(val.Text), ref flujoCaja);

                //VAN = string.Concat("₡ ", num2.ToString("#,##0.##"));
            }
            catch { VAN = 0; }
        }
    }
}
