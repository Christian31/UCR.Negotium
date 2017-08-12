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
using UCR.Negotium.Extensions;

namespace UCR.Negotium.UserControls
{
    /// <summary>
    /// Interaction logic for ctrl_Financiamiento.xaml
    /// </summary>
    public partial class ctrl_Financiamiento : UserControl, INotifyPropertyChanged
    {
        private Proyecto proyecto;
        private Financiamiento financiamiento;
        private int codProyecto;
        private DataView dtFinanciamiento;

        private List<int> finalizacionDisponibles, anosDisponibles;

        private ProyectoData proyectoData;
        private FinanciamientoData financiamientoData;
        private InteresFinanciamientoData interesData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_Financiamiento()
        {
            InitializeComponent();
            DataContext = this;
            tbMonto.ToolTip = "Ingrese en este campo el monto del Financiamiento que desea registrar";

            proyectoData = new ProyectoData();
            financiamientoData = new FinanciamientoData();
            interesData = new InteresFinanciamientoData();

            finalizacionDisponibles = anosDisponibles = new List<int>();
            proyecto = new Proyecto();
            financiamiento = new Financiamiento();
            dtFinanciamiento = new DataView();

            Reload();            
        }

        int anoFinalizacion = 0;
        int anoInicial = 0;
        bool interesFijo;
        public void Reload()
        {
            proyecto = proyectoData.GetProyecto(CodProyecto);
            FinanciamientoSelected = financiamientoData.GetFinanciamiento(CodProyecto);
            FinanciamientoSelected.TasaIntereses = interesData.GetInteresesFinanciamiento(CodProyecto);

            if (FinanciamientoSelected.CodFinanciamiento.Equals(0))
            {
                FinanciamientoSelected.AnoFinalPago = FinalizacionDisponible.LastOrDefault();
                FinanciamientoSelected.AnoInicialPago = AnosDisponibles.FirstOrDefault();
            }

            anoFinalizacion = FinanciamientoSelected.AnoFinalPago;
            anoInicial = FinanciamientoSelected.AnoInicialPago;
            interesFijo = FinanciamientoSelected.InteresFijo;

            ActualizarDTFinanciamiento();

            PropertyChanged(this, new PropertyChangedEventArgs("FinanciamientoSelected"));
            PropertyChanged(this, new PropertyChangedEventArgs("FinalizacionDisponible"));
            PropertyChanged(this, new PropertyChangedEventArgs("AnosDisponibles"));
        }

