using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.Extensions;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarTipoMoneda.xaml
    /// </summary>
    public partial class RegistrarTipoMoneda : MetroWindow, INotifyPropertyChanged
    {
        private TipoMonedaData tipoMonedaData;
        private ProyectoData proyectoData;
        private int codProyecto;
        private TipoMoneda tipoMoneda;
        private List<TipoMoneda> tiposMonedas;

        public RegistrarTipoMoneda(int codProyecto)
        {
            InitializeComponent();
            DataContext = this;

            tipoMonedaData = new TipoMonedaData();
            proyectoData = new ProyectoData();
            tiposMonedas = new List<TipoMoneda>();

            this.codProyecto = codProyecto;
            TiposMonedas = tipoMonedaData.GetTiposMonedas();
            TipoMonedaSelected = proyectoData.GetMonedaProyecto(codProyecto);

            if (TipoMonedaSelected.CodMoneda.Equals(0))
            {
                TipoMonedaSelected.CodMoneda = 1;
            }
            PropertyChanged(this, new PropertyChangedEventArgs("TipoMonedaSelected"));
        }

        public bool Reload { get; set; }

        public bool PendingSave { get; set; }

        public List<TipoMoneda> TiposMonedas
        {
            get
            {
                return tiposMonedas;
            }
            set
            {
                tiposMonedas = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TiposMonedas"));
            }
        }

        public TipoMoneda TipoMonedaSelected
        {
            get { return tipoMoneda; }
            set { tipoMoneda = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void btnGuardar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (codProyecto.Equals(0))
            {
                PendingSave = true;
            }
            else 
            {
                if (LocalContext.SetMoneda(codProyecto, TipoMonedaSelected.CodMoneda))
                {
                    Reload = true;
                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error al actualizar la Moneda", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Close();
        }

        private void btnCancelar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}
