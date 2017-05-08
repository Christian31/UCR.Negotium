using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
using UCR.Negotium.Utils;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ProyeccionVentas.xaml
    /// </summary>
    public partial class ProyeccionVentas : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private int codProyecto;
        private ProyeccionVentaArticulo proyeccionSelected;
        private List<ProyeccionVentaArticulo> proyeccionesList;
        private DataView proyeccionesTotales;

        private ProyeccionVentaArticuloData proyeccionArticuloData;
        private ProyectoData proyectoData;
        private CrecimientoOfertaObjetoInteresData crecimientoOfertaData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ProyeccionVentas()
        {
            InitializeComponent();
            DataContext = this;

            proyecto = new Proyecto();
            proyeccionSelected = new ProyeccionVentaArticulo();
            proyeccionesList = new List<ProyeccionVentaArticulo>();
            proyeccionesTotales = new DataView();

            proyeccionArticuloData = new ProyeccionVentaArticuloData();
            proyectoData = new ProyectoData();
            crecimientoOfertaData = new CrecimientoOfertaObjetoInteresData();
        }

        public void Reload()
        {
            DTProyeccionesTotales = new DataView();
            ProyeccionesList = proyeccionArticuloData.GetProyeccionesVentaArticulo(CodProyecto);
            proyecto = proyectoData.GetProyecto(CodProyecto);
            proyecto.Proyecciones = ProyeccionesList;

            if (!proyecto.Proyecciones.Count.Equals(0))
            {
                DTProyeccionesTotales = DatatableBuilder.GenerarDTIngresosGenerados(proyecto).AsDataView();
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
                PropertyChanged(this, new PropertyChangedEventArgs("ProyeccionesList"));
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

        private void btnAgregarProyeccion_Click(object sender, RoutedEventArgs e)
        {
            RegistrarProyeccionVenta registarProyeccion = new RegistrarProyeccionVenta(CodProyecto);
            registarProyeccion.ShowDialog();

            if (registarProyeccion.IsActive == false && registarProyeccion.Reload)
            {
                Reload();
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
                    Reload();
                }
            }
        }

        private void btnEliminarProyeccion_Click(object sender, RoutedEventArgs e)
        {
            if (ProyeccionSelected != null)
            {
                if (MessageBox.Show("Esta seguro que desea eliminar esta proyección?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (crecimientoOfertaData.EliminarCrecimientoObjetoInteres(ProyeccionSelected.CodArticulo) && 
                        proyeccionArticuloData.EliminarProyeccionVenta(ProyeccionSelected.CodArticulo))
                    {
                        Reload();
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error al eliminar la proyección del proyecto",
                            "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
