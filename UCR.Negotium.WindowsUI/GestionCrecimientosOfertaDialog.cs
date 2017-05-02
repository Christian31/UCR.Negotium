using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class GestionCrecimientosOfertaDialog : Form
    {
        private Proyecto proyecto;
        private Encargado evaluador;
        private CrecimientoOfertaObjetoInteresData crecimientoOfertaData;
        public GestionCrecimientosOfertaDialog(Encargado evaluador, Proyecto proyecto)
        {
            InitializeComponent();
            this.proyecto = proyecto;
            this.evaluador = evaluador;
            this.LlenaDgvCrecimientoOferta();
            this.crecimientoOfertaData = new CrecimientoOfertaObjetoInteresData();
        }

        private void btnGuardarCrecimientos_Click(object sender, EventArgs e)
        {
            int validaInsersion = 1;
            List<CrecimientoOfertaObjetoInteres> listaCrecimientos = new List<CrecimientoOfertaObjetoInteres>();
            List<CrecimientoOfertaObjetoInteres> listaCrecimientosPersistente = new List<CrecimientoOfertaObjetoInteres>();
            DataTable dt = crecimientoOfertaData.GetCrecimientoOfertaObjetoIntereses(this.proyecto.CodProyecto);

            if (dt == null || dt.Rows.Count == 0)
            {
                //aplicar insert
                for (int i = 0; i < dgvCrecimientosOferta.RowCount; i++)
                {
                    if (Convert.ToInt32(this.dgvCrecimientosOferta.Rows[i].Cells["Porcentaje"].Value) > 0)
                    {
                        try
                        {
                            CrecimientoOfertaObjetoInteres crecimientoOferta = new CrecimientoOfertaObjetoInteres();
                            crecimientoOferta.AnoCrecimiento =
                                Int32.Parse(this.dgvCrecimientosOferta.Rows[i].Cells["Año"].Value.ToString());
                            crecimientoOferta.PorcentajeCrecimiento =
                                Int32.Parse(this.dgvCrecimientosOferta.Rows[i].Cells["Porcentaje"].Value.ToString());

                            crecimientoOfertaData.InsertarCrecimientoOfertaObjetoIntereses(crecimientoOferta, this.proyecto.CodProyecto);
                            validaInsersion = 1;
                            listaCrecimientos.Add(crecimientoOferta);

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
                    if (crecimientoOfertaData.eliminarCrecimientoObjetoInteres(this.proyecto.CodProyecto))
                    {
                        for (int i = 0; i < dgvCrecimientosOferta.RowCount; i++)
                        {
                            if (Convert.ToInt32(this.dgvCrecimientosOferta.Rows[i].Cells["Porcentaje"].Value) > 0)
                            {

                                CrecimientoOfertaObjetoInteres crecimientoOferta = new CrecimientoOfertaObjetoInteres();
                                crecimientoOferta.AnoCrecimiento =
                                    Int32.Parse(this.dgvCrecimientosOferta.Rows[i].Cells["Año"].Value.ToString());
                                crecimientoOferta.PorcentajeCrecimiento =
                                    Int32.Parse(this.dgvCrecimientosOferta.Rows[i].Cells["Porcentaje"].Value.ToString());

                                crecimientoOfertaData.InsertarCrecimientoOfertaObjetoIntereses(crecimientoOferta, this.proyecto.CodProyecto);
                                validaInsersion = 1;
                                listaCrecimientos.Add(crecimientoOferta);


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
                proyecto.CrecimientosAnuales = listaCrecimientos;
                MessageBox.Show("Porcentajes de crecimiento registrados con éxito",
                            "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }//if
            else if (validaInsersion == 2)
            {
                MessageBox.Show("Los porcentajes de crecimiento no se han podido registrar",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//else
        }

        private void LlenaDgvCrecimientoOferta()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("CrecimientoOferta");
            ds.Tables["CrecimientoOferta"].Columns.Add("Año", Type.GetType("System.String"));
            ds.Tables["CrecimientoOferta"].Columns.Add("Porcentaje", Type.GetType("System.String"));

            if (this.proyecto.CrecimientosAnuales != null && this.proyecto.CrecimientosAnuales.Count > 0)
            {

                for (Int32 i = 0; i < proyecto.CrecimientosAnuales.Count; i++)
                {
                    DataRow row = ds.Tables["CrecimientoOferta"].NewRow();
                    row["Año"] = proyecto.CrecimientosAnuales[i].AnoCrecimiento;
                    row["Porcentaje"] = proyecto.CrecimientosAnuales[i].PorcentajeCrecimiento;
                    ds.Tables["CrecimientoOferta"].Rows.Add(row);
                }//foreach
            }//if

            else
            {
                List<String> anosCrecimiento = new List<String>();
                for (int i = 2; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    int anoActual = proyecto.AnoInicial + i;
                    anosCrecimiento.Add(anoActual.ToString());
                }//for

                foreach (String anoIver in anosCrecimiento)
                {
                    DataRow row = ds.Tables["CrecimientoOferta"].NewRow();
                    row["Año"] = anoIver;
                    row["Porcentaje"] = 0;
                    ds.Tables["CrecimientoOferta"].Rows.Add(row);
                }
            }
            DataTable dtCrecimientos = ds.Tables["CrecimientoOferta"];
            dgvCrecimientosOferta.DataSource = dtCrecimientos;
            dgvCrecimientosOferta.Columns[0].ReadOnly = true;
            dgvCrecimientosOferta.Columns[1].HeaderText = "Porcentaje de crecimiento";

        }//LlenaDGVCrecimientoOfertas

        private void btnCancelarCrecimientos_Click(object sender, EventArgs e)
        {
            new RegistrarProyectoWindow(this.evaluador, this.proyecto, 5)
            {
                MdiParent = base.MdiParent
            }.Show();
            base.Close();
        }

        private void dgvCrecimientosOferta_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            List<int> intList = new List<int>(new int[] { 1 });
            string valueToValidate = string.Empty;
            bool isInList = false;
            if (e.RowIndex >= 0)
            {
                valueToValidate = dgvCrecimientosOferta[e.ColumnIndex, e.RowIndex].Value.ToString();
                isInList = intList.IndexOf(e.ColumnIndex) != -1;
                bool isValid = isInList ? ValidaNumeros(valueToValidate) : true;

                if (!isValid)
                {
                    dgvCrecimientosOferta[e.ColumnIndex, e.RowIndex].Value = 0;
                    MessageBox.Show("Los datos ingresados son inválidos en ese campo",
                                "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

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
    }
}
