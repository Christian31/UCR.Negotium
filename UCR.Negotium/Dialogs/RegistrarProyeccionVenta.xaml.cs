using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarProyeccionVentaDialog.xaml
    /// </summary>
    public partial class RegistrarProyeccionVenta : MetroWindow
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private Proyecto proyecto;
        List<UnidadMedida> unidadMedidas;
        private ProyeccionVentaArticulo proyeccionSelected;

        private ProyectoData proyectoData;
        private ProyeccionVentaArticuloData proyeccionArticuloData;
        private UnidadMedidaData unidadMedidaData;
        private CrecimientoOfertaObjetoInteresData crecimientoOfertaData;

        public RegistrarProyeccionVenta(int idProyecto, int idProyeccion = 0)
        {
            InitializeComponent();
            DataContext = this;
            tbNombreArticulo.ToolTip = "Ingrese en este campo el Nombre de la Proyección del Producto que desea registrar";
            lbCrecimientoOferta.AppendText("Ingrese los porcentajes de Crecimiento anual de Oferta del Artículo");

            proyecto = new Proyecto();
            unidadMedidas = new List<UnidadMedida>();
            proyeccionSelected = new ProyeccionVentaArticulo();

            proyectoData = new ProyectoData();
            proyeccionArticuloData = new ProyeccionVentaArticuloData();
            unidadMedidaData = new UnidadMedidaData();
            crecimientoOfertaData = new CrecimientoOfertaObjetoInteresData();

            unidadMedidas = unidadMedidaData.GetUnidadesMedidaAux();
            proyecto = proyectoData.GetProyecto(idProyecto);

            //default values
            proyeccionSelected.UnidadMedida = UnidadesMedida.FirstOrDefault();

            if (!idProyeccion.Equals(0))
            {
                proyeccionSelected = proyeccionArticuloData.GetProyeccionVentaArticulo(idProyeccion);
                proyeccionSelected.CrecimientoOferta = crecimientoOfertaData.GetCrecimientoOfertaObjetoIntereses(idProyeccion);
            }

            if (proyeccionSelected.CrecimientoOferta.Count.Equals(0))
            {
                LoadDefaultValues();
            }

        }

        private void LoadDefaultValues()
        {
            for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                int anoActual = proyecto.AnoInicial + i;
                proyeccionSelected.CrecimientoOferta.Add(new CrecimientoOfertaObjetoInteres() { AnoCrecimiento = anoActual });
            }//for
        }

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

        //check validation
        private void tbDatosPositivos_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox val = (TextBox)sender;
            if (!val.Text.Equals(string.Empty) && !ValidaNumeros(val.Text))
            {
                val.Text = 0.ToString();
            }
            else if (dgDetalleProyeccion.BorderBrush == Brushes.Red)
            {
                dgDetalleProyeccion.BorderBrush = Brushes.Gray;
                dgDetalleProyeccion.ToolTip = string.Empty;
            }
            else if (dgDetalleCrecimiento.BorderBrush == Brushes.Red)
            {
                dgDetalleCrecimiento.BorderBrush = Brushes.Gray;
                dgDetalleCrecimiento.ToolTip = string.Empty;
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
                        foreach(CrecimientoOfertaObjetoInteres crecimiento in proyeccionTemp.CrecimientoOferta)
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
                        foreach (CrecimientoOfertaObjetoInteres crecimiento in ProyeccionSelected.CrecimientoOferta)
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

            foreach (CrecimientoOfertaObjetoInteres detalleCrecimiento in ProyeccionSelected.CrecimientoOferta)
            {
                if (detalleCrecimiento.PorcentajeCrecimiento <= 0)
                {
                    dgDetalleCrecimiento.BorderBrush = Brushes.Red;
                    dgDetalleCrecimiento.ToolTip = CAMPOREQUERIDO;
                    validationResult = true;
                    break;
                }
            }

            return validationResult;
        }

        private bool ValidaNumeros(string valor)
        {
            double n;
            if (double.TryParse(valor, out n))
            {
                if (n >= 0)
                    return true;
            }

            return false;
        }

        private void tbNombreArticulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNombreArticulo.BorderBrush == Brushes.Red)
            {
                tbNombreArticulo.BorderBrush = Brushes.Gray;
                tbNombreArticulo.ToolTip = "Ingrese en este campo el Nombre de la Proyección del Producto que desea registrar";
            }
        }
    }
}
