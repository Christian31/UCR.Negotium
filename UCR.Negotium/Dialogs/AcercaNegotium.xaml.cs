using MahApps.Metro.Controls;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for AcercaNegotium.xaml
    /// </summary>
    public partial class AcercaNegotium : MetroWindow
    {
        public AcercaNegotium()
        {
            InitializeComponent();
            DataContext = this;

            descripcionNegotium.AppendText("Negotium es una plataforma interactiva de apoyo a " +
                "la formulación de perfiles de proyectos para el desarrollo de territorios rurales.");

            caracteristicasList.Items.Add("Simplifica la planeación de proyectos en territorios rurales.");
            caracteristicasList.Items.Add("Puede ser utilizada en la gestión de un nuevo negocio, producto o servicio, o fortalecer la competitividad de negocios existentes.");
            caracteristicasList.Items.Add("Constituye un apoyo para los y las agroempresarias y organizaciones con ideas emprendedoras.");
            caracteristicasList.Items.Add("Permite realizar un análisis preliminar del impacto ambiental que pueda generar la inversión.");
        }
    }
}
