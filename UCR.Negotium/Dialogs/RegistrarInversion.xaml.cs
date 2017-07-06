using MahApps.Metro.Controls;
using System;
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
    /// Interaction logic for RegistrarInversionDialog.xaml
    /// </summary>
    public partial class RegistrarInversion : MetroWindow
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";
        private const string CAMPOREQUERIDOPOSITIVO = "Este campo es requerido y debe tener un valor mayor a 0";

        private RequerimientoInversionData requerimientoInversionData;
        RequerimientoReinversionData reinversionData;
        private ProyectoData proyectoData = new ProyectoData();
        private UnidadMedidaData unidadMedidaData;
        private RequerimientoInversion inversion;
        private List<UnidadMedida> unidadMedidas;
        private int codProyecto;
        private Proyecto proyecto;

        #endregion

        #region Constructor
        public RegistrarInversion(int codProyecto, int codInversion = 0)
        {
            InitializeComponent();
            DataContext = this;
            tbDescInversion.ToolTip = "Ingrese en este campo el Nombre de la Inversión que desea registrar";
            tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Inversión que desea registrar";

            this.codProyecto = codProyecto;

            requerimientoInversionData = new RequerimientoInversionData();
            reinversionData = new RequerimientoReinversionData();
            unidadMedidaData = new UnidadMedidaData();

            proyecto = new Proyecto();
            inversion = new RequerimientoInversion();
            unidadMedidas = new List<UnidadMedida>();
            unidadMedidas = unidadMedidaData.GetUnidadesMedidaAux();
            inversion.UnidadMedida = unidadMedidas.FirstOrDefault();

            if (codInversion != 0)
                inversion = requerimientoInversionData.GetRequerimientoInversion(codInversion);

            proyecto = proyectoData.GetProyecto(codProyecto);
        }
        #endregion

        #region Properties
        public bool Reload { get; set; }

        public List<AnoDisponible> AnosDisponibles
        {
            get
            {
                List<AnoDisponible> anos = new List<AnoDisponible>();

                if (Inversion.CodRequerimientoInversion.Equals(0))
                {
                    for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                    {
                        int anoActual = proyecto.AnoInicial + i;

                        anos.Add(new AnoDisponible() { Ano = anoActual, IsChecked = false });
                    }//for
                }
                else
                {
                    List<RequerimientoReinversion> reinversiones = new List<RequerimientoReinversion>();
                    reinversiones = reinversionData.GetRequerimientosReinversion(codProyecto).Where(reinv => 
                        reinv.CodRequerimientoInversion.Equals(Inversion.CodRequerimientoInversion)).ToList();

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

        public RequerimientoInversion Inversion
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
                if (!Inversion.Depreciable)
                    Inversion.VidaUtil = 0;

                if (Inversion.CodRequerimientoInversion.Equals(0))
                {
                    int idInversion = requerimientoInversionData.InsertarRequerimientosInvesion(Inversion, codProyecto);
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
                        MessageBox.Show("Ha ocurrido un error al insertar la inversión del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (requerimientoInversionData.EditarRequerimientosInvesion(Inversion))
                    {
                        //success
                        //insertar multiples reinversiones
                        InsertarMultiplesReinversiones(Inversion.CodRequerimientoInversion);
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar la inversión del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void tbCostoUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCostoUnitario.BorderBrush == Brushes.Red)
            {
                tbCostoUnitario.BorderBrush = Brushes.Gray;
                tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Inversión que desea registrar";
            }
            int costoUnitario = 0;
            if(!int.TryParse(tbCostoUnitario.Text, out costoUnitario))
            {
                tbCostoUnitario.Text = string.Empty;
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

        private void cbDepreciable_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbDepreciable.IsChecked.Value.Equals(false))
            {
                cbNoDepreciable.IsChecked = true;
                nudVidaUtil.IsEnabled = false;
            }
        }

        private void cbDepreciable_Checked(object sender, RoutedEventArgs e)
        {
            cbNoDepreciable.IsChecked = false;
            nudVidaUtil.IsEnabled = true;
        }

        private void cbNoDepreciable_Checked(object sender, RoutedEventArgs e)
        {
            cbDepreciable.IsChecked = nudVidaUtil.IsEnabled = false;
        }
        #endregion

        #region PrivateMethods
        private void InsertarMultiplesReinversiones(int codInversion)
        {
            List<RequerimientoReinversion> reinversiones = new List<RequerimientoReinversion>();
            reinversiones = reinversionData.GetRequerimientosReinversion(codProyecto).Where(reinv => reinv.CodRequerimientoInversion.Equals(codInversion)).ToList();

            foreach (AnoDisponible anoDisponible in dgAnosDisponibles.ItemsSource)
            {
                if (anoDisponible.IsChecked && reinversiones.Where(reinv => reinv.AnoReinversion.Equals(anoDisponible.Ano)).Count().Equals(0))
                {
                    RequerimientoReinversion reinversion = new RequerimientoReinversion()
                    {
                        AnoReinversion = anoDisponible.Ano,
                        Cantidad = Inversion.Cantidad,
                        CodRequerimientoInversion = codInversion,
                        CostoUnitario = Inversion.CostoUnitario,
                        Depreciable = Inversion.Depreciable,
                        DescripcionRequerimiento = Inversion.DescripcionRequerimiento,
                        UnidadMedida = Inversion.UnidadMedida,
                        VidaUtil = Inversion.VidaUtil
                    };
                    
                    reinversionData.InsertarRequerimientosReinversion(reinversion, codProyecto);
                }
                else if (!anoDisponible.IsChecked && !reinversiones.Where(reinv => reinv.AnoReinversion.Equals(anoDisponible.Ano)).Count().Equals(0))
                {
                    reinversiones.Where(reinv => reinv.AnoReinversion.Equals(anoDisponible.Ano)).ToList()
                        .ForEach(reinv => reinversionData.EliminarRequerimientoReinversion(reinv.CodRequerimientoReinversion));
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
            if (Convert.ToInt64(tbCostoUnitario.Text.ToString()) <= 0)
            {
                tbCostoUnitario.ToolTip = CAMPOREQUERIDOPOSITIVO;
                tbCostoUnitario.BorderBrush = Brushes.Red;
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
