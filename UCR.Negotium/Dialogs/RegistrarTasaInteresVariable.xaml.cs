using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarTasaInteresVariable.xaml
    /// </summary>
    public partial class RegistrarTasaInteresVariable : DialogWithDataGrid
    {
        #region PrivateProperties
        private List<InteresFinanciamiento> interesesFinanciamiento;
        private Financiamiento financiamiento = new Financiamiento();
        private int codProyecto;
        #endregion

        #region Constructor
        public RegistrarTasaInteresVariable(int codProyecto, Financiamiento financiamiento)
        {
            InitializeComponent();
            DataContext = this;

            interesesFinanciamiento = new List<InteresFinanciamiento>();

            this.codProyecto = codProyecto;
            this.financiamiento = financiamiento;
            interesesFinanciamiento = financiamiento.TasaIntereses;
            if (interesesFinanciamiento == null || interesesFinanciamiento.Count.Equals(0) ||
                !interesesFinanciamiento.Count.Equals(financiamiento.TiempoFinanciamiento))
            {
                LoadDefaultValues();
            }
        }
        #endregion

        #region Properties
        public List<InteresFinanciamiento> InteresVariable
        {
            get { return interesesFinanciamiento; }
            set { interesesFinanciamiento = value; }
        }

        public bool Reload { get; set; }
        #endregion

        #region Events
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Reload = !ValidateRequiredFields();

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
            if (dgTasaVariable.BorderBrush == Brushes.Red)
            {
                dgTasaVariable.BorderBrush = Brushes.Gray;
                dgTasaVariable.ToolTip = string.Empty;
            }

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

        private void dgTasaVariable_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject depObject = (DependencyObject)e.OriginalSource;
            bool mostrarContextMenu = ContextMenuDisponible(depObject, dgTasaVariable.SelectedCells);
            if (mostrarContextMenu)
            {
                dgTasaVariable.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dgTasaVariable.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAgregarPorcentaje_Click(object sender, RoutedEventArgs e)
        {
            bool notifyChange = false;

            var selectedCells = dgTasaVariable.SelectedCells;
            var interesesVariableSelected = selectedCells.Select(cell => cell.Item).ToList();
            var interesVariableSelect = InteresVariable.Where(interes => interesesVariableSelected.Select(cm => ((InteresFinanciamiento)cm).AnoInteres).
            Contains(interes.AnoInteres)).ToList();

            if (selectedCells[0].Column.DisplayIndex == 1)
            {
                foreach (var interes in InteresVariable)
                {
                    if (interesVariableSelect.Contains(interes))
                    {
                        interes.PorcentajeInteres = NumeroACopiar;
                        notifyChange = true;
                    }
                }
            }

            if (notifyChange)
            {
                dgTasaVariable.ItemsSource = null;
                dgTasaVariable.ItemsSource = InteresVariable;
            }
            dgTasaVariable.ContextMenu.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region PrivateMethods
        private bool ValidateRequiredFields()
        {
            bool validationResult = false;

            foreach (InteresFinanciamiento interes in InteresVariable)
            {
                if (interes.PorcentajeInteres <= 0)
                {
                    dgTasaVariable.BorderBrush = Brushes.Red;
                    dgTasaVariable.ToolTip = CAMPOREQUERIDO;
                    validationResult = true;
                    break;
                }
            }

            return validationResult;
        }

        private void LoadDefaultValues()
        {
            InteresVariable = new List<InteresFinanciamiento>();
            for (int i = 0; i < financiamiento.TiempoFinanciamiento; i++)
            {
                InteresVariable.Add(new InteresFinanciamiento() { AnoInteres = financiamiento.AnoInicialPago + i });
            }//for
        }
        #endregion
    }
}
