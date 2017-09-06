using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarFactorAmbiental.xaml
    /// </summary>
    public partial class RegistrarFactorAmbiental : MetroWindow, INotifyPropertyChanged
    {
        private const string CAMPOREQUERIDO = "Este campo es requerido";

        private FactorAmbiental factorAmbientalSelected;
        private List<Tuple<int, string, List<Tuple<int, string>>>> condicionesAfectadas;
        private List<Tuple<int, string>> elementosAmbientales;
        private List<Tuple<int, string>> intensidades;
        private List<Tuple<int, string>> extensiones;
        private List<Tuple<int, string>> momentos;
        private List<Tuple<int, string>> persistencias;
        private List<Tuple<int, string>> reversibilidades;
        private List<Tuple<int, string>> sinergias;
        private List<Tuple<int, string>> acumulaciones;
        private List<Tuple<int, string>> efectos;
        private List<Tuple<int, string>> periodicidades;
        private List<Tuple<int, string>> recuperabilidades;
        private int codProyecto;

        private FactorAmbientalData factorAmbientalData = new FactorAmbientalData();

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public RegistrarFactorAmbiental(int codProyecto, int codFactor=0)
        {
            InitializeComponent();
            DataContext = this;
            tbNombreFactor.ToolTip = "Ingrese en este campo el Nombre del Factor que desea registrar";
            this.codProyecto = codProyecto;

            factorAmbientalSelected = new FactorAmbiental();
            condicionesAfectadas = new List<Tuple<int, string, List<Tuple<int, string>>>>();
            recuperabilidades = periodicidades = efectos = acumulaciones = sinergias = reversibilidades 
                = persistencias = momentos = extensiones = intensidades = elementosAmbientales 
                = new List<Tuple<int, string>>();


            factorAmbientalData = new FactorAmbientalData();

            CondicionesAfectadas = factorAmbientalData.GetCondicionesAfectadas();
            Intensidades = factorAmbientalData.GetIntensidades();
            Extensiones = factorAmbientalData.GetExtensiones();
            Momentos = factorAmbientalData.GetMomentos();
            Persistencias = factorAmbientalData.GetPersistencias();
            Reversibilidades = factorAmbientalData.GetReversibilidades();
            Sinergias = factorAmbientalData.GetSinergias();
            Acumulaciones = factorAmbientalData.GetAcumulaciones();
            Efectos = factorAmbientalData.GetEfectos();
            Periodicidades = factorAmbientalData.GetPeriodicidades();
            Recuperabilidades = factorAmbientalData.GetRecuperabilidades();

            if (!codFactor.Equals(0))
            {
                FactorAmbientalSelected = factorAmbientalData.GetFactor(codFactor);
                ElementosAmbientales = CondicionesAfectadas.Find(cond => cond.Item1.Equals(
                    FactorAmbientalSelected.CodCondicionAfectada)).Item3;
            }
            else
            {
                ElementosAmbientales = CondicionesAfectadas.FirstOrDefault().Item3;
                FactorAmbientalSelected.CodIntensidad = Intensidades.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodExtension = Extensiones.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodMomento = Momentos.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodPersistencia = Persistencias.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodReversibilidad = Reversibilidades.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodSinergia = Sinergias.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodAcumulacion = Acumulaciones.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodEfecto = Efectos.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodPeriodicidad = Periodicidades.FirstOrDefault().Item1;
                FactorAmbientalSelected.CodRecuperabilidad = Recuperabilidades.FirstOrDefault().Item1;
            }

            PropertyChanged(this, new PropertyChangedEventArgs("FactorAmbientalSelected"));
        }

        public List<Tuple<int, string>> Recuperabilidades
        {
            get
            {
                return recuperabilidades;
            }
            set
            {
                recuperabilidades = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Recuperabilidades"));
            }
        }

        public List<Tuple<int, string>> Periodicidades
        {
            get
            {
                return periodicidades;
            }
            set
            {
                periodicidades = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Periodicidades"));
            }
        }

        public List<Tuple<int, string>> Efectos
        {
            get
            {
                return efectos;
            }
            set
            {
                efectos = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Efectos"));
            }
        }

        public List<Tuple<int, string>> Acumulaciones
        {
            get
            {
                return acumulaciones;
            }
            set
            {
                acumulaciones = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Acumulaciones"));
            }
        }

        public List<Tuple<int, string>> Sinergias
        {
            get
            {
                return sinergias;
            }
            set
            {
                sinergias = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Sinergias"));
            }
        }

        public List<Tuple<int, string>> Reversibilidades
        {
            get
            {
                return reversibilidades;
            }
            set
            {
                reversibilidades = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Reversibilidades"));
            }
        }

        public List<Tuple<int, string>> Persistencias
        {
            get
            {
                return persistencias;
            }
            set
            {
                persistencias = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Persistencias"));
            }
        }

        public List<Tuple<int, string>> Momentos
        {
            get
            {
                return momentos;
            }
            set
            {
                momentos = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Momentos"));
            }
        }

        public List<Tuple<int, string>> Extensiones
        {
            get
            {
                return extensiones;
            }
            set
            {
                extensiones = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Extensiones"));
            }
        }

        public List<Tuple<int, string>> Intensidades
        {
            get
            {
                return intensidades;
            }
            set
            {
                intensidades = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Intensidades"));
            }
        }

        public bool Reload { get; set; }

        public List<Tuple<int, string, List<Tuple<int, string>>>> CondicionesAfectadas
        {
            get
            {
                return condicionesAfectadas;
            }
            set
            {
                condicionesAfectadas = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CondicionesAfectadas"));
            }
        }

        public List<Tuple<int, string>> ElementosAmbientales
        {
            get
            {
                return elementosAmbientales;
            }
            set
            {
                elementosAmbientales = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ElementosAmbientales"));
            }
        }

        public FactorAmbiental FactorAmbientalSelected
        {
            get
            {
                return factorAmbientalSelected;
            }
            set
            {
                factorAmbientalSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FactorAmbientalSelected"));
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                CalcularClasificacion();

                if (FactorAmbientalSelected.CodFactorAmbiental.Equals(0))
                {
                    FactorAmbientalSelected.CodProyecto = codProyecto;
                    FactorAmbiental factorTemp = factorAmbientalData.InsertarFactorAmbiental(FactorAmbientalSelected);
                    if (!factorTemp.CodFactorAmbiental.Equals(-1))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar el factor ambiental del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (factorAmbientalData.EditarFactorAmbiental(FactorAmbientalSelected))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar el factor ambiental del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cbCondicionesAfectadas_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbCondicionesAfectadas.IsLoaded)
            {
                ElementosAmbientales = CondicionesAfectadas.Find(cond => cond.Item1.Equals(FactorAmbientalSelected.CodCondicionAfectada)).Item3;
                cbElementosAmbientales.SelectedIndex = 0;
            }
        }

        private void tbNombreFactor_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tbNombreFactor.BorderBrush == Brushes.Red)
            {
                tbNombreFactor.BorderBrush = Brushes.Gray;
                tbNombreFactor.ToolTip = "Ingrese en este campo el Nombre del Factor que desea registrar";
            }
        }

        private bool ValidateRequiredFields()
        {
            if (string.IsNullOrWhiteSpace(tbNombreFactor.Text))
            {
                tbNombreFactor.ToolTip = CAMPOREQUERIDO;
                tbNombreFactor.BorderBrush = Brushes.Red;
                return true;
            }

            return false;
        }

        //Preguntar sobre signo y valor de importancia (+-)
        private void CalcularClasificacion()
        {
            List<int> values = new List<int>() { FactorAmbientalSelected.CodAcumulacion,
            FactorAmbientalSelected.CodEfecto, 2 * FactorAmbientalSelected.CodExtension,
            3 * FactorAmbientalSelected.CodIntensidad, FactorAmbientalSelected.CodMomento,
            FactorAmbientalSelected.CodPeriodicidad, FactorAmbientalSelected.CodPersistencia,
            FactorAmbientalSelected.CodRecuperabilidad, FactorAmbientalSelected.CodReversibilidad,
            FactorAmbientalSelected.CodSinergia };

            if (FactorAmbientalSelected.MomentoCritico)
            {
                values.Add(4);
            }
            if (FactorAmbientalSelected.ExtensionCritico)
            {
                values.Add(4);
            }

            FactorAmbientalSelected.CodClasificacion = (values.Sum() / 25);

        }
    }
}
