using MahApps.Metro.Controls;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for ReferenciasNegotium.xaml
    /// </summary>
    public partial class ReferenciasNegotium : MetroWindow
    {
        public ReferenciasNegotium()
        {
            InitializeComponent();
            DataContext = this;

            descripcionReferencias.AppendText("Las funcionalidades, cálculos y demás fórmulas utilizados " +
                "y aplicados en Negotium fueron basados en las siguientes fuentes bibliográficas");

            referenciasList.Items.Add("Aguirre, J.A. (1985) Introducción a la evaluación económica y financiera de inversiones agropecuarias. Manual de instrucción programada. IICA. Coronado. 191 pag.");
            referenciasList.Items.Add("ITCR (Instituto Tecnológico de Costa Rica) 2005 Programa de Formación en espíritu emprendedor: Manual del estudiante, 3ra. Edición. ITCR, Escuela de Administración de Negocios. Cartago.");
            referenciasList.Items.Add("Leiva Bonilla, J.C. (2007) Los emprendedores y la creación de empresas. Edit.Tecnológica de Costa Rica. Cartago. 232 p.");
            referenciasList.Items.Add("Quirós, O. (2007) Gestión de la calidad e inocuidad en la agroindustria. Programa Formación de Técnicos en Administración Agrícola y Agroindustrial. Escuela de Economía Agrícola, Universidad de Costa Rica.");
            referenciasList.Items.Add("Quirós, O.; L. Temple (2008) Metodología general para la evaluación de la pre-factibilidad de innovaciones tecnológicas en procesamientos de frutas-Guía I. CIEDA-UCR/Cirad; UMR MOISA. Proyecto PAVUC.");
            referenciasList.Items.Add("Tennent, J.; G. Friend (2008) Cómo delinear un modelo de negocios. The Economist: Colección Finanzas y Negocios. 1 ed. Buenos Aires. 320 pag.");
        }
    }
}
