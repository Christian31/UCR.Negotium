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
    public partial class AgregarCosto : Form
    {
        private Evaluador evaluador;
        private Proyecto proyecto;
        private Costo costoNuevo;
        private CostoMensual costoMensualNuevo;
        private CostoData costoData;

        public AgregarCosto(Evaluador evaluador, Proyecto proyecto)
        {
            this.evaluador = evaluador;
            costoNuevo = new Costo();
            InitializeComponent();
            this.proyecto = proyecto;
            this.LlenaDgvCosto();
            this.LlenarComboUnidadMedida();
            this.costoData = new CostoData();
        }

        private void LlenaDgvCosto()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("Costo");
            ds.Tables["Costo"].Columns.Add("Mes", Type.GetType("System.String"));
            ds.Tables["Costo"].Columns.Add("Cantidad", Type.GetType("System.String"));
            ds.Tables["Costo"].Columns.Add("Precio", Type.GetType("System.String"));

            string[] meses = new string[] { "Enero", "Febrero", "Marzo","Abril","Mayo","Junio","Julio","Agosto",
                "Setiembre","Octubre","Noviembre", "Diciembre" };
            for (Int32 i = 0; i < 12; i++)
            {
                DataRow row = ds.Tables["Costo"].NewRow();
                row["mes"] = meses[i];
                row["cantidad"] = 0;
                row["precio"] = 0;
                ds.Tables["Costo"].Rows.Add(row);
            }
            DataTable dtCosto = ds.Tables["Costo"];
            this.dgvCosto.DataSource = dtCosto;
        }

        private void btnAgregarCosto_Click(object sender, EventArgs e)
        {
            try
            {
                costoNuevo.NombreCosto = tbxNombreCosto.Text;
                costoNuevo.UnidadMedida.CodUnidad = Convert.ToInt32(cbxUnidadCosto.SelectedValue);
                costoNuevo.UnidadMedida.NombreUnidad = cbxUnidadCosto.Text;
                costoNuevo.Categoria_costo = cbxCategoriasCosto.Text;

                Int32 mes = 1;
                foreach (DataGridViewRow row in dgvCosto.Rows)
                {
                    costoMensualNuevo = new CostoMensual();
                    costoMensualNuevo.Mes = mes;
                    costoMensualNuevo.Cantidad = Convert.ToDouble(row.Cells[1].Value.ToString());
                    costoMensualNuevo.CostoUnitario = Convert.ToDouble(row.Cells[2].Value.ToString());
                    costoNuevo.CostosMensuales.Add(costoMensualNuevo);

                    mes++;
                }

                proyecto.Costos.Add(costoData.InsertarCosto(costoNuevo, this.proyecto.CodProyecto));

                new RegistrarProyecto(this.evaluador, this.proyecto, 6)
                {
                    MdiParent = base.MdiParent
                }.Show();
                base.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void LlenarComboUnidadMedida()
        {
            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            cbxUnidadCosto.DataSource = unidadMedidaData.GetUnidadesMedida();
            cbxUnidadCosto.DisplayMember = "nombre_unidad";
            cbxUnidadCosto.ValueMember = "cod_unidad";

            cbxUnidadCosto.SelectedIndex = 0;
            cbxCategoriasCosto.SelectedIndex = 0;
        }

        private void btnCancelarCosto_Click(object sender, EventArgs e)
        {
            new RegistrarProyecto(this.evaluador, this.proyecto, 6)
            {
                MdiParent = base.MdiParent
            }.Show();
            base.Close();
        }
    }
}
