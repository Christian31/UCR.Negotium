using MahApps.Metro.Controls;
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
        private IndicadorEconomico tir, pri, relacionBC, van, vac;
        private string signoMoneda;
        private Proyecto proyecto;
        private IndicadoresFinancieros indicFinancieros;
        private IndicadoresSociales indicSociales;
        private bool esSocial;

        private ProyectoData proyectoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public IndicadoresFinales(int codProyecto, object indicadores)
        {
            tir = pri = relacionBC = van = vac = new IndicadorEconomico();
            InitializeComponent();
            DataContext = this;

            if (indicadores.GetType() == typeof(IndicadoresFinancieros)){
                this.indicFinancieros = (IndicadoresFinancieros)indicadores;
                tir = indicFinancieros.TIR;
                pri = indicFinancieros.PRI;
                van = indicFinancieros.VAN;
                relacionBC = indicFinancieros.RelacionBC;
            }
            else
            {
                this.indicSociales = (IndicadoresSociales)indicadores;
                esSocial = true;
                vac = indicSociales.VAC;
                indicadoresFinancieros.Visibility = Visibility.Hidden;
                indicadoresSociales.Visibility = Visibility.Visible;
            }

            proyectoData = new ProyectoData();
            proyecto = proyectoData.GetProyecto(codProyecto);
            signoMoneda = LocalContext.GetSignoMoneda(codProyecto);
        }

        #region Properties
        public bool Reload { get; set; }

        public string TIR
        {
            get { return tir.ToString(); }
            set { }
        }

        public string PRI
        {
            get { return pri.ToString(); }
            set { }
        }

        public string RelacionBC
        {
            get { return relacionBC.ToString(); }
            set { }
        }

        public string VAN
        {
            get { return van.ToString(); }
            set { }
        }

        public string VANPersonas
        {
            get
            {
                return van.EvaluarPorCantidad(ProyectoSelected.PersonasParticipantes);
            }
            set { }
        }

        public string VANFamilias
        {
            get
            {
                return van.EvaluarPorCantidad(ProyectoSelected.FamiliasInvolucradas);
            }
            set { }
        }

        public string VANBeneficiarios
        {
            get
            {
                return van.EvaluarPorCantidad(ProyectoSelected.PersonasBeneficiadas);
            }
            set { }
        }

        public string VAC
        {
            get { return vac.ToString(); }
            set { }
        }

        public string VACPersonas
        {
            get
            {
                return vac.EvaluarPorCantidad(ProyectoSelected.PersonasParticipantes);
            }
            set { }
        }

        public string VACFamilias
        {
            get
            {
                return vac.EvaluarPorCantidad(ProyectoSelected.FamiliasInvolucradas);
            }
            set { }
        }

        public string VACBeneficiarios
        {
            get
            {
                return vac.EvaluarPorCantidad(ProyectoSelected.PersonasBeneficiadas);
            }
            set { }
        }

        public Proyecto ProyectoSelected
        {
            get { return proyecto; }
            set { proyecto = value; }
        }
        #endregion

        #region Events
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (proyectoData.EditarProyectoFlujoCaja(ProyectoSelected))
            {
                Reload = true;
                Close();
            }
            else
            {
                MessageBox.Show(Constantes.ACTUALIZARPROYECTOERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                    MessageBoxButton.OK, MessageBoxImage.Error);
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

                double tasaTemp = Convert.ToDouble(tbTasaCostoCapital.Text);

                if (esSocial)
                {
                    vac = indicSociales.CalculateVAC(tasaTemp);
                    PropertyChanged(this, new PropertyChangedEventArgs("VAC"));
                    PropertyChanged(this, new PropertyChangedEventArgs("VACPersonas"));
                    PropertyChanged(this, new PropertyChangedEventArgs("VACFamilias"));
                    PropertyChanged(this, new PropertyChangedEventArgs("VACBeneficiarios"));
                }
                else
                {
                    van = indicFinancieros.CalculateVAN(tasaTemp);
                    relacionBC = indicFinancieros.CalculateRelacionBC(tasaTemp);

                    PropertyChanged(this, new PropertyChangedEventArgs("RelacionBC"));
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
            PropertyChanged(this, new PropertyChangedEventArgs("VACPersonas"));
        }

        private void tbFamiliasInvolucradas_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbFamiliasInvolucradas.Text = tbFamiliasInvolucradas.Text.CheckStringFormat();
            PropertyChanged(this, new PropertyChangedEventArgs("VANFamilias"));
            PropertyChanged(this, new PropertyChangedEventArgs("VACFamilias"));
        }

        private void tbPersonasBeneficiadas_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPersonasBeneficiadas.Text = tbPersonasBeneficiadas.Text.CheckStringFormat();
            PropertyChanged(this, new PropertyChangedEventArgs("VANBeneficiarios"));
            PropertyChanged(this, new PropertyChangedEventArgs("VACBeneficiarios"));
        }
        #endregion
    }
}
