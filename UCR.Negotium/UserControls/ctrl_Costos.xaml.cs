using UCR.Negotium.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UCR.Negotium.Domain;
using System.Data;
using UCR.Negotium.DataAccess;
using System.ComponentModel;
using UCR.Negotium.Extensions;
using UCR.Negotium.Domain.Enums;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_Costos.xaml
    /// </summary>
    public partial class ctrl_Costos : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private int codProyecto;
        private Costo costoSelected;
        private List<Costo> costosList;
        private DataView costosTotales;

        private CostoData costoData;
        private ProyectoData proyectoData;
        private VariacionAnualCostoData variacionCostoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_Costos()
        {
            InitializeComponent();
            DataContext = this;

            costoData = new CostoData();
            proyectoData = new ProyectoData();
            variacionCostoData = new VariacionAnualCostoData();

            proyecto = new Proyecto();
            costoSelected = new Costo();
            costosList = new List<Costo>();
            costosTotales = new DataView();
        }

        #region Properties
        public List<Costo> CostosList
        {
            get
            {
                return costosList;
            }
            set
            {
                costosList = value;
                CostoSelected = CostosList.FirstOrDefault();
                PropertyChanged(this, new PropertyChangedEventArgs("CostosList"));
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

        public Costo CostoSelected
        {
            get
            {
                return costoSelected;
            }
            set
            {
                costoSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CostoSelected"));
            }
        }

        public DataView DTCostosTotales
        {
            get
            {
                return costosTotales;
            }
            set
            {
                costosTotales = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DTCostosTotales"));
            }
        }
        #endregion

        #region Events
        private void btnCrearCosto_Click(object sender, RoutedEventArgs e)
        {
            if (!proyecto.TipoProyecto.CodTipo.Equals(2))
            {
                RegistrarCosto registarCosto = new RegistrarCosto(CodProyecto);
                registarCosto.ShowDialog();

                if (registarCosto.IsActive == false && registarCosto.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.Costos);
                }
            }
            else
            {
                MessageBox.Show("Este Tipo de Análisis es Ambiental, si desea realizar un Análisis Completo actualice el Tipo de Análisis del Proyecto", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditarCosto_Click(object sender, RoutedEventArgs e)
        {
            if(CostoSelected != null)
            {
                RegistrarCosto registarCosto = new RegistrarCosto(CodProyecto, CostoSelected.CodCosto);
                registarCosto.ShowDialog();

                if (registarCosto.IsActive == false && registarCosto.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.Costos);
                }
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (CostoSelected != null)
            {
                if (MessageBox.Show("Esta seguro que desea eliminar este costo?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (costoData.EliminarCosto(CostoSelected.CodCosto))
                    {
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.Costos);
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error al eliminar el costo del proyecto",
                            "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void lblVariacionCostos_Click(object sender, RoutedEventArgs e)
        {
            if (!proyecto.TipoProyecto.CodTipo.Equals(2))
            {
                RegistrarVariacionAnualCostos registrarVariacionAnual = new RegistrarVariacionAnualCostos(CodProyecto);
                registrarVariacionAnual.ShowDialog();

                if (registrarVariacionAnual.IsActive == false && registrarVariacionAnual.Reload)
                {
                    LocalContext.ReloadUserControls(CodProyecto, Modulo.Costos);
                }
            }
            else
            {
                MessageBox.Show("Este Tipo de Análisis es Ambiental, si desea realizar un Análisis Completo actualice el Tipo de Análisis del Proyecto", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #endregion

        #region InternalMethods
        private void Reload()
        {
            string signoMoneda = LocalContext.GetSignoMoneda(CodProyecto);

            DTCostosTotales = new DataView();
            CostosList = costoData.GetCostos(CodProyecto);

            CostosList.All(costo =>
            {
                costo.CostosMensuales.ForEach(det =>
                {
                    det.CostoUnitarioFormat = signoMoneda + " " + det.CostoUnitario.ToString("#,##0.##");
                    det.SubtotalFormat = signoMoneda + " " + det.Subtotal.ToString("#,##0.##");
                });
                return true;
            });

            proyecto = proyectoData.GetProyecto(CodProyecto);
            proyecto.Costos = CostosList;
            proyecto.VariacionCostos = variacionCostoData.GetVariacionAnualCostos(CodProyecto);
            if (!proyecto.Costos.Count.Equals(0))
            {
                DTCostosTotales = DatatableBuilder.GenerarCostosTotales(proyecto).AsDataView();
            }
        }
        #endregion
    }
}
