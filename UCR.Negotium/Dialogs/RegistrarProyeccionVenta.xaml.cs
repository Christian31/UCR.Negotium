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
        private List<UnidadMedida> unidadMedidas;
        private ProyeccionVenta proyeccionSelected;
        private List<CrecimientoOferta> crecimientosPrecio;
        private List<CrecimientoOferta> crecimientosCantidad;

        private ProyectoData proyectoData;
        private ProyeccionVentaData proyeccionVentaData;
        private UnidadMedidaData unidadMedidaData;
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

            CargarDatosDeProyeccion(codProyecto, codProyeccion);
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

        public List<CrecimientoOferta> CrecimientoOfertaPrecio
        {
            get
            {
                return crecimientosPrecio;
            }
            set
            {
                crecimientosPrecio = value;
            }
        }

        public List<CrecimientoOferta> CrecimientoOfertaCantidad
        {
            get
            {
                return crecimientosCantidad;
            }
            set
            {
                crecimientosCantidad = value;
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

        #region DetalleProyeccion

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

        #endregion

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                ProyeccionSelected.CrecimientosOferta = GetCrecimientosActuales();
                if (ProyeccionSelected.CodArticulo.Equals(0))
                {
                    ProyeccionVenta proyeccionTemp = proyeccionVentaData.InsertarProyeccionVenta(ProyeccionSelected, proyecto.CodProyecto);
                    if (!proyeccionTemp.CodArticulo.Equals(0))
                    {
                        //success
                        Reload = true;
                        Close();
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
                        //success
                        Reload = true;
                        Close();
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

        private void cbAnosDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadValues();
        }

        #region CrecimientoPrecio

        private void dgDetalleCrecimientoPrecio_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject depObject = (DependencyObject)e.OriginalSource;
            bool mostrarContextMenu = ContextMenuDisponible(depObject, dgDetalleCrecimientoPrecio.SelectedCells);
            if (mostrarContextMenu)
            {
                dgDetalleCrecimientoPrecio.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dgDetalleCrecimientoPrecio.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAgregarPorcentajePrecio_Click(object sender, RoutedEventArgs e)
        {
            bool notifyChange = false;

            var selectedCells = dgDetalleCrecimientoPrecio.SelectedCells;
            var detallesProyeccionSelected = selectedCells.Select(cell => cell.Item).ToList();
            var detalleProyeccionSelect = CrecimientoOfertaPrecio.
                Where(detalle => detallesProyeccionSelected.Select(cm => ((CrecimientoOferta)cm).AnoCrecimiento).
                Contains(detalle.AnoCrecimiento)).ToList();

            if (selectedCells[0].Column.DisplayIndex == 1)
            {
                foreach (var detalleProyeccion in CrecimientoOfertaPrecio)
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
                dgDetalleCrecimientoPrecio.ItemsSource = null;
                dgDetalleCrecimientoPrecio.ItemsSource = CrecimientoOfertaPrecio;
            }
            dgDetalleCrecimientoPrecio.ContextMenu.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region CrecimientoCantidad
        private void dgDetalleCrecimientoCantidad_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject depObject = (DependencyObject)e.OriginalSource;
            bool mostrarContextMenu = ContextMenuDisponible(depObject, dgDetalleCrecimientoCantidad.SelectedCells);
            if (mostrarContextMenu)
            {
                dgDetalleCrecimientoCantidad.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dgDetalleCrecimientoCantidad.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAgregarPorcentajeCantidad_Click(object sender, RoutedEventArgs e)
        {
            bool notifyChange = false;

            var selectedCells = dgDetalleCrecimientoCantidad.SelectedCells;
            var detallesProyeccionSelected = selectedCells.Select(cell => cell.Item).ToList();
            var detalleProyeccionSelect = CrecimientoOfertaCantidad.
                Where(detalle => detallesProyeccionSelected.Select(cm => ((CrecimientoOferta)cm).AnoCrecimiento).
                Contains(detalle.AnoCrecimiento)).ToList();

            if (selectedCells[0].Column.DisplayIndex == 1)
            {
                foreach (var detalleProyeccion in CrecimientoOfertaCantidad)
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
                dgDetalleCrecimientoCantidad.ItemsSource = null;
                dgDetalleCrecimientoCantidad.ItemsSource = CrecimientoOfertaCantidad;
            }
            dgDetalleCrecimientoCantidad.ContextMenu.Visibility = Visibility.Collapsed;
        }
        #endregion

        #endregion

        #region PrivateMethods

        private List<CrecimientoOfertaPorTipo> GetCrecimientosActuales()
        {
            List<CrecimientoOfertaPorTipo> crecimientosActuales = new List<CrecimientoOfertaPorTipo>();

            CrecimientoOfertaPorTipo crecimientoPrecio = new CrecimientoOfertaPorTipo();
            crecimientoPrecio.TipoCrecimiento = TipoAplicacionPorcentaje.PorPrecio;
            crecimientoPrecio.CrecimientoOferta = CrecimientoOfertaPrecio;
            crecimientosActuales.Add(crecimientoPrecio);

            CrecimientoOfertaPorTipo crecimientoCantidad = new CrecimientoOfertaPorTipo();
            crecimientoCantidad.TipoCrecimiento = TipoAplicacionPorcentaje.PorCantidad;
            crecimientoCantidad.CrecimientoOferta = CrecimientoOfertaCantidad;
            crecimientosActuales.Add(crecimientoCantidad);

            return crecimientosActuales;
        }

        private void CargarDatosDeProyeccion(int codProyecto, int codProyeccion)
        {
            unidadMedidas = unidadMedidaData.GetUnidadesMedidas();
            proyecto = proyectoData.GetProyectoLite(codProyecto);

            //default values
            proyeccionSelected.UnidadMedida = UnidadesMedida.FirstOrDefault();
            proyeccionSelected.AnoArticulo = AnosDisponibles.FirstOrDefault();

            if (!codProyeccion.Equals(0))
            {
                proyeccionSelected = proyeccionVentaData.GetProyeccionVenta(codProyeccion);
            }

            if (proyeccionSelected.CrecimientosOferta.Count.Equals(0))
            {
                ReloadValues();
            }
            else
            {
                crecimientosPrecio = proyeccionSelected.CrecimientosOferta.
                    FirstOrDefault(crec => crec.TipoCrecimiento == TipoAplicacionPorcentaje.PorPrecio).CrecimientoOferta;

                crecimientosCantidad = proyeccionSelected.CrecimientosOferta.
                    FirstOrDefault(crec => crec.TipoCrecimiento == TipoAplicacionPorcentaje.PorCantidad).CrecimientoOferta;
            }
        }

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
                else if (detalleVenta.Precio > 0)
                {
                    detalleVenta.Precio = 0;
                }
                else if (detalleVenta.Cantidad > 0)
                {
                    detalleVenta.Cantidad = 0;
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

        List<CrecimientoOferta> crecimientosPrecioGuardados = new List<CrecimientoOferta>();
        List<CrecimientoOferta> crecimientosCantidadGuardados = new List<CrecimientoOferta>();
        private void ReloadValues()
        {
            if(crecimientosPrecioGuardados.Count == 0 && proyeccionSelected.CrecimientosOferta.Count != 0)
            {
                crecimientosPrecioGuardados = proyeccionSelected.CrecimientosOferta.
                    FirstOrDefault(crec => crec.TipoCrecimiento == TipoAplicacionPorcentaje.PorPrecio).CrecimientoOferta;
                crecimientosCantidadGuardados = proyeccionSelected.CrecimientosOferta.
                    FirstOrDefault(crec => crec.TipoCrecimiento == TipoAplicacionPorcentaje.PorCantidad).CrecimientoOferta;
            }
            CrecimientoOfertaCantidad = new List<CrecimientoOferta>();
            CrecimientoOfertaPrecio = new List<CrecimientoOferta>();

            int anoInicial = proyeccionSelected.AnoArticulo + 1;
            int anoFinal = proyecto.HorizonteEvaluacionEnAnos + proyecto.AnoInicial;
            for (int anoActual = anoInicial; anoActual <= anoFinal; anoActual++)
            {
                CrecimientoOferta crecimientoCantidad = crecimientosCantidadGuardados.
                    FirstOrDefault(crec => crec.AnoCrecimiento == anoActual);
                if(crecimientoCantidad == null)
                {
                    crecimientosCantidad.Add(new CrecimientoOferta() { AnoCrecimiento = anoActual });
                }
                else
                {
                    crecimientosCantidad.Add(new CrecimientoOferta() { AnoCrecimiento = anoActual,
                        PorcentajeCrecimiento = crecimientoCantidad.PorcentajeCrecimiento });
                }

                CrecimientoOferta crecimientoPrecio = crecimientosPrecioGuardados.
                    FirstOrDefault(crec => crec.AnoCrecimiento == anoActual);
                if (crecimientoPrecio == null)
                {
                    crecimientosPrecio.Add(new CrecimientoOferta() { AnoCrecimiento = anoActual });
                }
                else
                {
                    crecimientosPrecio.Add(new CrecimientoOferta() { AnoCrecimiento = anoActual,
                        PorcentajeCrecimiento = crecimientoPrecio.PorcentajeCrecimiento });
                }
            }

            dgDetalleCrecimientoPrecio.ItemsSource = null;
            dgDetalleCrecimientoPrecio.ItemsSource = CrecimientoOfertaPrecio;

            dgDetalleCrecimientoCantidad.ItemsSource = null;
            dgDetalleCrecimientoCantidad.ItemsSource = CrecimientoOfertaCantidad;
        }

        #endregion        
    }
}