        private void ActualizarDTFinanciamiento()
        {
            if (!FinanciamientoSelected.TasaIntereses.Count.Equals(0))
            {
                DTFinanciamiento = FinanciamientoSelected.InteresFijo  ?
                    DatatableBuilder.GenerarFinanciamientoIF(FinanciamientoSelected, proyecto.CodProyecto).AsDataView():
                    DatatableBuilder.GenerarFinanciamientoIV(FinanciamientoSelected, proyecto.CodProyecto).AsDataView();

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

        public Financiamiento FinanciamientoSelected
        {
            get { return financiamiento; }
            set { financiamiento = value; }
        }

        public List<int> FinalizacionDisponible
        {
            get
            {
                List<int> tiempo = new List<int>();
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    tiempo.Add(proyecto.AnoInicial + i);
                }//for

                finalizacionDisponibles = tiempo;
                return finalizacionDisponibles;
            }
            set
            {
                finalizacionDisponibles = value;
            }
        }

        private List<int> GetAnosDisponibles()
        {
            List<int> anos = new List<int>();
            foreach (int anoDisponible in FinalizacionDisponible)
            {
                if (anoDisponible > FinanciamientoSelected.AnoFinalPago)
                    break;
                else
                    anos.Add(anoDisponible);
            }

            return anos;
        }

        public List<int> AnosDisponibles
        {
            get
            {
                anosDisponibles = GetAnosDisponibles();
                return anosDisponibles;
            }
            set
            {
                anosDisponibles = value;
                PropertyChanged(this, new PropertyChangedEventArgs("AnosDisponibles"));
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
            int monto = 0;
            if (!int.TryParse(tbMonto.Text, out monto))
            {
                tbMonto.Text = string.Empty;
            }
            else
            {
                ActualizarDTFinanciamiento();
            }
        }

        private void lblTasaInteres_Click(object sender, RoutedEventArgs e)
        {
            if (!proyecto.TipoProyecto.CodTipo.Equals(2))
            {
                if (FinanciamientoSelected.InteresFijo)
                {
                    RegistrarTasaInteresFijo interesFinanciamiento = new RegistrarTasaInteresFijo(CodProyecto, FinanciamientoSelected);
                    interesFinanciamiento.ShowDialog();

                    if (!interesFinanciamiento.IsActive && interesFinanciamiento.Reload)
                    {
                        FinanciamientoSelected.TasaIntereses = 
                            new List<InteresFinanciamiento>() { interesFinanciamiento.InteresFijo };

                        if (lblTasaInteres.Foreground.Equals(Brushes.Red))
                        {
                            lblTasaInteres.Foreground = Brushes.Blue;
                        }
                        ActualizarDTFinanciamiento();
                    }   
                }
                else
                {
                    RegistrarTasaInteresVariable interesFinanciamiento = new RegistrarTasaInteresVariable(CodProyecto, FinanciamientoSelected);
                    interesFinanciamiento.ShowDialog();

                    if (!interesFinanciamiento.IsActive && interesFinanciamiento.Reload)
                    {
                        if (lblTasaInteres.Foreground.Equals(Brushes.Red))
                        {
                            lblTasaInteres.Foreground = Brushes.Blue;
                        }
                        FinanciamientoSelected.TasaIntereses = new List<InteresFinanciamiento>(interesFinanciamiento.InteresVariable);
                    }   ActualizarDTFinanciamiento();
                }
            }
            else
            {
                MessageBox.Show("Este Tipo de Análisis es Ambiental, si desea realizar un Análisis Completo actualice el Tipo de Análisis del Proyecto", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (!proyecto.TipoProyecto.CodTipo.Equals(2))
            {
                if (!ValidateRequiredFields())
                {
                    if (FinanciamientoSelected.CodFinanciamiento.Equals(0))
                    {
                        Financiamiento financiamientoTemp = financiamientoData.InsertarFinanciamiento(FinanciamientoSelected, proyecto.CodProyecto);
                        if (!financiamientoTemp.CodFinanciamiento.Equals(-1) && GuardarIntereses())
                        {
                            //success
                            FinanciamientoSelected = financiamientoTemp;
                            ActualizarDTFinanciamiento();
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
                        if (financiamientoData.ActualizarFinanciamiento(FinanciamientoSelected) && GuardarIntereses())
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
            else
            {
                MessageBox.Show("Este Tipo de Análisis es Ambiental, si desea realizar un Análisis Completo actualice el Tipo de Análisis del Proyecto", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool ValidateRequiredFields()
        {
            bool result = false;

            if(FinanciamientoSelected.MontoFinanciamiento <= 0)
            {
                tbMonto.ToolTip = "Este campo es requerido";
                tbMonto.BorderBrush = Brushes.Red;
                result = true;
            }

            if(FinanciamientoSelected.TasaIntereses.Count.Equals(0) || 
                (!FinanciamientoSelected.InteresFijo &&
                (!FinanciamientoSelected.AnoInicialPago.Equals(FinanciamientoSelected.TasaIntereses.FirstOrDefault().AnoInteres) || 
                !FinanciamientoSelected.TiempoFinanciamiento.Equals(FinanciamientoSelected.TasaIntereses.Count))))
            {
                lblTasaInteres.Foreground = Brushes.Red;
                result = true;
            }
            return result;
        }

        private bool GuardarIntereses()
        {
            foreach (InteresFinanciamiento interesVariable in FinanciamientoSelected.TasaIntereses)
            {
                if (interesVariable.CodInteresFinanciamiento.Equals(0))
                {
                    if (!interesData.InsertarInteresFinanciamiento(interesVariable, codProyecto))
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar la tasa de interés del financiamiento del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    if (!interesData.ActualizarInteresFinanciamiento(interesVariable))
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar la tasa de interés del financiamiento del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }

            return true;
        }

        private void cbxAnosDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxAnosDisponibles.IsLoaded)
            {
                if (cbxAnosDisponibles.SelectedItem != null &&
                    anoInicial.Equals((int)cbxAnosDisponibles.SelectedItem))
                {
                    ActualizarDTFinanciamiento();
                }
                else
                {
                    DTFinanciamiento = new DataView();
                }
            }
        }

        private void cbInteresFijo_Checked(object sender, RoutedEventArgs e)
        {
            if (interesFijo.Equals(cbInteresFijo.IsChecked))
            {
                ActualizarDTFinanciamiento();
            }
            else
            {
                DTFinanciamiento = new DataView();
            }
        }

        private void cbInteresFijo_Unchecked(object sender, RoutedEventArgs e)
        {
            if (interesFijo.Equals(cbInteresFijo.IsChecked))
            {
                ActualizarDTFinanciamiento();
            }
            else
            {
                DTFinanciamiento = new DataView();
            }
        }

        private void cbxFinalizacionFinanciamiento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxFinalizacionFinanciamiento.IsLoaded)
            {
                AnosDisponibles = GetAnosDisponibles();

                if (!AnosDisponibles.Contains(anoInicial))
                {
                    FinanciamientoSelected.AnoInicialPago = AnosDisponibles.FirstOrDefault();
                    PropertyChanged(this, new PropertyChangedEventArgs("FinanciamientoSelected"));
                }
                else
                {
                    if (anoFinalizacion.Equals((int)cbxFinalizacionFinanciamiento.SelectedItem)
                        && (cbxAnosDisponibles.SelectedItem != null &&
                            anoInicial.Equals((int)cbxAnosDisponibles.SelectedItem)))
                    {
                        ActualizarDTFinanciamiento();
                    }
                    else
                    {
                        DTFinanciamiento = new DataView();
                    }
                }
            }
        }
    }
}
