using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.Base.Enumerados;
using UCR.Negotium.Base.Utilidades;
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
        private ProyectoLite proyecto;
        private Financiamiento financiamiento;
        private InteresFinanciamiento interesesFijos;
        private List<InteresFinanciamiento> interesesVariables;
        private int codProyecto;
        private DataView dtFinanciamiento;

        private List<int> finalizacionDisponibles, anosDisponibles;

        private ProyectoData proyectoData;
        private FinanciamientoData financiamientoData;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ctrl_Financiamiento()
        {
            InitializeComponent();
            DataContext = this;
            tbMonto.ToolTip = "Ingrese en este campo el monto del Financiamiento que desea registrar";

            proyectoData = new ProyectoData();
            financiamientoData = new FinanciamientoData();

            proyecto = new ProyectoLite();
            Inicialice();

            Reload();            
        }

        private void Inicialice()
        {
            finalizacionDisponibles = anosDisponibles = new List<int>();
            financiamiento = new Financiamiento();
            dtFinanciamiento = new DataView();
            interesesFijos = new InteresFinanciamiento();
            interesesVariables = new List<InteresFinanciamiento>();
        }

        int anoFinalizacion = 0;
        int anoInicial = 0;
        private void Reload()
        {
            proyecto = proyectoData.GetProyectoLite(CodProyecto);
            if (proyecto.ConFinanciamiento)
            {
                FinanciamientoSelected = financiamientoData.GetFinanciamiento(CodProyecto);

                string signoMoneda = LocalContext.GetSignoMoneda(CodProyecto);
                FinanciamientoSelected.MontoFinanciamientoFormat =
                    FinanciamientoSelected.MontoFinanciamiento.FormatoMoneda(signoMoneda);

                if (FinanciamientoSelected.InteresFijo)
                {
                    interesesFijos = FinanciamientoSelected.TasaIntereses.FirstOrDefault();
                }
                else
                {
                    interesesVariables = FinanciamientoSelected.TasaIntereses;
                }

                if (FinanciamientoSelected.CodFinanciamiento.Equals(0))
                {
                    FinanciamientoSelected.AnoFinalPago = FinalizacionDisponible.LastOrDefault();
                    FinanciamientoSelected.AnoInicialPago = AnosDisponibles.FirstOrDefault();
                }

                anoFinalizacion = FinanciamientoSelected.AnoFinalPago;
                anoInicial = FinanciamientoSelected.AnoInicialPago;

                ActualizarDTFinanciamiento();
            }
            else
            {
                Inicialice();
            }

            PropertyChanged(this, new PropertyChangedEventArgs("FinanciamientoSelected"));
            PropertyChanged(this, new PropertyChangedEventArgs("FinalizacionDisponible"));
            PropertyChanged(this, new PropertyChangedEventArgs("AnosDisponibles"));
        }

        private bool SePuedeActualizarDT()
        {
            bool sePuedeActualizar;
            sePuedeActualizar = !FinanciamientoSelected.TasaIntereses.Count.Equals(0) &&
                FinanciamientoSelected.TasaIntereses.First().AnoInteres != 0;

            if (sePuedeActualizar && !FinanciamientoSelected.InteresFijo)
            {
                sePuedeActualizar = FinanciamientoSelected.TiempoFinanciamiento.Equals(FinanciamientoSelected.TasaIntereses.Count) &&
                    FinanciamientoSelected.AnoInicialPago.Equals(FinanciamientoSelected.TasaIntereses.FirstOrDefault().AnoInteres);
            }

            return sePuedeActualizar;
        }

        private void ActualizarDTFinanciamiento()
        {
            FinanciamientoSelected.TasaIntereses = FinanciamientoSelected.InteresFijo ?
                new List<InteresFinanciamiento> { interesesFijos } : interesesVariables;

            if (SePuedeActualizarDT())
            {
                if (lblTasaInteres.Foreground.Equals(Brushes.Red))
                {
                    lblTasaInteres.Foreground = Brushes.Blue;
                }

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
            else
            {
                DTFinanciamiento = new DataView();
            }
        }

        public bool CargaDesdeCombos { get; set; }

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
                if (proyecto.ConFinanciamiento)
                {
                    List<int> tiempo = new List<int>();
                    for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                    {
                        tiempo.Add(proyecto.AnoInicial + i);
                    }//for

                    finalizacionDisponibles = tiempo;
                }
                
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
            if (tbMontotxtChngEvent)
            {
                if (tbMonto.BorderBrush == Brushes.Red)
                {
                    tbMonto.BorderBrush = Brushes.Gray;
                    tbMonto.ToolTip = "Ingrese en este campo el monto del Financiamiento que desea registrar";
                }

                tbMonto.Text = tbMonto.Text.CheckStringFormat();
                ActualizarDTFinanciamiento();
            }
            else
            {
                tbMontotxtChngEvent = true;
            }            
        }

        private void lblTasaInteres_Click(object sender, RoutedEventArgs e)
        {
            if (!proyecto.CodTipoProyecto.Equals(2))
            {
                if (proyecto.ConFinanciamiento)
                {
                    if (FinanciamientoSelected.InteresFijo)
                    {
                        RegistrarTasaInteresFijo interesFinanciamiento = new RegistrarTasaInteresFijo(CodProyecto, FinanciamientoSelected);
                        interesFinanciamiento.ShowDialog();

                        if (!interesFinanciamiento.IsActive && interesFinanciamiento.Reload)
                        {
                            interesesFijos = interesFinanciamiento.InteresFijo;
                            ActualizarDTFinanciamiento();
                        }
                    }
                    else
                    {
                        RegistrarTasaInteresVariable interesFinanciamiento = new RegistrarTasaInteresVariable(CodProyecto, FinanciamientoSelected);
                        interesFinanciamiento.ShowDialog();

                        if (!interesFinanciamiento.IsActive && interesFinanciamiento.Reload)
                        {
                            interesesVariables = interesFinanciamiento.InteresVariable;
                            ActualizarDTFinanciamiento();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(Constantes.ACTUALIZARPROYECTORESTRTIPOAMBIENTAL, Constantes.ACTUALIZARPROYECTOTLT,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (!proyecto.CodTipoProyecto.Equals(2))
            {
                if (proyecto.ConFinanciamiento)
                {
                    if (!ValidateRequiredFields())
                    {
                        if (lblTasaInteres.Foreground.Equals(Brushes.Red))
                        {
                            lblTasaInteres.Foreground = Brushes.Blue;
                        }

                        if (FinanciamientoSelected.CodFinanciamiento.Equals(0))
                        {
                            Financiamiento financiamientoTemp = financiamientoData.InsertarFinanciamiento(FinanciamientoSelected, proyecto.CodProyecto);
                            if (!financiamientoTemp.CodFinanciamiento.Equals(-1))
                            {
                                //success
                                FinanciamientoSelected = financiamientoTemp;
                                LocalContext.ReloadUserControls(CodProyecto, Modulo.Financiamiento);
                                MessageBox.Show(Constantes.INSERTARFINANMSG, Constantes.ACTUALIZARPROYECTOTLT,
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                //error
                                MessageBox.Show(Constantes.INSERTARFINANERROR, Constantes.ACTUALIZARPROYECTOTLT,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            if (financiamientoData.EditarFinanciamiento(FinanciamientoSelected, CodProyecto))
                            {
                                //success
                                LocalContext.ReloadUserControls(CodProyecto, Modulo.Financiamiento);
                                MessageBox.Show(Constantes.ACTUALIZARFINANMSG, Constantes.ACTUALIZARPROYECTOTLT,
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                //error
                                MessageBox.Show(Constantes.ACTUALIZARFINANERROR, Constantes.ACTUALIZARPROYECTOTLT,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(Constantes.ACTUALIZARPROYECTORESTRTIPOAMBIENTAL, Constantes.ACTUALIZARPROYECTOTLT,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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

            if(DTFinanciamiento.Table == null)
            {
                lblTasaInteres.Foreground = Brushes.Red;
                result = true;
            }
            return result;
        }

        private void cbxAnosDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxAnosDisponibles.IsLoaded)
            {
                ActualizarDTFinanciamiento();
                CargaDesdeCombos = true;
            }
        }

        private void cbInteresFijo_Checked(object sender, RoutedEventArgs e)
        {
            ActualizarDTFinanciamiento();
        }

        private void cbInteresFijo_Unchecked(object sender, RoutedEventArgs e)
        {
            ActualizarDTFinanciamiento();
        }

        bool tbMontotxtChngEvent = true;
        private void tbMonto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbMonto.Text.Equals("0") || tbMonto.Text.Equals("0.00"))
            {
                tbMontotxtChngEvent = false;
                tbMonto.Text = "";
            }
        }

        private void tbMonto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbMonto.Text.Equals(""))
            {
                tbMontotxtChngEvent = false;
                tbMonto.Text = "0.00";
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
                    ActualizarDTFinanciamiento();
                }
                CargaDesdeCombos = true;
            }
        }
    }
}
