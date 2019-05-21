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
    public partial class RegistrarCosto : DialogWithDataGrid
    {
        #region PrivateProperties
        private ProyectoLite proyecto;
        private List<UnidadMedida> unidadMedidas;
        private Costo costoSelected;
        private List<VariacionAnualCosto> variacionesPrecio;
        private List<VariacionAnualCosto> variacionesCantidad;

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

            CargarDatosDeCosto(codProyecto, codCosto);
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
            }
        }

        public List<VariacionAnualCosto> VariacionAnualPrecio
        {
            get
            {
                return variacionesPrecio;
            }
            set
            {
                variacionesPrecio = value;
            }
        }

        public List<VariacionAnualCosto> VariacionAnualCantidad
        {
            get
            {
                return variacionesCantidad;
            }
            set
            {
                variacionesCantidad = value;
            }
        }

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

        #region CostoMensual

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

        private void tbNumerosPositivos_TextChanged(object sender, TextChangedEventArgs e)
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

        private void tbNumeros_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dgCostosMensual_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject depObject = (DependencyObject)e.OriginalSource;
            bool mostrarContextMenu = ContextMenuDisponible(depObject, dgCostosMensual.SelectedCells);
            if (mostrarContextMenu)
            {
                dgCostosMensual.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dgCostosMensual.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAgregarValor_Click(object sender, RoutedEventArgs e)
        {
            bool notifyChange = false;

            var selectedCells = dgCostosMensual.SelectedCells;
            var costosMensualesSelected = selectedCells.Select(cell => cell.Item).ToList();
            var costosMensualesSelect = CostoSelected.CostosMensuales.
                Where(costo => costosMensualesSelected.Select(cm => ((CostoMensual)cm).Mes).
                Contains(costo.Mes)).ToList();
            switch (selectedCells[0].Column.DisplayIndex)
            {
                case 1:
                    foreach (var costoMensual in CostoSelected.CostosMensuales)
                    {
                        if (costosMensualesSelect.Contains(costoMensual))
                        {
                            costoMensual.Cantidad = NumeroACopiar;
                            notifyChange = true;
                        }
                    }
                    break;
                case 2:
                    foreach (var costoMensual in CostoSelected.CostosMensuales)
                    {
                        if (costosMensualesSelect.Contains(costoMensual))
                        {
                            costoMensual.CostoUnitario = NumeroACopiar;
                            notifyChange = true;
                        }
                    }
                    break;
            }

            if (notifyChange)
            {
                dgCostosMensual.ItemsSource = null;
                dgCostosMensual.ItemsSource = CostoSelected.CostosMensuales;
            }
            dgCostosMensual.ContextMenu.Visibility = Visibility.Collapsed;
        }
        
        #endregion

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
                CostoSelected.VariacionCostos = GetVariacionesActuales();
                if (CostoSelected.CodCosto.Equals(0))
                {
                    Costo costoTemp = costoData.InsertarCosto(CostoSelected, proyecto.CodProyecto);
                    if (!costoTemp.CodCosto.Equals(0))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show(Constantes.INSERTARCOSTOERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
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
                        MessageBox.Show(Constantes.ACTUALIZARCOSTOERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cbAnosDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadValues();
        }

        #region VariacionPrecio

        private void dgVariacionAnualPrecio_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject depObject = (DependencyObject)e.OriginalSource;
            bool mostrarContextMenu = ContextMenuDisponible(depObject, dgVariacionAnualPrecio.SelectedCells);
            if (mostrarContextMenu)
            {
                dgVariacionAnualPrecio.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dgVariacionAnualPrecio.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAgregarPorcentajePrecio_Click(object sender, RoutedEventArgs e)
        {
            bool notifyChange = false;

            var selectedCells = dgVariacionAnualPrecio.SelectedCells;
            var variacionesAnualesSelected = selectedCells.Select(cell => cell.Item).ToList();
            var variacionesSelect = VariacionAnualPrecio.
                Where(variacion => variacionesAnualesSelected.Select(cm => ((VariacionAnualCosto)cm).Ano).
                Contains(variacion.Ano)).ToList();

            if (selectedCells[0].Column.DisplayIndex == 1)
            {
                foreach (var variaciones in VariacionAnualPrecio)
                {
                    if (variacionesSelect.Contains(variaciones))
                    {
                        variaciones.PorcentajeIncremento = NumeroACopiar;
                        notifyChange = true;
                    }
                }
            }

            if (notifyChange)
            {
                dgVariacionAnualPrecio.ItemsSource = null;
                dgVariacionAnualPrecio.ItemsSource = VariacionAnualPrecio;
            }
            dgVariacionAnualPrecio.ContextMenu.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region VariacionCantidad

        private void dgVariacionAnualCantidad_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject depObject = (DependencyObject)e.OriginalSource;
            bool mostrarContextMenu = ContextMenuDisponible(depObject, dgVariacionAnualCantidad.SelectedCells);
            if (mostrarContextMenu)
            {
                dgVariacionAnualCantidad.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dgVariacionAnualCantidad.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAgregarPorcentajeCantidad_Click(object sender, RoutedEventArgs e)
        {
            bool notifyChange = false;

            var selectedCells = dgVariacionAnualCantidad.SelectedCells;
            var detallesProyeccionSelected = selectedCells.Select(cell => cell.Item).ToList();
            var detalleProyeccionSelect = VariacionAnualCantidad.
                Where(detalle => detallesProyeccionSelected.Select(cm => ((VariacionAnualCosto)cm).Ano).
                Contains(detalle.Ano)).ToList();

            if (selectedCells[0].Column.DisplayIndex == 1)
            {
                foreach (var detalleProyeccion in VariacionAnualCantidad)
                {
                    if (detalleProyeccionSelect.Contains(detalleProyeccion))
                    {
                        detalleProyeccion.PorcentajeIncremento = NumeroACopiar;
                        notifyChange = true;
                    }
                }
            }

            if (notifyChange)
            {
                dgVariacionAnualCantidad.ItemsSource = null;
                dgVariacionAnualCantidad.ItemsSource = VariacionAnualCantidad;
            }
            dgVariacionAnualCantidad.ContextMenu.Visibility = Visibility.Collapsed;
        }

        #endregion

        #endregion

        #region PrivateMethods
        private List<VariacionAnualCostoPorTipo> GetVariacionesActuales()
        {
            List<VariacionAnualCostoPorTipo> variacionesActuales = new List<VariacionAnualCostoPorTipo>();

            VariacionAnualCostoPorTipo variacionPrecio = new VariacionAnualCostoPorTipo();
            variacionPrecio.TipoVariacion = TipoAplicacionPorcentaje.PorPrecio;
            variacionPrecio.VariacionAnual = VariacionAnualPrecio;
            variacionesActuales.Add(variacionPrecio);

            VariacionAnualCostoPorTipo variacionCantidad = new VariacionAnualCostoPorTipo();
            variacionCantidad.TipoVariacion = TipoAplicacionPorcentaje.PorCantidad;
            variacionCantidad.VariacionAnual = VariacionAnualCantidad;
            variacionesActuales.Add(variacionCantidad);

            return variacionesActuales;
        }

        private void CargarDatosDeCosto(int codProyecto, int codCosto)
        {
            costoSelected.AnoCosto = AnosDisponibles.FirstOrDefault();
            costoSelected.CategoriaCosto = Categorias.FirstOrDefault();
            costoSelected.UnidadMedida = UnidadesMedida.FirstOrDefault();

            if (!codCosto.Equals(0))
            {
                costoSelected = costoData.GetCosto(codCosto);
            }

            if (costoSelected.VariacionCostos.Count.Equals(0))
            {
                ReloadValues();
            }
            else
            {
                variacionesPrecio = costoSelected.VariacionCostos.
                    FirstOrDefault(vari => vari.TipoVariacion == TipoAplicacionPorcentaje.PorPrecio).VariacionAnual;

                variacionesCantidad = costoSelected.VariacionCostos.
                    FirstOrDefault(vari => vari.TipoVariacion == TipoAplicacionPorcentaje.PorCantidad).VariacionAnual;
            }
        }

        List<VariacionAnualCosto> variacionesPrecioGuardados = new List<VariacionAnualCosto>();
        List<VariacionAnualCosto> variacionesCantidadGuardados = new List<VariacionAnualCosto>();
        private void ReloadValues()
        {
            if (variacionesPrecioGuardados.Count == 0 && costoSelected.VariacionCostos.Count != 0)
            {
                variacionesPrecioGuardados = costoSelected.VariacionCostos.
                    FirstOrDefault(variacion => variacion.TipoVariacion == TipoAplicacionPorcentaje.PorPrecio).VariacionAnual;
                variacionesCantidadGuardados = costoSelected.VariacionCostos.
                    FirstOrDefault(variacion => variacion.TipoVariacion == TipoAplicacionPorcentaje.PorCantidad).VariacionAnual;
            }
            VariacionAnualCantidad = new List<VariacionAnualCosto>();
            VariacionAnualPrecio = new List<VariacionAnualCosto>();

            int anoInicial = costoSelected.AnoCosto + 1;
            int anoFinal = proyecto.HorizonteEvaluacionEnAnos + proyecto.AnoInicial;
            for (int anoActual = anoInicial; anoActual <= anoFinal; anoActual++)
            {
                VariacionAnualCosto variacionCantidad = variacionesCantidadGuardados.
                    FirstOrDefault(variacion => variacion.Ano == anoActual);
                if (variacionCantidad == null)
                {
                    variacionesCantidad.Add(new VariacionAnualCosto() { Ano = anoActual });
                }
                else
                {
                    variacionesCantidad.Add(new VariacionAnualCosto()
                    {
                        Ano = anoActual,
                        PorcentajeIncremento = variacionCantidad.PorcentajeIncremento
                    });
                }

                VariacionAnualCosto variacionPrecio = variacionesPrecioGuardados.
                    FirstOrDefault(crec => crec.Ano == anoActual);
                if (variacionPrecio == null)
                {
                    variacionesPrecio.Add(new VariacionAnualCosto() { Ano = anoActual });
                }
                else
                {
                    variacionesPrecio.Add(new VariacionAnualCosto()
                    {
                        Ano = anoActual,
                        PorcentajeIncremento = variacionPrecio.PorcentajeIncremento
                    });
                }
            }

            dgVariacionAnualPrecio.ItemsSource = null;
            dgVariacionAnualPrecio.ItemsSource = VariacionAnualPrecio;

            dgVariacionAnualCantidad.ItemsSource = null;
            dgVariacionAnualCantidad.ItemsSource = VariacionAnualCantidad;
        }

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
                if (costoMensual.CostoUnitario > 0 && costoMensual.Cantidad > 0)
                {
                    hasValues = true;
                }
                else if (costoMensual.CostoUnitario > 0)
                {
                    costoMensual.CostoUnitario = 0;
                }
                else if (costoMensual.Cantidad > 0)
                {
                    costoMensual.Cantidad = 0;
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