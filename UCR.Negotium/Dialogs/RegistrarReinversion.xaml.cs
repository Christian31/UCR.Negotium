using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for RegistrarReinversionDialog.xaml
    /// </summary>
    public partial class RegistrarReinversion : MetroWindow, INotifyPropertyChanged
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";
        private const string CAMPOREQUERIDOPOSITIVO = "Este campo es requerido y debe tener un valor mayor a 0";

        private ReinversionData reinversionData;
        private InversionData inversionData;
        private UnidadMedidaData unidadMedidaData;

        private Reinversion reinversion;
        private List<UnidadMedida> unidadMedidas;
        private ProyectoLite proyecto;
        private List<Inversion> inversiones;

        private bool vincularInversion;
        private string descReinv;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region Constructor
        public RegistrarReinversion(ProyectoLite proyectoLite, int codReinversion = 0)
        {
            InitializeComponent();
            DataContext = this;
            tbDescReinversion.ToolTip = "Ingrese en este campo el Nombre de la Reinversión que desea registrar";
            tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Reinversión que desea registrar";

            this.proyecto = proyectoLite;

            reinversionData = new ReinversionData();
            inversionData = new InversionData();
            unidadMedidaData = new UnidadMedidaData();

            reinversion = new Reinversion();
            unidadMedidas = new List<UnidadMedida>();
            inversiones = new List<Inversion>();

            unidadMedidas = unidadMedidaData.GetUnidadesMedidas();
            inversiones = inversionData.GetInversiones(proyecto.CodProyecto).Where(inv => inv.Depreciable).ToList();
            
            reinversion.UnidadMedida = unidadMedidas.FirstOrDefault();
            reinversion.AnoReinversion = AnosDisponibles.FirstOrDefault();

            if (codReinversion != 0)
            {
                reinversion = reinversionData.GetReinversion(codReinversion);
                descReinv = reinversion.Descripcion;
                vincularInversion = !reinversion.CodInversion.Equals(0);
            }
        }
        #endregion

        #region Properties
        public bool VincularInversion
        {
            get
            {
                return vincularInversion;
            }
            set
            {
                vincularInversion = value;
                PropertyChanged(this, new PropertyChangedEventArgs("VincularInversion"));
            }
        }

        public bool Reload { get; set; }

        public List<Inversion> Inversiones
        {
            get
            {
                return inversiones;
            }
            set
            {
                inversiones = value;
            }
        }

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

        public Reinversion Reinversion
        {
            get
            {
                return reinversion;
            }
            set
            {
                reinversion = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Reinversion"));
            }
        }
        #endregion

        #region Events
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (!VincularInversion)
                    Reinversion.CodInversion = 0;

                if (Reinversion.CodReinversion.Equals(0))
                {
                    int idInversion = reinversionData.InsertarReinversion(Reinversion, proyecto.CodProyecto);
                    if (!idInversion.Equals(-1))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show(Constantes.INSERTARREINVERSIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (reinversionData.EditarReinversion(Reinversion))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show(Constantes.ACTUALIZARREINVERSIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void tbCostoUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCostoUnitariotxtChngEvent)
            {
                if (tbCostoUnitario.BorderBrush == Brushes.Red)
                {
                    tbCostoUnitario.BorderBrush = Brushes.Gray;
                    tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Reinversión que desea registrar";
                }

                tbCostoUnitario.Text = tbCostoUnitario.Text.CheckStringFormat();
            }
            else
            {
                tbCostoUnitariotxtChngEvent = true;
            }
        }
        
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbDescReinversion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDescReinversion.BorderBrush == Brushes.Red)
            {
                tbDescReinversion.BorderBrush = Brushes.Gray;
                tbDescReinversion.ToolTip = "Ingrese en este campo el Nombre de la Reinversión que desea registrar";
            }
        }

        private void cbxInversiones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxInversiones.SelectedIndex >= 0 && Reinversion.CodInversion == 0)
            {
                Inversion inversion = (Inversion)cbxInversiones.SelectedItem;
                tbDescReinversion.Text = inversion.Descripcion;
                Reinversion.CodInversion = inversion.CodInversion;
            }
        }

        private void cbInversion_Checked(object sender, RoutedEventArgs e)
        {
            cbxInversiones.SelectedIndex = 0;
            PropertyChanged(this, new PropertyChangedEventArgs("Reinversion"));
        }

        private void cbNoInversion_Checked(object sender, RoutedEventArgs e)
        {
            Reinversion.CodInversion = 0;
            Reinversion.Descripcion = descReinv;
            PropertyChanged(this, new PropertyChangedEventArgs("Reinversion"));
        }

        bool tbCostoUnitariotxtChngEvent = true;
        private void tbCostoUnitario_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbCostoUnitario.Text.Equals("0") || tbCostoUnitario.Text.Equals("0.00"))
            {
                tbCostoUnitariotxtChngEvent = false;
                tbCostoUnitario.Text = "";
            }
        }

        private void tbCostoUnitario_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbCostoUnitario.Text.Equals(""))
            {
                tbCostoUnitariotxtChngEvent = false;
                tbCostoUnitario.Text = "0.00";
            }
        }

        bool tbCantidadtxtChngEvent = true;
        private void tbCantidad_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbCantidad.Text.Equals("0") || tbCantidad.Text.Equals("0.00"))
            {
                tbCantidadtxtChngEvent = false;
                tbCantidad.Text = "";
            }
        }

        private void tbCantidad_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbCantidad.Text.Equals(""))
            {
                tbCantidadtxtChngEvent = false;
                tbCantidad.Text = "0.00";
            }
        }

        private void tbCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCantidadtxtChngEvent)
            {
                if (tbCantidad.BorderBrush == Brushes.Red)
                {
                    tbCantidad.BorderBrush = Brushes.Gray;
                    tbCantidad.ToolTip = "Ingrese en este campo Cantidad de la Inversión que desea registrar";
                }

                tbCantidad.Text = tbCantidad.Text.CheckStringFormat();
            }
            else
            {
                tbCantidadtxtChngEvent = true;
            }
        }

        #endregion

        #region PrivateMethods
        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbDescReinversion.Text))
            {
                tbDescReinversion.ToolTip = CAMPOREQUERIDO;
                tbDescReinversion.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (Convert.ToDouble(tbCostoUnitario.Text.ToString()) <= 0)
            {
                tbCostoUnitario.ToolTip = CAMPOREQUERIDOPOSITIVO;
                tbCostoUnitario.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (Convert.ToDouble(tbCantidad.Text.ToString()) <= 0)
            {
                tbCantidad.ToolTip = CAMPOREQUERIDOPOSITIVO;
                tbCantidad.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            return validationResult;
        }
        #endregion
    }
}
