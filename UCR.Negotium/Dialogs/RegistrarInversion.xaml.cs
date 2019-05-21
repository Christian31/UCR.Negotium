using MahApps.Metro.Controls;
using System;
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
    /// Interaction logic for RegistrarInversionDialog.xaml
    /// </summary>
    public partial class RegistrarInversion : MetroWindow
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";
        private const string CAMPOREQUERIDOPOSITIVO = "Este campo es requerido y debe tener un valor mayor a 0";

        private InversionData requerimientoInversionData;
        private ReinversionData reinversionData;
        private UnidadMedidaData unidadMedidaData;

        private Inversion inversion;
        private List<UnidadMedida> unidadMedidas;
        private ProyectoLite proyecto;

        #endregion

        #region Constructor
        public RegistrarInversion(ProyectoLite proyectoLite, int codInversion = 0)
        {
            InitializeComponent();
            DataContext = this;
            tbDescInversion.ToolTip = "Ingrese en este campo el Nombre de la Inversión que desea registrar";
            tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Inversión que desea registrar";

            this.proyecto = proyectoLite;

            requerimientoInversionData = new InversionData();
            reinversionData = new ReinversionData();
            unidadMedidaData = new UnidadMedidaData();

            inversion = new Inversion();
            unidadMedidas = new List<UnidadMedida>();
            unidadMedidas = unidadMedidaData.GetUnidadesMedidas();
            inversion.UnidadMedida = unidadMedidas.FirstOrDefault();

            if (codInversion != 0)
                inversion = requerimientoInversionData.GetInversion(codInversion);
        }
        #endregion

        #region Properties
        public bool Reload { get; set; }

        public List<AnoDisponible> AnosDisponibles
        {
            get
            {
                List<AnoDisponible> anos = new List<AnoDisponible>();

                if (Inversion.CodInversion.Equals(0))
                {
                    for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                    {
                        int anoActual = proyecto.AnoInicial + i;

                        anos.Add(new AnoDisponible() { Ano = anoActual, IsChecked = false });
                    }//for
                }
                else
                {
                    List<Reinversion> reinversiones = new List<Reinversion>();
                    reinversiones = reinversionData.GetReinversiones(proyecto.CodProyecto).Where(reinv => 
                        reinv.CodInversion.Equals(Inversion.CodInversion)).ToList();

                    for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                    {
                        int anoActual = proyecto.AnoInicial + i;

                        anos.Add(new AnoDisponible() {
                            Ano = anoActual,
                            IsChecked = !reinversiones.Where(reinv => reinv.AnoReinversion.Equals(anoActual)).Count().Equals(0)
                        });
                    }//for
                }

                return anos;
            }
            set
            {
                AnosDisponibles = value;
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

        public Inversion Inversion
        {
            get
            {
                return inversion;
            }
            set
            {
                inversion = value;
            }
        }
        #endregion

        #region Events
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (Inversion.CodInversion.Equals(0))
                {
                    int idInversion = requerimientoInversionData.InsertarInvesion(Inversion, proyecto.CodProyecto);
                    if(!idInversion.Equals(-1))
                    {
                        //success
                        //insertar multiples reinversiones
                        InsertarMultiplesReinversiones(idInversion);
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show(Constantes.INSERTARINVERSIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (requerimientoInversionData.EditarInvesion(Inversion))
                    {
                        //success
                        //insertar multiples reinversiones
                        InsertarMultiplesReinversiones(Inversion.CodInversion);
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show(Constantes.ACTUALIZARINVERSIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
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

        private void tbCostoUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCostoUnitariotxtChngEvent)
            {
                if (tbCostoUnitario.BorderBrush == Brushes.Red)
                {
                    tbCostoUnitario.BorderBrush = Brushes.Gray;
                    tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Inversión que desea registrar";
                }

                tbCostoUnitario.Text = tbCostoUnitario.Text.CheckStringFormat();
            }
            else
            {
                tbCostoUnitariotxtChngEvent = true;
            }
        }

        private void tbDescInversion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDescInversion.BorderBrush == Brushes.Red)
            {
                tbDescInversion.BorderBrush = Brushes.Gray;
                tbDescInversion.ToolTip = "Ingrese en este campo el Nombre de la Inversión que desea registrar";
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
        private void InsertarMultiplesReinversiones(int codInversion)
        {
            List<Reinversion> reinversiones = new List<Reinversion>();
            reinversiones = reinversionData.GetReinversiones(proyecto.CodProyecto).Where(reinv => reinv.CodInversion.Equals(codInversion)).ToList();

            foreach (AnoDisponible anoDisponible in dgAnosDisponibles.ItemsSource)
            {
                if (anoDisponible.IsChecked && reinversiones.Where(reinv => reinv.AnoReinversion.Equals(anoDisponible.Ano)).Count().Equals(0))
                {
                    Reinversion reinversion = new Reinversion()
                    {
                        AnoReinversion = anoDisponible.Ano,
                        Cantidad = Inversion.Cantidad,
                        CodInversion = codInversion,
                        CostoUnitario = Inversion.CostoUnitario,
                        Depreciable = Inversion.Depreciable,
                        Descripcion = Inversion.Descripcion,
                        UnidadMedida = Inversion.UnidadMedida,
                        VidaUtil = Inversion.VidaUtil
                    };
                    
                    reinversionData.InsertarReinversion(reinversion, proyecto.CodProyecto);
                }
                else if (!anoDisponible.IsChecked && !reinversiones.Where(reinv => reinv.AnoReinversion.Equals(anoDisponible.Ano)).Count().Equals(0))
                {
                    reinversiones.Where(reinv => reinv.AnoReinversion.Equals(anoDisponible.Ano)).ToList()
                        .ForEach(reinv => reinversionData.EliminarReinversion(reinv.CodReinversion));
                }
            }
        }

        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbDescInversion.Text))
            {
                tbDescInversion.ToolTip = CAMPOREQUERIDO;
                tbDescInversion.BorderBrush = Brushes.Red;
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

    public class AnoDisponible
    {
        public int Ano { get; set; }
        public bool IsChecked { get; set; }
    }
}
