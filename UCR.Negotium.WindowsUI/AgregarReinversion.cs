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
    public partial class AgregarReinversion : Form
    {
        private Proyecto proyecto;
        private Evaluador evaluador;
        private List<RequerimientoReinversion> requerimientoReinversionList;

        public AgregarReinversion(Evaluador evaluador, Proyecto proyecto)
        {
            this.evaluador = new Evaluador();
            this.proyecto = new Proyecto();
            InitializeComponent();
            this.proyecto = proyecto;
            this.LlenaDgvReinversion();
            this.evaluador = evaluador;
            this.requerimientoReinversionList = new List<RequerimientoReinversion>();
            this.requerimientoReinversionList = proyecto.RequerimientosReinversion;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new RegistrarProyecto(this.evaluador, this.proyecto, 4)
            {
                MdiParent = base.MdiParent
            }.Show();
            base.Close();
        }

        private void LlenaDgvReinversion()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("RequerimientReinversion");
            ds.Tables["RequerimientReinversion"].Columns.Add("Codigo", Type.GetType("System.String"));
            ds.Tables["RequerimientReinversion"].Columns.Add("Descripcion", Type.GetType("System.String"));
            ds.Tables["RequerimientReinversion"].Columns.Add("Cantidad", Type.GetType("System.String"));
            ds.Tables["RequerimientReinversion"].Columns.Add("CostoUnitario", Type.GetType("System.String"));
            ds.Tables["RequerimientReinversion"].Columns.Add("VidaUtil", Type.GetType("System.String"));
            foreach (RequerimientoInversion requerimiento in this.proyecto.RequerimientosInversion)
            {
                if (requerimiento.Depreciable)
                {
                    DataRow row = ds.Tables["RequerimientReinversion"].NewRow();
                    row["Codigo"] = requerimiento.CodRequerimientoInversion;
                    row["Descripcion"] = requerimiento.DescripcionRequerimiento;
                    row["Cantidad"] = requerimiento.Cantidad;
                    row["CostoUnitario"] = requerimiento.CostoUnitario;
                    row["VidaUtil"] = requerimiento.VidaUtil;
                    ds.Tables["RequerimientReinversion"].Rows.Add(row);
                }
            }
            DataTable dtRequerimientos = ds.Tables["RequerimientReinversion"];
            this.dgvAgregaReinversion.DataSource = dtRequerimientos;
        }

        private void btnAgregarReinversion_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dgvAgregaReinversion.RowCount; i++)
                {
                    bool flag = Convert.ToBoolean(this.dgvAgregaReinversion.Rows[i].Cells[0].Value);
                    if (flag)
                    {
                        foreach (RequerimientoInversion requerimiento in this.proyecto.RequerimientosInversion)
                        {
                            bool flag2 = requerimiento.CodRequerimientoInversion == int.Parse(this.dgvAgregaReinversion.Rows[i].Cells[1].Value.ToString());
                            if (flag2)
                            {
                                RequerimientoReinversion requerimientoReinversion = new RequerimientoReinversion();
                                requerimientoReinversion.Cantidad = requerimiento.Cantidad;
                                requerimientoReinversion.CostoUnitario = requerimiento.CostoUnitario;
                                requerimientoReinversion.Depreciable = requerimiento.Depreciable;
                                requerimientoReinversion.DescripcionRequerimiento = requerimiento.DescripcionRequerimiento;
                                requerimientoReinversion.CodRequerimientoInversion = requerimiento.CodRequerimientoInversion;
                                requerimientoReinversion.UnidadMedida = requerimiento.UnidadMedida;
                                requerimientoReinversion.AnoReinversion = 2001;
                                this.requerimientoReinversionList.Add(requerimientoReinversion);
                            }
                        }
                    }
                }

                for (int i= 0; i<this.requerimientoReinversionList.Count; i++)
                {
                    RequerimientoReinversionData requereinvData = new RequerimientoReinversionData();
                    if (requerimientoReinversionList[i].CodRequerimientoReinversion.Equals(0))
                    {
                        requerimientoReinversionList[i] = requereinvData.InsertarRequerimientosReinversion(requerimientoReinversionList[i], this.proyecto.CodProyecto);
                    }
                }

                this.proyecto.RequerimientosReinversion = this.requerimientoReinversionList;
                new RegistrarProyecto(this.evaluador, this.proyecto, 4)
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
    }
}
