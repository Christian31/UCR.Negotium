using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarTasaInteresFijo.xaml
    /// </summary>
    public partial class RegistrarTasaInteresFijo : MetroWindow
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";
        
        private int codProyecto;
        private InteresFinanciamiento interesFinanciamiento;
        #endregion

        #region Constructor
        public RegistrarTasaInteresFijo(int codProyecto, Financiamiento financiamiento)
        {
            InitializeComponent();
            DataContext = this;
            interesFinanciamiento = new InteresFinanciamiento();

            this.codProyecto = codProyecto;
            List<InteresFinanciamiento> intereses = financiamiento.TasaIntereses;
            
            if (intereses.Count == 0 || intereses.Count != 1
                || intereses.First().AnoInteres == 0)
            {
                interesFinanciamiento = new InteresFinanciamiento() { AnoInteres = financiamiento.AnoInicialPago };
            }
            else
            {
                interesFinanciamiento = intereses.First();
            }

            InteresFijo = interesFinanciamiento;
        }
        #endregion

        #region Events
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Reload = !ValidateRequiredFields();

            if (Reload)
                Close();
        }

        private void tbTasaInteresFijo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbTasaInteresFijo.BorderBrush == Brushes.Red)
            {
                tbTasaInteresFijo.BorderBrush = Brushes.Gray;
                tbTasaInteresFijo.ToolTip = string.Empty;
            }

            if (tbNumeroChngEvent)
            {
                tbTasaInteresFijo.Text = tbTasaInteresFijo.Text.CheckStringFormat();
            }
            else
            {
                tbNumeroChngEvent = true;
            }
        }        

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Properties
        public bool Reload { get; set; }

        public InteresFinanciamiento InteresFijo
        {
            get { return interesFinanciamiento; }
            set { interesFinanciamiento = value; }
        }
        #endregion

        #region PrivateMethods
        private bool ValidaNumeros(string valor)
        {
            double n;
            return double.TryParse(valor, out n);
        }

        private bool ValidateRequiredFields()
        {
            bool validationResult = false;

            if (InteresFijo.PorcentajeInteres <= 0)
            {
                tbTasaInteresFijo.BorderBrush = Brushes.Red;
                tbTasaInteresFijo.ToolTip = CAMPOREQUERIDO;
                validationResult = true;
            }

            return validationResult;
        }
        #endregion

        bool tbNumeroChngEvent = true;
        private void tbTasaInteresFijo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbTasaInteresFijo.Text.Equals("0") || tbTasaInteresFijo.Text.Equals("0.00"))
            {
                tbNumeroChngEvent = false;
                tbTasaInteresFijo.Text = "";
            }
        }

        private void tbTasaInteresFijo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbTasaInteresFijo.Text.Equals(""))
            {
                tbNumeroChngEvent = false;
                tbTasaInteresFijo.Text = "0.00";
            }
        }
    }
}
