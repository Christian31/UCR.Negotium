//@Copyright Yordan Campos Piedra
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class RegistrarseWindow : Form
    {
        public RegistrarseWindow()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LoginWindow login = new LoginWindow();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        //Este metodo inserta a un evaluador en la base de datos
        private void RegistrarEvaluador(Encargado evaluador)
        {
            EncargadoData evaluadorData = new EncargadoData();
            //Si el evaluador fue insertado retorna true
            bool result = evaluadorData.InsertarEncargado(evaluador);
            if (result)
            {
                MessageBox.Show("Usuario registrado correctamente",
                   "Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoginWindow login = new LoginWindow();
                this.Hide();
                login.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error a la hora de ingresar al usuario",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//RegistrarEvaluador

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            //Antes de registrar al evaluador primero se valida que ningun campo este vacio
            if ((tbxEmail.Text.Trim().Equals("")) || (tbxNombre.Text.Trim().Equals("")) ||
                (tbxPassword.Text.Trim().Equals("")) || (tbxRePassword.Text.Trim().Equals("")) ||
                (tbxCedula.Text.Trim().Equals("")) || (tbxApellidos.Text.Trim().Equals("")) ||
                (tbxTelefono.Text.Trim().Equals("")) || (txbOrganizacion.Text.Trim().Equals("")))
            {
                MessageBox.Show("Por favor no deje ningún espacio en blanco",
                   "Error en el formulario", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//if
            else
            {
                //luego se valida que las contraseñas coincidan
                if (tbxPassword.Text.Equals(tbxRePassword.Text))
                {
                    //por ultimo se valida que el correo tenga un formato correcto
                    if (ValidaCorreo(tbxEmail.Text))
                    {
                        Encargado evaluador = new Encargado();
                        evaluador.Nombre = tbxNombre.Text;
                        evaluador.Apellidos = tbxApellidos.Text;
                        evaluador.Email = tbxEmail.Text;
                        evaluador.NumIdentificacion = tbxCedula.Text;
                        evaluador.Password = tbxPassword.Text;
                        evaluador.Telefono = tbxTelefono.Text;
                        evaluador.Organizacion = txbOrganizacion.Text;
                        RegistrarEvaluador(evaluador);
                    }//if
                    else
                    {
                        MessageBox.Show("Por favor ingrese un correo valido",
                        "Error en el correo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }//if
                else
                {
                    MessageBox.Show("Las contraseñas no coinciden, por favor ingreselas nuevamente",
                   "Error en la contraseña", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbxPassword.Text = String.Empty;
                    tbxRePassword.Text = String.Empty;
                }//else
            }//else
        }//accion del botón


        //El siguiente metodo valiada que el correo que ingresa el usuario
        //para registrarse tenga el formato correcto utilizando expresiones
        //regulares
        private bool ValidaCorreo(String correo)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(correo, expresion))
            {
                if (Regex.Replace(correo, expresion, String.Empty).Length == 0)
                {
                    return true;
                }//if
                else
                {
                    return false;
                }//else
            }//if
            else
            {
                return false;
            }//else
        }//validaCorreo
    }//Registrarse
}
