using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Dialogs;
using UCR.Negotium.Domain;
using UCR.Negotium.Utils;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for Financiamiento.xaml
    /// </summary>
    public partial class Financiamiento : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private Domain.Financiamiento financiamiento;
        private int codProyecto;
        private DataView dtFinanciamiento;
        private List<InteresFinanciamiento> tasaInteres;

        private ProyectoData proyectoData;
        private FinanciamientoData financiamientoData;
        private InteresFinanciamientoData interesData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public Financiamiento()
        {
            InitializeComponent();
            DataContext = this;
            tbMonto.ToolTip = "Ingrese en este campo el monto del Financiamiento que desea registrar";

            proyectoData = new ProyectoData();
            financiamientoData = new FinanciamientoData();
            interesData = new InteresFinanciamientoData();

            proyecto = new Proyecto();
            financiamiento = new Domain.Financiamiento();
            financiamiento.InteresFijo = true;
            dtFinanciamiento = new DataView();
            tasaInteres = new List<InteresFinanciamiento>();

            Reload();
        }

        public void Reload()
        {
            proyecto = proyectoData.GetProyecto(CodProyecto);
            FinanciamientoSelected = financiamientoData.GetFinanciamiento(CodProyecto);
            tasaInteres = interesData.GetInteresesFinanciamiento(CodProyecto);
            proyecto.Financiamiento = FinanciamientoSelected;
            proyecto.InteresesFinanciamiento = tasaInteres;

            if (FinanciamientoSelected.CodFinanciamiento.Equals(0))
            {
                FinanciamientoSelected.TiempoFinanciamiento = TiempoDisponible.FirstOrDefault();
                FinanciamientoSelected.AnoInicialPago = AnosDisponibles.FirstOrDefault();
            }

            ActualizarDTFinanciamiento();
        }

        private void ActualizarDTFinanciamiento()
        {
            if (!proyecto.InteresesFinanciamiento.Count.Equals(0))
            {
                if (FinanciamientoSelected.InteresFijo)
                {
                    DTFinanciamiento = DatatableBuilder.GenerarDTFinanciamientoIF(proyecto.AnoInicial, FinanciamientoSelected.MontoFinanciamiento, FinanciamientoSelected.TiempoFinanciamiento, tasaInteres.FirstOrDefault()).AsDataView();
                }
                else
                {
                    DTFinanciamiento = DatatableBuilder.GenerarDTFinanciamientoIV(proyecto).AsDataView();                
                }

                if (dgFinanciamiento.IsLoaded && dgFinanciamiento.Columns.Count > 0)
                {
                    this.dgFinanciamiento.Columns[0].Header = "Año de Pago";
                    this.dgFinanciamiento.Columns[1].Header = "Saldo";
                    this.dgFinanciamiento.Columns[2].Header = "Cuota";
                    this.dgFinanciamiento.Columns[3].Header = "Interés";
                    this.dgFinanciamiento.Columns[4].Header = "Amortización";
                    this.dgFinanciamiento.Columns[0].Width = 130;
                    this.dgFinanciamiento.Columns[1].Width = 160;
                    this.dgFinanciamiento.Columns[2].Width = 150;
                    this.dgFinanciamiento.Columns[3].Width = 150;
                    this.dgFinanciamiento.Columns[4].Width = 160;
                }
            }
        }

        public DataView DTFinanciamiento
        {
            get
            {
                return dtFinanciamiento;
            }
            set
            {
                dtFinanciamiento = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DTFinanciamiento"));
            }
        }

        public Domain.Financiamiento FinanciamientoSelected
        {
            get
            {
                return financiamiento;
            }
            set
            {
                financiamiento = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FinanciamientoSelected"));
            }
        }

        public List<int> TiempoDisponible
        {
            get
            {
                List<int> tiempo = new List<int>();
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    tiempo.Add(i);
                }//for

                return tiempo;
            }
            set
            {
                TiempoDisponible = value;
            }
        }

        public List<int> AnosDisponibles
        {
            get
            {
                List<int> anos = new List<int>();
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    int anoActual = proyecto.AnoInicial + i;
                    anos.Add(anoActual);
                }//for

                return anos;
            }
            set
            {
                AnosDisponibles = value;
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

        private void tbMonto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbMonto.BorderBrush == Brushes.Red)
            {
                tbMonto.BorderBrush = Brushes.Gray;
                tbMonto.ToolTip = "Ingrese en este campo el monto del Financiamiento que desea registrar";
            }
            int costoUnitario = 0;
            if (!int.TryParse(tbMonto.Text, out costoUnitario))
            {
                tbMonto.Text = string.Empty;
            }
            else
            {
                ActualizarDTFinanciamiento();
            }
        }

        private void cbFijo_Checked(object sender, RoutedEventArgs e)
        {
            cbVariable.IsChecked = false;
        }

        private void cbFijo_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbFijo.IsChecked.Value.Equals(false))
            {
                cbVariable.IsChecked = true;
            }
        }

        private void cbVariable_Checked(object sender, RoutedEventArgs e)
        {
            cbFijo.IsChecked = false;
        }

        private void lblTasaInteres_Click(object sender, RoutedEventArgs e)
        {
            if (FinanciamientoSelected.InteresFijo)
            {
                RegistrarTasaInteresFijo interesFinanciamiento = new RegistrarTasaInteresFijo(CodProyecto, FinanciamientoSelected);
                interesFinanciamiento.ShowDialog();

                if (!interesFinanciamiento.IsActive && interesFinanciamiento.Reload)
                {
                    Reload();
                }
            }
            else
            {
                RegistrarTasaInteresFinanciamiento interesFinanciamiento = new RegistrarTasaInteresFinanciamiento(CodProyecto, FinanciamientoSelected);
                interesFinanciamiento.ShowDialog();

                if (!interesFinanciamiento.IsActive && interesFinanciamiento.Reload)
                {
                    Reload();
                }
            }
        }

        private void dgFinanciamiento_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgFinanciamiento.Columns.Count > 0)
            {
                this.dgFinanciamiento.Columns[0].Header = "Año de Pago";
                this.dgFinanciamiento.Columns[1].Header = "Saldo";
                this.dgFinanciamiento.Columns[2].Header = "Cuota";
                this.dgFinanciamiento.Columns[3].Header = "Interés";
                this.dgFinanciamiento.Columns[4].Header = "Amortización";
                this.dgFinanciamiento.Columns[0].Width = 130;
                this.dgFinanciamiento.Columns[1].Width = 160;
                this.dgFinanciamiento.Columns[2].Width = 150;
                this.dgFinanciamiento.Columns[3].Width = 150;
                this.dgFinanciamiento.Columns[4].Width = 160;
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (FinanciamientoSelected.CodFinanciamiento.Equals(0))
                {
                    Domain.Financiamiento financiamientoTemp = financiamientoData.InsertarFinanciamiento(FinanciamientoSelected, proyecto.CodProyecto);
                    if (!financiamientoTemp.CodFinanciamiento.Equals(-1))
                    {
                        //success
                        ActualizarDTFinanciamiento();
                        FinanciamientoSelected = financiamientoTemp;
                        MessageBox.Show("El financiamiento del proyecto se ha insertado correctamente", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar el financiamiento del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (financiamientoData.ActualizarFinanciamiento(FinanciamientoSelected))
                    {
                        //success
                        ActualizarDTFinanciamiento();
                        MessageBox.Show("El financiamiento del proyecto se ha actualizado correctamente", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar el financiamiento del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool ValidateRequiredFields()
        {
            if(FinanciamientoSelected.MontoFinanciamiento <= 0)
            {
                tbMonto.ToolTip = "Este campo es requerido";
                tbMonto.BorderBrush = Brushes.Red;
                return true;
            }
            return false;
        }
    }
}
