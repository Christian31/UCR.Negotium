using MahApps.Metro.Controls;
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
    /// Interaction logic for RegistrarTasaInteresFijo.xaml
    /// </summary>
    public partial class RegistrarTasaInteresFijo : MetroWindow
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private InteresFinanciamientoData interesData;
        private int codProyecto;
        private InteresFinanciamiento interesFinanciamiento;
        #endregion

        #region Constructor
        public RegistrarTasaInteresFijo(int codProyecto, Financiamiento financiamiento)
        {
            InitializeComponent();
            DataContext = this;
            interesData = new InteresFinanciamientoData();
            interesFinanciamiento = new InteresFinanciamiento();

            this.codProyecto = codProyecto;
            List<InteresFinanciamiento> intereses = interesData.GetInteresesFinanciamiento(codProyecto);
            
            if (intereses == null)
            {
                interesFinanciamiento = new InteresFinanciamiento() { AnoInteres = financiamiento.AnoInicialPago };
            }
            else if (!intereses.Count.Equals(1))
            {
                interesData.EliminarInteresFinanciamiento(codProyecto);
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
            TextBox val = (TextBox)sender;
            if (!val.Text.Equals(string.Empty) && !ValidaNumeros(val.Text))
            {
                val.Text = 0.ToString();
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
    }
}
