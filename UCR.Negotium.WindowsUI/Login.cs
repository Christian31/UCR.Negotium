//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            lblEmail.BackColor = Color.Transparent;
            lblPass.BackColor = Color.Transparent;
            llbOlvidoContraseña.BackColor = Color.Transparent;       
        }

        private bool isOpen = false;

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            Evaluador evaluador = new Evaluador();
            EvaluadorData evaluadorData = new EvaluadorData();
            evaluador = evaluadorData.GetEvaluador(tbxCorreo.Text.ToString().Trim(),
                                                    tbxPass.Text.ToString().Trim());
            //Si existe el evaluador entonces que ingrese al sistema
            if (evaluador != null)
            {
                //El siguiente codigo es para abrir un nuevo form y cerrar
                //el form actual
                
                VentanaPrincipal ventanaPrincipal = new VentanaPrincipal(evaluador);
                this.Hide();
                ventanaPrincipal.ShowDialog();
                this.Close();
            }//if
            else
            {
               MessageBox.Show("Usuario incorrecto, por favor ingrese los datos nuevamente",
                   "Error en los datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbxPass.Text = String.Empty;
            }//else
        }//evento de btnIngresar

        private void button1_Click(object sender, EventArgs e)
        {
            Registrarse registrarse = new Registrarse();
            this.Hide();
            registrarse.ShowDialog();
            this.Close();
        }

        private void llbOlvidoContraseña_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OlvidoContrasena olvido = new OlvidoContrasena();
            olvido.ShowDialog();
        }

        private void Login_Activated(object sender, EventArgs e)
        {
            
        }
    }//Class Login
}
