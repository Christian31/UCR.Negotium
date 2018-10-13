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
    public partial class RegistrarProyeccionVenta : DialogWithDataGrid
    {
        #region PrivateProperties
        private ProyectoLite proyecto;
        List<UnidadMedida> unidadMedidas;
        private ProyeccionVenta proyeccionSelected;

        private ProyectoData proyectoData;
        private ProyeccionVentaData proyeccionVentaData;
        private UnidadMedidaData unidadMedidaData;
        private CrecimientoOfertaData crecimientoOfertaData;
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
            proyeccionSelected = new ProyeccionVenta();

            proyectoData = new ProyectoData();
            proyeccionVentaData = new ProyeccionVentaData();
            unidadMedidaData = new UnidadMedidaData();
            crecimientoOfertaData = new CrecimientoOfertaData();

            unidadMedidas = unidadMedidaData.GetUnidadesMedidas();
            proyecto = proyectoData.GetProyectoLite(codProyecto);

            //default values
            proyeccionSelected.UnidadMedida = UnidadesMedida.FirstOrDefault();
            proyeccionSelected.AnoArticulo = AnosDisponibles.FirstOrDefault();

            if (!codProyeccion.Equals(0))
            {
                proyeccionSelected = proyeccionVentaData.GetProyeccionVenta(codProyeccion);
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

        public ProyeccionVenta ProyeccionSelected
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

        public List<int> AnosDisponibles
        {
            get
            {
                List<int> anos = new List<int>();
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    anos.Add(proyecto.AnoInicial + i);
                }

                return anos;
            }
            set
            {
                AnosDisponibles = value;
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
                    ProyeccionVenta proyeccionTemp = proyeccionVentaData.InsertarProyeccionVenta(ProyeccionSelected, proyecto.CodProyecto);
                    if (!proyeccionTemp.CodArticulo.Equals(-1))
                    {
                        bool hasErrors = false;
                        foreach(CrecimientoOferta crecimiento in proyeccionTemp.CrecimientoOferta)
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
                        MessageBox.Show(Constantes.INSERTARPROYECCIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (proyeccionVentaData.EditarProyeccionVenta(ProyeccionSelected))
                    {
                        bool hasErrors = false;
                        foreach (CrecimientoOferta crecimiento in ProyeccionSelected.CrecimientoOferta)
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
                        MessageBox.Show(Constantes.ACTUALIZARPROYECCIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnAgregarValor_Click(object sender, RoutedEventArgs e)
        {
            bool notifyChange = false;

            var selectedCells = dgDetalleProyeccion.SelectedCells;
            var detallesProyeccionSelected = selectedCells.Select(cell => cell.Item).ToList();
            var detalleProyeccionSelect = ProyeccionSelected.DetallesProyeccionVenta.
                Where(detalle => detallesProyeccionSelected.Select(cm => ((DetalleProyeccionVenta)cm).Mes).
                Contains(detalle.Mes)).ToList();
            switch (selectedCells[0].Column.DisplayIndex)
            {
                case 1:
                    foreach (var detalleProyeccion in ProyeccionSelected.DetallesProyeccionVenta)
                    {
                        if (detalleProyeccionSelect.Contains(detalleProyeccion))
                        {
                            detalleProyeccion.Cantidad = NumeroACopiar;
                            notifyChange = true;
                        }
                    }
                    break;
                case 2:
                    foreach (var detalleProyeccion in ProyeccionSelected.DetallesProyeccionVenta)
                    {
                        if (detalleProyeccionSelect.Contains(detalleProyeccion))
                        {
                            detalleProyeccion.Precio = NumeroACopiar;
                            notifyChange = true;
                        }
                    }
                    break;
            }

            if (notifyChange)
            {
                dgDetalleProyeccion.ItemsSource = null;
                dgDetalleProyeccion.ItemsSource = ProyeccionSelected.DetallesProyeccionVenta;
            }
            dgDetalleProyeccion.ContextMenu.Visibility = Visibility.Collapsed;
        }

        private void dgDetalleProyeccion_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject depObject = (DependencyObject)e.OriginalSource;
            bool mostrarContextMenu = ContextMenuDisponible(depObject, dgDetalleProyeccion.SelectedCells);
            if (mostrarContextMenu)
            {
                dgDetalleProyeccion.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dgDetalleProyeccion.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void dgDetalleCrecimiento_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject depObject = (DependencyObject)e.OriginalSource;
            bool mostrarContextMenu = ContextMenuDisponible(depObject, dgDetalleCrecimiento.SelectedCells);
            if (mostrarContextMenu)
            {
                dgDetalleCrecimiento.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dgDetalleCrecimiento.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAgregarPorcentaje_Click(object sender, RoutedEventArgs e)
        {
            bool notifyChange = false;

            var selectedCells = dgDetalleCrecimiento.SelectedCells;
            var detallesProyeccionSelected = selectedCells.Select(cell => cell.Item).ToList();
            var detalleProyeccionSelect = ProyeccionSelected.CrecimientoOferta.
                Where(detalle => detallesProyeccionSelected.Select(cm => ((CrecimientoOferta)cm).AnoCrecimiento).
                Contains(detalle.AnoCrecimiento)).ToList();

            if (selectedCells[0].Column.DisplayIndex == 1)
            {
                foreach (var detalleProyeccion in ProyeccionSelected.CrecimientoOferta)
                {
                    if (detalleProyeccionSelect.Contains(detalleProyeccion))
                    {
                        detalleProyeccion.PorcentajeCrecimiento = NumeroACopiar;
                        notifyChange = true;
                    }
                }
            }

            if (notifyChange)
            {
                dgDetalleCrecimiento.ItemsSource = null;
                dgDetalleCrecimiento.ItemsSource = ProyeccionSelected.CrecimientoOferta;
            }
            dgDetalleCrecimiento.ContextMenu.Visibility = Visibility.Collapsed;
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
                proyeccionSelected.CrecimientoOferta.Add(new CrecimientoOferta() { AnoCrecimiento = anoActual });
            }
        }
        #endregion
    }
}
