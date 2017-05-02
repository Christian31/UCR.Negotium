using System;
using System.Windows.Forms;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class IndicadoresDialog : Form
    {
        private Proyecto proyecto;
        private Encargado evaluador;
        private double TIR, VAN;

        public IndicadoresDialog(Encargado evaluador, Proyecto proyecto, double TIR, double VAN)
        {
            InitializeComponent();
            this.proyecto = proyecto;
            this.evaluador = evaluador;
            this.TIR = TIR;
            this.VAN = VAN;
            LLenaDatosIniciales();
        }

        private void LLenaDatosIniciales()
        {
            tbxTIR.Text = TIR.ToString()+ " %";
            tbxVAN.Text = ("₡ " + VAN.ToString("#,##0.##"));
            nudTasaCostoCapital.Value = Convert.ToDecimal(proyecto.TasaCostoCapital);
            nudPersonasParticipantes.Value = proyecto.PersonasParticipantes;
            nudFamiliasInvolucradas.Value = proyecto.FamiliasInvolucradas;
            nudBeneficiariosIndirectos.Value = proyecto.PersonasBeneficiadas;

            LLenaCalculosVAN();
        }

        private void nudPersonasParticipantes_ValueChanged(object sender, EventArgs e)
        {
            LLenaCalculosVAN();
        }

        private void nudFamiliasInvolucradas_ValueChanged(object sender, EventArgs e)
        {
            LLenaCalculosVAN();
        }

        private void nudBeneficiariosIndirectos_ValueChanged(object sender, EventArgs e)
        {
            LLenaCalculosVAN();
        }

        private void btnCancelarIndicadores_Click(object sender, EventArgs e)
        {
            new RegistrarProyectoWindow(this.evaluador, this.proyecto, 10)
            {
                MdiParent = base.MdiParent
            }.Show();
            Close();
        }

        private void btnGuardarIndicadores_Click(object sender, EventArgs e)
        {
            ProyectoData proyectoData = new ProyectoData();
            proyecto.TasaCostoCapital = Convert.ToDouble(nudTasaCostoCapital.Value);
            proyecto.PersonasParticipantes = Convert.ToInt32(nudPersonasParticipantes.Value);
            proyecto.FamiliasInvolucradas = Convert.ToInt32(nudFamiliasInvolucradas.Value);
            proyecto.PersonasBeneficiadas = Convert.ToInt32(nudBeneficiariosIndirectos.Value);
            if (proyectoData.ActualizarProyectoFlujoCaja(proyecto))
            {
                MessageBox.Show("Proyecto actualizado con éxito",
                        "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                new RegistrarProyectoWindow(evaluador, proyecto, 10)
                {
                    MdiParent = base.MdiParent
                }.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Error al actualizar el Proyecto",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LLenaCalculosVAN()
        {
            tbxVANPersonas.Text = "₡ " + (Convert.ToDouble(tbxVAN.Text.Replace("₡ ", string.Empty).ToString()) /
                Convert.ToDouble(nudPersonasParticipantes.Value)).ToString("#,##0.##");

            tbxVANFamilias.Text = "₡ " + (Convert.ToDouble(tbxVAN.Text.Replace("₡ ", string.Empty).ToString()) /
                Convert.ToDouble(nudFamiliasInvolucradas.Value)).ToString("#,##0.##");

            tbxVANBeneficiarios.Text = "₡ " + (Convert.ToDouble(tbxVAN.Text.Replace("₡ ", string.Empty).ToString()) /
                Convert.ToDouble(nudBeneficiariosIndirectos.Value)).ToString("#,##0.##");
        }
    }
}
