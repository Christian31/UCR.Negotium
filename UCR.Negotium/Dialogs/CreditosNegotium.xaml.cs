using MahApps.Metro.Controls;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for CreditosNegotium.xaml
    /// </summary>
    public partial class CreditosNegotium : MetroWindow
    {
        public CreditosNegotium()
        {
            InitializeComponent();
            DataContext = this;

            descripcionCreditos.AppendText("Los profesionales involucrados en el desarrollo de Negotium 1.2 tanto usuarios finales con tareas de validaciones y pruebas como directores y desarrolladores del proyecto son:");
            
            creditosList.Items.Add("MBA. Olga Calvo Hernández. Escuela de Economía Agrícola");
            creditosList.Items.Add("Bach. Yordan Campos Piedra. Informática Empresarial");
            creditosList.Items.Add("Bach. Christian Delgado Pérez. Informática Empresarial");
            creditosList.Items.Add("MGP. Álvaro Mena Monge. Coordinador Informática Empresarial");
            creditosList.Items.Add("Econ. Agríc. Luis Mora Noguera. Escuela de Economía Agrícola");
            creditosList.Items.Add("Dr. Olman Quiros Madrigal. Profesor Asociado, Escuela de Economía Agrícola y Agronegocios/ CIEDA");
            creditosList.Items.Add("Ing. Nelson Ramírez Sanchez. Escuela de Economía Agrícola");
        }
    }
}
