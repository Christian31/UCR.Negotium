using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class AgregarCostoDialog : Form
    {
        private Evaluador evaluador;
        private Proyecto proyecto;
        private Costo costoNuevo;
        private CostoMensual costoMensualNuevo;
        private CostoData costoData;

        public AgregarCostoDialog(Evaluador evaluador, Proyecto proyecto, int codCosto = 0)
        {
            this.evaluador = evaluador;
            costoNuevo = codCosto == 0 ? new Costo() : proyecto.Costos.Where(c => c.CodCosto == codCosto).First();
            InitializeComponent();
            this.proyecto = proyecto;
            this.LlenaDgvCosto();
            this.LlenarComboUnidadMedida();
            this.LlenarComboAnoInicial();
            this.costoData = new CostoData();

            if (codCosto != 0)
            {
                cbxUnidadCosto.SelectedValue = costoNuevo.UnidadMedida.CodUnidad;
                cbxCategoriasCosto.SelectedValue = costoNuevo.CategoriaCosto;
                cbxAnoInicialCosto.SelectedText = costoNuevo.AnoCosto.ToString();
                tbxNombreCosto.Text = costoNuevo.NombreCosto;
            }
        }

        private void LlenaDgvCosto()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("Costo");
            ds.Tables["Costo"].Columns.Add("Mes", Type.GetType("System.String"));
            ds.Tables["Costo"].Columns.Add("Cantidad", Type.GetType("System.Int32"));
            ds.Tables["Costo"].Columns.Add("Precio", Type.GetType("System.Int32"));

            string[] meses = new string[] { "Enero", "Febrero", "Marzo","Abril","Mayo","Junio","Julio","Agosto",
                "Setiembre","Octubre","Noviembre", "Diciembre" };

            if(costoNuevo.CodCosto == 0)
            {
                for (Int32 i = 0; i < 12; i++)
                {
                    DataRow row = ds.Tables["Costo"].NewRow();
                    row["mes"] = meses[i];
                    row["cantidad"] = 0;
                    row["precio"] = 0;
                    ds.Tables["Costo"].Rows.Add(row);
                }
            }
            else
            {
                for (Int32 i = 0; i < 12; i++)
                {
                    DataRow row = ds.Tables["Costo"].NewRow();
                    row["mes"] = meses[i];
                    row["cantidad"] = costoNuevo.CostosMensuales[i].Cantidad;
                    row["precio"] = costoNuevo.CostosMensuales[i].CostoUnitario;
                    ds.Tables["Costo"].Rows.Add(row);
                }
            }
            
            DataTable dtCosto = ds.Tables["Costo"];
            this.dgvCosto.DataSource = dtCosto;
            this.dgvCosto.Columns["Precio"].HeaderText = "Precio (₡)";
            this.dgvCosto.Columns["mes"].ReadOnly = true;
        }

        private void btnAgregarCosto_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbxNombreCosto.Text.Length >0)
                {
                    costoNuevo.NombreCosto = tbxNombreCosto.Text;
                    costoNuevo.UnidadMedida.CodUnidad = Convert.ToInt32(cbxUnidadCosto.SelectedValue);
                    costoNuevo.UnidadMedida.NombreUnidad = cbxUnidadCosto.Text;
                    costoNuevo.CategoriaCosto = cbxCategoriasCosto.Text;
                    costoNuevo.AnoCosto = Convert.ToInt32(cbxAnoInicialCosto.Text);

                    Int32 mes = 1;

                    if(costoNuevo.CodCosto == 0)
                    {
                        for (int i = 0; i < dgvCosto.Rows.Count; i++)
                        {
                            costoMensualNuevo = new CostoMensual();
                            costoMensualNuevo.Mes = mes;
                            costoMensualNuevo.Cantidad = Convert.ToDouble(dgvCosto.Rows[i].Cells[1].Value.ToString());
                            costoMensualNuevo.CostoUnitario = Convert.ToDouble(dgvCosto.Rows[i].Cells[2].Value.ToString());
                            costoNuevo.CostosMensuales.Add(costoMensualNuevo);

                            mes++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dgvCosto.Rows.Count; i++)
                        {
                            costoNuevo.CostosMensuales[i].Cantidad = Convert.ToDouble(dgvCosto.Rows[i].Cells[1].Value.ToString());
                            costoNuevo.CostosMensuales[i].CostoUnitario = Convert.ToDouble(dgvCosto.Rows[i].Cells[2].Value.ToString());

                            mes++;
                        }
                    }
                    
                    if (costoNuevo.CodCosto == 0)
                    {
                        proyecto.Costos.Add(costoData.InsertarCosto(costoNuevo, this.proyecto.CodProyecto));
                        MessageBox.Show("Costo insertado con éxito",
                        "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        new RegistrarProyectoWindow(this.evaluador, this.proyecto, 6)
                        {
                            MdiParent = base.MdiParent
                        }.Show();
                        base.Close();
                    }
                    else if (costoData.EditarCosto(costoNuevo, this.proyecto.CodProyecto))
                    {
                        MessageBox.Show("Costo actualizado con éxito",
                        "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        new RegistrarProyectoWindow(this.evaluador, this.proyecto, 6)
                        {
                            MdiParent = base.MdiParent
                        }.Show();
                        base.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el Costo",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("El nombre del Costo no puede ser vacío",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el Costo",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void LlenarComboAnoInicial()
        {
            for(int i =1; i <=this.proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                cbxAnoInicialCosto.Items.Add(this.proyecto.AnoInicial + i);
            }

            cbxAnoInicialCosto.SelectedIndex = 0;
        }

        private void btnCancelarCosto_Click(object sender, EventArgs e)
        {
            new RegistrarProyectoWindow(this.evaluador, this.proyecto, 6)
            {
                MdiParent = base.MdiParent
            }.Show();
            base.Close();
        }
    }
}
