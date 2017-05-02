﻿//@Copyright Yordan Campos Piedra
using System;
using System.Windows.Forms;
using System.Net.Mail;
using UCR.Negotium.DataAccess;

namespace UCR.Negotium.WindowsUI
{
    public partial class OlvidoContrasenaWindow : Form
    {
        //Gmail: smtp.gmail.com puerto:587
        SmtpClient server = new SmtpClient("smtp.gmail.com", 587);

        public OlvidoContrasenaWindow()
        {
            InitializeComponent();
            server.Credentials = new System.Net.NetworkCredential("negotiumucr@gmail.com", "Negotium2015");
            server.EnableSsl = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EncargadoData evaluadorData = new EncargadoData();
            //Si el evaluador es null entonces es porque no está en la base de datos.
            if (evaluadorData.GetEncargados(txbCorreo.Text) != null)
            {
                String password = evaluadorData.GetEncargados(txbCorreo.Text).Password;
                EnviarCorreo(txbCorreo.Text, password);
            }//if
            else
            {
                MessageBox.Show("El correo especificado no pertenece a ningún usuario"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//else
        }

        private void EnviarCorreo(String correoDestino, String password)
        {
            try
            {
                MailMessage mensaje = new MailMessage();
                mensaje.Subject = "Recuperar contraseña Negotium";
                mensaje.Body = "Tu contraseña es: "+ password;
                mensaje.To.Add(new MailAddress(correoDestino));
                mensaje.From = new MailAddress("negotiumucr@gmail.com", "Negotium");
                server.Send(mensaje);
                MessageBox.Show("El correo se ha enviado correctamente", 
                    "Enviado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error a la hora de enviar su correo, "+
                    "por favor, verifique su conexión a Internet", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
