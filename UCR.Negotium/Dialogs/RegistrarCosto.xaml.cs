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
    /// Interaction logic for RegistrarCostoDialog.xaml
    /// </summary>
    public partial class RegistrarCosto : MetroWindow
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private ProyectoLite proyecto;
        List<UnidadMedida> unidadMedidas;
        private Costo costoSelected;

        private ProyectoData proyectoData;
        private CostoData costoData;
        private UnidadMedidaData unidadMedidaData;
        #endregion

        #region Constructor
        public RegistrarCosto(int codProyecto, int codCosto = 0)
        {
            InitializeComponent();
            DataContext = this;
            tbNombreCosto.ToolTip = "Ingrese en este campo el Nombre del Costo que desea registrar";
            string signo = LocalContext.GetSignoMoneda(codProyecto);
            dgtxcPrecio.Header = string.Format("Precio ({0})", signo);

            proyecto = new ProyectoLite();
            unidadMedidas = new List<UnidadMedida>();
            costoSelected = new Costo();

            proyectoData = new ProyectoData();
            costoData = new CostoData();
            unidadMedidaData = new UnidadMedidaData();

            unidadMedidas = unidadMedidaData.GetUnidadesMedidasParaCostos();
            proyecto = proyectoData.GetProyectoLite(codProyecto);

            //default values
            costoSelected.AnoCosto = AnosDisponibles.FirstOrDefault();
            costoSelected.CategoriaCosto = Categorias.FirstOrDefault();
            costoSelected.UnidadMedida = UnidadesMedida.FirstOrDefault();

            if (!codCosto.Equals(0))
            {
                costoSelected = costoData.GetCosto(codCosto);
            }
        }
        #endregion

        #region Properties
        public bool Reload { get; set; }

        public Costo CostoSelected
        {
            get
            {
                return costoSelected;
            }
            set
            {
                costoSelected = value;
                CostoMensualSelected = costoSelected.CostosMensuales.FirstOrDefault();
            }
        }

        public CostoMensual CostoMensualSelected { get; set; }

        public List<string> Categorias
        {
            get
            {
                return new List<string> { "Operativos", "Administrativos" };
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
                    anos.Add(proyecto.AnoInicial + i);
                }//for

                return anos;
            }
            set
            {
                AnosDisponibles = value;
            }
        }
        #endregion

        #region Events
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
            if (dgCostosMensual.BorderBrush == Brushes.Red)
            {
                dgCostosMensual.BorderBrush = Brushes.Gray;
                dgCostosMensual.ToolTip = string.Empty;
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

        private void tbNombreCosto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNombreCosto.BorderBrush == Brushes.Red)
            {
                tbNombreCosto.BorderBrush = Brushes.Gray;
                tbNombreCosto.ToolTip = "Ingrese en este campo el Nombre del Costo que desea registrar";
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (CostoSelected.CodCosto.Equals(0))
                {
                    Costo costoTemp = costoData.InsertarCosto(CostoSelected, proyecto.CodProyecto);
                    if (!costoTemp.CodCosto.Equals(-1))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar el costo del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (costoData.EditarCosto(CostoSelected))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar el costo del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region PrivateMethods
        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbNombreCosto.Text))
            {
                tbNombreCosto.ToolTip = CAMPOREQUERIDO;
                tbNombreCosto.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            bool hasValues = false;
            foreach (CostoMensual costoMensual in CostoSelected.CostosMensuales)
            {
                if(costoMensual.CostoUnitario > 0 && costoMensual.Cantidad > 0)
                {
                    hasValues = true;
                    break;
                }
            }

            if (!hasValues)
            {
                dgCostosMensual.BorderBrush = Brushes.Red;
                dgCostosMensual.ToolTip = CAMPOREQUERIDO;
                validationResult = true;
            }
            
            return validationResult;
        }
        #endregion
    }
}
