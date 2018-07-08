using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Enums;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_ProyeccionVentas.xaml
    /// </summary>
    public partial class ctrl_ProyeccionVentas : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private int codProyecto;
        private ProyeccionVentaArticulo proyeccionSelected;
        private List<ProyeccionVentaArticulo> proyeccionesList;
        private DataView proyeccionesTotales;
        private string signoMoneda;

        private ProyeccionVentaArticuloData proyeccionArticuloData;
        private ProyectoData proyectoData;
        private CrecimientoOfertaArticuloData crecimientoOfertaData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_ProyeccionVentas()
        {
            InitializeComponent();
            DataContext = this;

            proyecto = new Proyecto();
            proyeccionSelected = new ProyeccionVentaArticulo();
            proyeccionesList = new List<ProyeccionVentaArticulo>();
            proyeccionesTotales = new DataView();

            proyeccionArticuloData = new ProyeccionVentaArticuloData();
            proyectoData = new ProyectoData();
            crecimientoOfertaData = new CrecimientoOfertaArticuloData();
        }

        #region InternalMethods
        private void Reload()
        {
            SignoMoneda = LocalContext.GetSignoMoneda(CodProyecto);

            DTProyeccionesTotales = new DataView();
            ProyeccionesList = proyeccionArticuloData.GetProyeccionesVentaArticulo(CodProyecto);

            ProyeccionesList.All(proy =>
            {
                proy.DetallesProyeccionVenta.ForEach(det =>
                {
                    det.PrecioFormat = SignoMoneda + " " + det.Precio.ToString("#,##0.##");
                    det.SubtotalFormat = SignoMoneda + " " + det.Subtotal.ToString("#,##0.##");
                });
                return true;
            });

            proyecto = proyectoData.GetProyecto(CodProyecto);
            proyecto.Proyecciones = ProyeccionesList;

            if (!proyecto.Proyecciones.Count.Equals(0))
            {
                DTProyeccionesTotales = DatatableBuilder.GenerarIngresosTotales(proyecto).AsDataView();
            }

            PropertyChanged(this, new PropertyChangedEventArgs("ProyeccionesList"));
        }
        #endregion

        #region Properties
        public string SignoMoneda
        {
            get
            {
                return signoMoneda;
            }
            set
            {
                signoMoneda = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SignoMoneda"));
            }
        }

        public DataView DTProyeccionesTotales
        {
            get
            {
                return proyeccionesTotales;
            }
            set
            {
                proyeccionesTotales = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DTProyeccionesTotales"));
            }
        }

        public ProyeccionVentaArticulo ProyeccionSelected
        {
            get
            {
                return proyeccionSelected;
            }
            set
            {
                proyeccionSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ProyeccionSelected"));
            }
        }

        public List<ProyeccionVentaArticulo> ProyeccionesList
        {
            get
            {
                return proyeccionesList;
            }
            set
            {
                proyeccionesList = value;
                ProyeccionSelected = ProyeccionesList.FirstOrDefault();
            }
        }

        public int CodProyecto
        {
            get
            {
                return codProyecto;
            }
            set
            {
                codProyecto = value;
                Reload();
            }
        }
        #endregion

        #region Events
        private void btnAgregarProyeccion_Click(object sender, RoutedEventArgs e)
        {
            if (proyecto.TipoProyecto.CodTipo.Equals(1))
            {
                RegistrarProyeccionVenta registarProyeccion = new RegistrarProyeccionVenta(CodProyecto);
                registarProyeccion.ShowDialog();

                if (registarProyeccion.IsActive == false && registarProyeccion.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.ProyeccionVentas);
                }
            }
            else
            {
                MessageBox.Show("Este Tipo de Análisis no permite agregar Proyecciones de Ventas, si desea hacer un análisis con Proyecciones de Ventas incluídas realice un proyecto de Tipo Financiero", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditarProyeccion_Click(object sender, RoutedEventArgs e)
        {
            if (ProyeccionSelected != null)
            {
                RegistrarProyeccionVenta registarProyeccion = new RegistrarProyeccionVenta(CodProyecto, ProyeccionSelected.CodArticulo);
                registarProyeccion.ShowDialog();

                if (registarProyeccion.IsActive == false && registarProyeccion.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.ProyeccionVentas);
                }
            }
        }

        private void btnEliminarProyeccion_Click(object sender, RoutedEventArgs e)
        {
            if (ProyeccionSelected != null)
            {
                if (CustomMessageBox.Show("Esta seguro que desea eliminar esta proyección?"))
                {
                    if (crecimientoOfertaData.EliminarCrecimientoObjetoInteres(ProyeccionSelected.CodArticulo) && 
                        proyeccionArticuloData.EliminarProyeccionVenta(ProyeccionSelected.CodArticulo))
                    {
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.ProyeccionVentas);
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error al eliminar la proyección del proyecto",
                            "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion
    }
}
