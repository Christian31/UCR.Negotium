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
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class VentanaPrincipal : Form
    {

        Evaluador evaluadorLogeado = new Evaluador();
        public VentanaPrincipal(Evaluador evaluador)
        {
            evaluadorLogeado = evaluador;
            InitializeComponent();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginWindow login = new LoginWindow();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        private void nuevoProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiChildren.Length < 1)
            {
                RegistrarProyectoWindow registrarProyecto = new RegistrarProyectoWindow(evaluadorLogeado);
                registrarProyecto.MdiParent = this;
                //registrarProyecto.WindowState = FormWindowState.Maximized;
                registrarProyecto.Size = new Size(1700, 590);
                registrarProyecto.Show();
            }
        }

        private void modificarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiChildren.Length < 1)
            {
                //Evaluador evaluador = new Evaluador(1, "Yordan", "apellidos", "72056090", "ycampospiedra@gmail.com", "3-0476-0003");
                ListaProyectosWindow selecciona = new ListaProyectosWindow(evaluadorLogeado);
                selecciona.MdiParent = this;
                selecciona.Show();
            }//if
        }
    }
}
