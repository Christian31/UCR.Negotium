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
    /// Interaction logic for RegistrarProyeccionVentaDialog.xaml
    /// </summary>
    public partial class RegistrarProyeccionVenta : MetroWindow
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private ProyectoLite proyecto;
        List<UnidadMedida> unidadMedidas;
        private ProyeccionVentaArticulo proyeccionSelected;

        private ProyectoData proyectoData;
        private ProyeccionVentaArticuloData proyeccionArticuloData;
        private UnidadMedidaData unidadMedidaData;
        private CrecimientoOfertaArticuloData crecimientoOfertaData;
        #endregion

        #region Constructor
        public RegistrarProyeccionVenta(int codProyecto, int codProyeccion = 0)
        {
            InitializeComponent();
            DataContext = this;
            tbNombreArticulo.ToolTip = "Ingrese en este campo el Nombre de la Proyección del Producto que desea registrar";
            string signo = LocalContext.GetSignoMoneda(codProyecto);
            dgtxcPrecio.Header = string.Format("Precio ({0})", signo);

            proyecto = new ProyectoLite();
            unidadMedidas = new List<UnidadMedida>();
            proyeccionSelected = new ProyeccionVentaArticulo();

            proyectoData = new ProyectoData();
            proyeccionArticuloData = new ProyeccionVentaArticuloData();
            unidadMedidaData = new UnidadMedidaData();
            crecimientoOfertaData = new CrecimientoOfertaArticuloData();

            unidadMedidas = unidadMedidaData.GetUnidadesMedidas();
            proyecto = proyectoData.GetProyectoLite(codProyecto);

            //default values
            proyeccionSelected.UnidadMedida = UnidadesMedida.FirstOrDefault();

            if (!codProyeccion.Equals(0))
            {
                proyeccionSelected = proyeccionArticuloData.GetProyeccionVentaArticulo(codProyeccion);
                proyeccionSelected.CrecimientoOferta = crecimientoOfertaData.GetCrecimientoOfertaObjetoIntereses(codProyeccion);
            }

            if (proyeccionSelected.CrecimientoOferta.Count.Equals(0))
            {
                LoadDefaultValues();
            }

        }
        #endregion

        #region Properties
        public bool Reload { get; set; }

        public ProyeccionVentaArticulo ProyeccionSelected
        {
            get
            {
                return proyeccionSelected;
            }
            set
            {
                proyeccionSelected = value;
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
        #endregion

        #region Events

        bool tbCostoUnitarioChngEvent = true;
        private void tbNumerosPositivos_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox val = (TextBox)sender;
            if (val.Text.Equals("0") || val.Text.Equals("0.00"))
            {
                tbCostoUnitarioChngEvent = false;
                val.Text = "";
            }
        }

        private void tbNumerosPositivos_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox val = (TextBox)sender;
            if (val.Text.Equals(""))
            {
                tbCostoUnitarioChngEvent = false;
                val.Text = "0.00";
            }
        }

        //check validation
        private void tbNumerosPositivos_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCostoUnitarioChngEvent)
            { 
                if (dgDetalleProyeccion.BorderBrush == Brushes.Red)
                {
                    dgDetalleProyeccion.BorderBrush = Brushes.Gray;
                    dgDetalleProyeccion.ToolTip = string.Empty;
                }

                TextBox val = (TextBox)sender;
                val.Text = val.Text.CheckStringFormat();
            }
            else
            {
                tbCostoUnitarioChngEvent = true;
            }
        }

        private void tbNumeros_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCostoUnitarioChngEvent)
            {
                TextBox val = (TextBox)sender;
                val.Text = val.Text.CheckStringFormat();
            }
            else
            {
                tbCostoUnitarioChngEvent = true;
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (ProyeccionSelected.CodArticulo.Equals(0))
                {
                    ProyeccionVentaArticulo proyeccionTemp = proyeccionArticuloData.InsertarProyeccionVenta(ProyeccionSelected, proyecto.CodProyecto);
                    if (!proyeccionTemp.CodArticulo.Equals(-1))
                    {
                        bool hasErrors = false;
                        foreach(CrecimientoOfertaArticulo crecimiento in proyeccionTemp.CrecimientoOferta)
                        {
                            if (crecimientoOfertaData.InsertarCrecimientoOfertaObjetoIntereses(crecimiento, proyeccionTemp.CodArticulo).CodCrecimiento.Equals(0))
                            {
                                hasErrors = true;
                                break;
                            }
                        }
                        if (!hasErrors)
                        {
                            //success
                            Reload = true;
                            Close();
                        }
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar la proyección del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (proyeccionArticuloData.EditarProyeccionVenta(ProyeccionSelected))
                    {
                        bool hasErrors = false;
                        foreach (CrecimientoOfertaArticulo crecimiento in ProyeccionSelected.CrecimientoOferta)
                        {
                            if (!crecimientoOfertaData.EditarCrecimientoOfertaObjetoIntereses(crecimiento))
                            {
                                hasErrors = true;
                                break;
                            }
                        }
                        if (!hasErrors)
                        {
                            //success
                            Reload = true;
                            Close();
                        }
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar la proyección del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbNombreArticulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNombreArticulo.BorderBrush == Brushes.Red)
            {
                tbNombreArticulo.BorderBrush = Brushes.Gray;
                tbNombreArticulo.ToolTip = "Ingrese en este campo el Nombre de la Proyección del Producto que desea registrar";
            }
        }
        #endregion

        #region PrivateMethods
        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbNombreArticulo.Text))
            {
                tbNombreArticulo.ToolTip = CAMPOREQUERIDO;
                tbNombreArticulo.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            bool hasValues = false;
            foreach (DetalleProyeccionVenta detalleVenta in ProyeccionSelected.DetallesProyeccionVenta)
            {
                if (detalleVenta.Precio > 0 && detalleVenta.Cantidad > 0)
                {
                    hasValues = true;
                    break;
                }
            }

            if (!hasValues)
            {
                dgDetalleProyeccion.BorderBrush = Brushes.Red;
                dgDetalleProyeccion.ToolTip = CAMPOREQUERIDO;
                validationResult = true;
            }

            return validationResult;
        }

        private void LoadDefaultValues()
        {
            for (int i = 2; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                int anoActual = proyecto.AnoInicial + i;
                proyeccionSelected.CrecimientoOferta.Add(new CrecimientoOfertaArticulo() { AnoCrecimiento = anoActual });
            }//for
        }
        #endregion
    }
}
