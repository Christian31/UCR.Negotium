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
    public partial class GestionVariacionCostosDialog : Form
    {
        private Proyecto proyecto;
        private Evaluador evaluador;
        private VariacionAnualCostoData variacionAnualCostoData;
        public GestionVariacionCostosDialog(Evaluador evaluador, Proyecto proyecto)
        {
            InitializeComponent();
            this.proyecto = proyecto;
            this.evaluador = evaluador;
            variacionAnualCostoData = new VariacionAnualCostoData();
            LlenaDgvVariacionCostos();
        }

        private void btnGuardarVariacion_Click(object sender, EventArgs e)
        {
            int validaInsersion = 1;
            List<VariacionAnualCosto> listaVariaciones = new List<VariacionAnualCosto>();
            List<VariacionAnualCosto> listaVariacionesPersistente = new List<VariacionAnualCosto>();
            VariacionAnualCostoData variacionCostoData = new VariacionAnualCostoData();
            DataTable dt = variacionCostoData.GetVariacionAnualCostos(this.proyecto.CodProyecto);

            if (dt == null || dt.Rows.Count == 0)
            {
                //aplicar insert
                for (int i = 0; i < dgvVariacionCostos.RowCount; i++)
                {
                    if (Convert.ToInt32(this.dgvVariacionCostos.Rows[i].Cells["Porcentaje"].Value) > 0)
                    {
                        try
                        {
                            VariacionAnualCosto variacionAnual = new VariacionAnualCosto();
                            variacionAnual.Ano =
                                Int32.Parse(this.dgvVariacionCostos.Rows[i].Cells["Año"].Value.ToString());
                            variacionAnual.PorcentajeIncremento =
                                Int32.Parse(this.dgvVariacionCostos.Rows[i].Cells["Porcentaje"].Value.ToString());

                            variacionCostoData.InsertarVariacionAnualCosto(variacionAnual, this.proyecto.CodProyecto);
                            validaInsersion = 1;
                            listaVariaciones.Add(variacionAnual);

                        }//try
                        catch (Exception ex)
                        {
                            validaInsersion = 2;
                            Console.WriteLine(ex);
                        }//catch
                    }//if
                }//for
            }//if
            else
            {
                //editar TODO cambiar el eliminar/insertar y usar el editar
                try
                {
                    if (variacionCostoData.eliminarVariacionAnualCostos(this.proyecto.CodProyecto))
                    {
                        for (int i = 0; i < dgvVariacionCostos.RowCount; i++)
                        {
                            if (Convert.ToInt32(this.dgvVariacionCostos.Rows[i].Cells["Porcentaje"].Value) > 0)
                            {

                                VariacionAnualCosto variacionAnual = new VariacionAnualCosto();
                                variacionAnual.Ano =
                                    Int32.Parse(this.dgvVariacionCostos.Rows[i].Cells["Año"].Value.ToString());
                                variacionAnual.PorcentajeIncremento =
                                    Int32.Parse(this.dgvVariacionCostos.Rows[i].Cells["Porcentaje"].Value.ToString());

                                variacionCostoData.InsertarVariacionAnualCosto(variacionAnual, this.proyecto.CodProyecto);
                                validaInsersion = 1;
                                listaVariaciones.Add(variacionAnual);

                            }//if
                        }//for
                    }//if
                }//try
                catch (Exception ex)
                {
                    validaInsersion = 2;
                    Console.WriteLine(ex);
                }//catch
            }//else
            if (validaInsersion == 1)
            {
                proyecto.VariacionCostos = listaVariaciones;
                MessageBox.Show("Porcentajes de incremento registrados con éxito",
                            "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }//if
            else if (validaInsersion == 2)
            {
                MessageBox.Show("Los porcentajes de incremento no se han podido registrar",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//else
        }

        private void btnVolverVariacionAnual_Click(object sender, EventArgs e)
        {
            new RegistrarProyectoWindow(this.evaluador, this.proyecto, 6)
            {
                MdiParent = base.MdiParent
            }.Show();
            base.Close();
        }

        private void LlenaDgvVariacionCostos()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("VariacionCosto");
            ds.Tables["VariacionCosto"].Columns.Add("Año", Type.GetType("System.String"));
            ds.Tables["VariacionCosto"].Columns.Add("Porcentaje", Type.GetType("System.String"));

            if (this.proyecto.VariacionCostos != null && this.proyecto.VariacionCostos.Count > 0)
            {

                for (Int32 i = 0; i < proyecto.VariacionCostos.Count; i++)
                {
                    DataRow row = ds.Tables["VariacionCosto"].NewRow();
                    row["Año"] = proyecto.VariacionCostos[i].Ano;
                    row["Porcentaje"] = proyecto.VariacionCostos[i].PorcentajeIncremento;
                    ds.Tables["VariacionCosto"].Rows.Add(row);
                }//foreach
            }//if

            else
            {
                List<String> anosCrecimiento = new List<String>();
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    int anoActual = proyecto.AnoInicial + i;
                    anosCrecimiento.Add(anoActual.ToString());
                }//for

                foreach (String anoIver in anosCrecimiento)
                {
                    DataRow row = ds.Tables["VariacionCosto"].NewRow();
                    row["Año"] = anoIver;
                    row["Porcentaje"] = 0;
                    ds.Tables["VariacionCosto"].Rows.Add(row);
                }
            }
            DataTable dtCostos = ds.Tables["VariacionCosto"];
            dgvVariacionCostos.DataSource = dtCostos;
            dgvVariacionCostos.Columns[0].ReadOnly = true;
            dgvVariacionCostos.Columns[1].HeaderText = "Porcentaje de incremento";

        }//LlenaDgvVariacionCostos
    }
}
