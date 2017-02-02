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
    public partial class IndicadoresDialog : Form
    {
        public IndicadoresDialog()
        {
            InitializeComponent();
        }

        private void LlenaVANDivide()
        {
            tbxVANPersonas.Text = "₡" + (Convert.ToDouble(tbxVAN.Text.Replace("₡ ", string.Empty).ToString()) /
                Convert.ToDouble(nudPersonasParticipantes.Value)).ToString("#,##0.##");

            tbxVANFamilias.Text = "₡" + (Convert.ToDouble(tbxVAN.Text.Replace("₡ ", string.Empty).ToString()) /
                Convert.ToDouble(nudFamiliasInvolucradas.Value)).ToString("#,##0.##");
        }
    }
}
