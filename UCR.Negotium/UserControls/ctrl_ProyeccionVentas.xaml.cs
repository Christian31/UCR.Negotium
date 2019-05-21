using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.Base.Enumerados;
using UCR.Negotium.Base.Utilidades;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
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
        private ProyeccionVenta proyeccionSelected;
        private List<ProyeccionVenta> proyeccionesList;
        private DataView proyeccionesTotales;
        private string signoMoneda;

        private ProyeccionVentaData proyeccionVentaData;
        private ProyectoData proyectoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_ProyeccionVentas()
        {
            InitializeComponent();
            DataContext = this;

            proyecto = new Proyecto();
            proyeccionSelected = new ProyeccionVenta();
            proyeccionesList = new List<ProyeccionVenta>();
            proyeccionesTotales = new DataView();

            proyeccionVentaData = new ProyeccionVentaData();
            proyectoData = new ProyectoData();
        }

        #region InternalMethods
        private void Reload()
        {
            SignoMoneda = LocalContext.GetSignoMoneda(CodProyecto);

            DTProyeccionesTotales = new DataView();
            ProyeccionesList = proyeccionVentaData.GetProyeccionesVenta(CodProyecto);

            ProyeccionesList.All(proy =>
            {
                proy.DetallesProyeccionVenta.ForEach(det =>
                {
                    det.PrecioFormat = det.Precio.FormatoMoneda(SignoMoneda);
                    det.SubtotalFormat = det.Subtotal.FormatoMoneda(SignoMoneda);
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

        public ProyeccionVenta ProyeccionSelected
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

        public List<ProyeccionVenta> ProyeccionesList
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
                MessageBox.Show(Constantes.ACTUALIZARPROYECTORESTRTIPONOFINANCIERO, Constantes.ACTUALIZARPROYECTOTLT,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (CustomMessageBox.ShowConfirmationMesage(Constantes.ELIMINARPROYECCIONMSG))
                {
                    if (proyeccionVentaData.EliminarProyeccionVenta(ProyeccionSelected.CodArticulo))
                    {
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.ProyeccionVentas);
                    }
                    else
                    {
                        MessageBox.Show(Constantes.ELIMINARPROYECCIONERROR, Constantes.ACTUALIZARPROYECTOTLT, 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion
    }
}