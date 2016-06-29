using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCR.Negotium.WindowsUI
{
    public partial class AgregarReinversion : Form
    {
        public AgregarReinversion()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new RegistrarProyecto(this.evaluador, this.proyecto)
            {
                MdiParent = base.MdiParent
            }.Show();
            base.Close();
        }
    }
}
