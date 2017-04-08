using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCR.Negotium.WindowsUI
{
    public partial class RegistrarProyectoWindow : Form
    {
        #region InformacionGeneral
        public bool ValidarInformacionGeneral(bool isCleaning = false)
        {
            bool errorEncontrado = false;
            if (!isCleaning)
            {
                if (string.IsNullOrWhiteSpace(txbNombreProyecto.Text))
                {
                    lblNombreProyectoError.Visible = errorEncontrado = true;
                }
                if (string.IsNullOrWhiteSpace(txbObjetoInteres.Text))
                {
                    lblObjetoInteresError.Visible = errorEncontrado = true;
                }
                //if (string.IsNullOrWhiteSpace(txbDireccionExacta.Text))
                //{
                //    lblDireccionExactaError.Visible = errorEncontrado = true;
                //}
                //if (string.IsNullOrWhiteSpace(txbResumenEjecutivo.Text))
                //{
                //    lblResumenEjecutivoError.Visible = errorEncontrado = true;
                //}

                if (errorEncontrado)
                {
                    MessageBox.Show("Favor inserte todos los datos requeridos", "Datos Requeridos",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                lblNombreProyectoError.Visible = lblObjetoInteresError.Visible = errorEncontrado; //lblDireccionExactaError.Visible = lblResumenEjecutivoError.Visible = errorEncontrado;
            }

            return errorEncontrado;
        }
        #endregion

        #region Proponente
        private bool ValidarProponente(bool isCleaning = false)
        {
            bool errorEncontrado = false;
            if (!isCleaning)
            {
                if (string.IsNullOrWhiteSpace(txbCedulaProponente.Text) || !txbCedulaProponente.MaskCompleted)
                {
                    lblCedulaRepresentanteError.Visible = errorEncontrado = true;
                }
                if (string.IsNullOrWhiteSpace(txbCedulaJuridica.Text))
                {
                    lblCedulaJuridicaError.Visible = errorEncontrado = true;
                }
                if (!txbTelefonoProponente.MaskCompleted)
                {
                    lblTelefonoProponenteError.Visible = errorEncontrado = true;
                }
                if (!txbTelefonoOrganizacion.MaskCompleted)
                {
                    lblTelefonoOrganizacionError.Visible = errorEncontrado = true;
                }

                if (errorEncontrado)
                {
                    MessageBox.Show("Favor inserte todos los datos requeridos", "Datos Requeridos",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                lblTelefonoOrganizacionError.Visible = lblTelefonoProponenteError.Visible = lblCedulaRepresentanteError.Visible = lblCedulaJuridicaError.Visible = errorEncontrado;
            }

            return errorEncontrado;
        }
        #endregion

        #region caracterizacion
        #endregion

        #region tables
        private bool ValidaNumeros(string valor)
        {
            double n;
            if (Double.TryParse(valor, out n))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
