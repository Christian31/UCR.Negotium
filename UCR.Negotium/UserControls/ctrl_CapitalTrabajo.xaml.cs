using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.Base.Enumerados;
using UCR.Negotium.Base.Utilidades;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_CapitalTrabajo.xaml
    /// </summary>
    public partial class ctrl_CapitalTrabajo : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyectoSelected;
        private int codProyecto;
        private DataView capitalTrabajo;
        private double recuperacionCT;
        private string signoMoneda;

        private ProyectoData proyectoData;
        private CostoData costoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_CapitalTrabajo()
        {
            InitializeComponent();
            DataContext = this;
            tbDiasDesface.ToolTip = "Ingrese en este campo los días al desface del Capital de trabajo";

            proyectoData = new ProyectoData();
            costoData = new CostoData();

            proyectoSelected = new Proyecto();
            capitalTrabajo = new DataView();
        }

        #region InternalMethods
        private void Reload()
        {
            DTCapitalTrabajo = new DataView();
            recuperacionCT = 0;
            signoMoneda = LocalContext.GetSignoMoneda(CodProyecto);

            ProyectoSelected = proyectoData.GetProyecto(CodProyecto);
            ProyectoSelected.Costos = costoData.GetCostos(CodProyecto);

            ActualizarDTCapitalTrabajo();
        }

        bool conError = false;
        private void ActualizarDTCapitalTrabajo()
        {
            if (!ProyectoSelected.Costos.Count.Equals(0) && !ValidateRequiredFields())
            {
                DatatableBuilder.GenerarCapitalTrabajo(ProyectoSelected, out capitalTrabajo, out recuperacionCT);
                conError = false;
            }
            else
            {
                capitalTrabajo = new DataView();
                conError = true;
            }

            PropertyChanged(this, new PropertyChangedEventArgs("RecuperacionCT"));
            PropertyChanged(this, new PropertyChangedEventArgs("DTCapitalTrabajo"));
        }

        private bool ValidateRequiredFields()
        {
            bool result = false;
            if (Validador.ValideEntreRangoDeNumero(10, 99, ProyectoSelected.DiasDesfaceCapitalTrabajo))
            {
                tbDiasDesface.ToolTip = "Los Días al desface debe ser un valor mayor a 10 y menor a 99";
                tbDiasDesface.BorderBrush = Brushes.Red;
                result = true;
            }

            return result;
        }
        #endregion

        #region Properties
        public string RecuperacionCT
        {
            get
            {
                if (conError)
                {
                    return "Indefinido";
                }
                return recuperacionCT.FormatoMoneda(signoMoneda);
            }
            set
            {
                double.TryParse(value, out recuperacionCT);
            }
        }

        public DataView DTCapitalTrabajo
        {
            get
            {
                return capitalTrabajo;
            }
            set
            {
                capitalTrabajo = value;
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

        public Proyecto ProyectoSelected
        {
            get
            {
                return proyectoSelected;
            }
            set
            {
                proyectoSelected = value;
            }
        }
        #endregion

        #region Events
        private void datagridCapitalTrabajo_Loaded(object sender, RoutedEventArgs e)
        {
            if (datagridCapitalTrabajo.Columns.Count >0)
            {
                datagridCapitalTrabajo.Columns[0].Width = 130;
            }
        }

        bool tbDiasDesfacetxtChngEvent = true;
        private void tbDiasDesface_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDiasDesfacetxtChngEvent)
            {
                if (tbDiasDesface.BorderBrush == Brushes.Red)
                {
                    tbDiasDesface.BorderBrush = Brushes.Gray;
                    tbDiasDesface.ToolTip = "Ingrese en este campo los días al desface del Capital de trabajo";
                }

                tbDiasDesface.Text = tbDiasDesface.Text.CheckStringFormat();
                ActualizarDTCapitalTrabajo();
            }
            else
            {
                tbDiasDesfacetxtChngEvent = true;
            }
        }

        private void tbDiasDesface_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbDiasDesface.Text.Equals("0") || tbDiasDesface.Text.Equals("0.00"))
            {
                tbDiasDesfacetxtChngEvent = false;
                tbDiasDesface.Text = "";
            }
        }

        private void tbDiasDesface_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbDiasDesface.Text.Equals(""))
            {
                tbDiasDesfacetxtChngEvent = false;
                tbDiasDesface.Text = "0.00";
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!proyectoSelected.TipoProyecto.CodTipo.Equals(2))
            {
                if (!ValidateRequiredFields())
                {
                    if (proyectoData.EditarProyectoCapitalTrabajo(CodProyecto, ProyectoSelected.DiasDesfaceCapitalTrabajo))
                    {
                        //success
                        LocalContext.ReloadUserControls(CodProyecto, Modulo.Costos);
                        MessageBox.Show(Constantes.ACTUALIZARPROYECTOMSG, Constantes.ACTUALIZARPROYECTOTLT,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //error
                        MessageBox.Show(Constantes.ACTUALIZARPROYECTOERROR, Constantes.ACTUALIZARPROYECTOTLT,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(Constantes.ACTUALIZARPROYECTORESTRTIPOAMBIENTAL, Constantes.ACTUALIZARPROYECTOTLT,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #endregion
    }
}
