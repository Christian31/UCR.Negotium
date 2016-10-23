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
    public partial class InteresFinanciamientoUI : Form
    {
        private Proyecto proyecto;
        private Evaluador evaluador;
        private int tiempoFinanciamiento;
        private InteresFinanciamientoData interesFinanciamientoData; 
        public InteresFinanciamientoUI(Evaluador evaluador, Proyecto proyecto)
        {
            this.proyecto = proyecto;
            this.evaluador = evaluador;

            this.tiempoFinanciamiento = proyecto.FinanciamientoIV.TiempoFinanciamiento;
            this.interesFinanciamientoData = new InteresFinanciamientoData();
            InitializeComponent();
            LlenaDgvInteresFinanciamiento();
        }

        private void LlenaDgvInteresFinanciamiento()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("InteresFinanciamiento");
            ds.Tables["InteresFinanciamiento"].Columns.Add("Año", Type.GetType("System.String"));
            ds.Tables["InteresFinanciamiento"].Columns.Add("Porcentaje", Type.GetType("System.String"));

            if (this.proyecto.InteresesFinanciamientoIV != null && this.proyecto.InteresesFinanciamientoIV.Count > 0)
            {
                
                    for (Int32 i = 0; i < proyecto.InteresesFinanciamientoIV.Count; i++)
                    {
                        DataRow row = ds.Tables["InteresFinanciamiento"].NewRow();
                        row["Año"] = "Año " + i;
                        row["Porcentaje"] = proyecto.InteresesFinanciamientoIV[i].PorcentajeInteresFinanciamiento;
                        ds.Tables["InteresFinanciamiento"].Rows.Add(row);
                    }//foreach
                
        }//if

            else
            {
                if (proyecto.InteresesFinanciamientoIV.Count == 0)
                {
                    List<String> anosFinanciamiento = new List<String>();
                    for (int i = 1; i <= tiempoFinanciamiento; i++)
                    {
                        DataRow row = ds.Tables["InteresFinanciamiento"].NewRow();
                        row["Año"] = "Año " + i;
                        row["Porcentaje"] = 0;
                        ds.Tables["InteresFinanciamiento"].Rows.Add(row);
                    }//for
                }
                else {
                    DataRow row = ds.Tables["InteresFinanciamiento"].NewRow();
                    row["Año"] = "Año General";
                    row["Porcentaje"] = 0;
                    ds.Tables["InteresFinanciamiento"].Rows.Add(row);
                }
            }
            DataTable dtIntereses = ds.Tables["InteresFinanciamiento"];
            dgvInteresesFinanciamiento.DataSource = dtIntereses;
            dgvInteresesFinanciamiento.Columns[0].ReadOnly = true;
            dgvInteresesFinanciamiento.Columns[1].HeaderText = "Porcentaje de interés";

        }//LlenaDgvInteresFinanciamiento

        private void btnGuardarInteres_Click(object sender, EventArgs e)
        {
            int validaInsersion = 1;
            List<InteresFinanciamiento> listaCrecimientos = new List<InteresFinanciamiento>();
            List<InteresFinanciamiento> listaCrecimientosPersistente = new List<InteresFinanciamiento>();

            if (interesFinanciamientoData.GetInteresesFinanciamiento(this.proyecto.CodProyecto, 1).Count == 0)
            {
                //aplicar insert
                for (int i = 0; i < dgvInteresesFinanciamiento.RowCount; i++)
                {
                    if (Convert.ToInt32(this.dgvInteresesFinanciamiento.Rows[i].Cells["Porcentaje"].Value) > 0)
                    {
                        try
                        {
                            InteresFinanciamiento interesFinanciamiento = new InteresFinanciamiento();
                            interesFinanciamiento.PorcentajeInteresFinanciamiento =
                                Int32.Parse(this.dgvInteresesFinanciamiento.Rows[i].Cells["Porcentaje"].Value.ToString());
                            interesFinanciamiento.VariableInteres = true;
                            interesFinanciamientoData.InsertarInteresFinanciamiento(interesFinanciamiento, this.proyecto.CodProyecto);
                            validaInsersion = 1;
                            listaCrecimientos.Add(interesFinanciamiento);

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
                //try
                //{
                //    if (interesFinanciamientoData.eliminarCrecimientoObjetoInteres(this.proyecto.CodProyecto))
                //    {
                //        for (int i = 0; i < dgvCrecimientosOferta.RowCount; i++)
                //        {
                //            if (Convert.ToInt32(this.dgvCrecimientosOferta.Rows[i].Cells["Porcentaje"].Value) > 0)
                //            {

                //                CrecimientoOfertaObjetoInteres crecimientoOferta = new CrecimientoOfertaObjetoInteres();
                //                crecimientoOferta.AnoCrecimiento =
                //                    Int32.Parse(this.dgvCrecimientosOferta.Rows[i].Cells["Año"].Value.ToString());
                //                crecimientoOferta.PorcentajeCrecimiento =
                //                    Int32.Parse(this.dgvCrecimientosOferta.Rows[i].Cells["Porcentaje"].Value.ToString());

                //                crecimientoOfertaData.InsertarCrecimientoOfertaObjetoIntereses(crecimientoOferta, this.proyecto.CodProyecto);
                //                validaInsersion = 1;
                //                listaCrecimientos.Add(crecimientoOferta);


                //            }//if
                //        }//for
                //    }//if
                //}//try
                //catch (Exception ex)
                //{
                //    validaInsersion = 2;
                //    Console.WriteLine(ex);
                //}//catch
            }//else
            if (validaInsersion == 1)
            {
                proyecto.InteresesFinanciamientoIV = listaCrecimientos;
                MessageBox.Show("Porcentajes de intereses registrados con éxito",
                            "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }//if
            else if (validaInsersion == 2)
            {
                MessageBox.Show("Los porcentajes de intereses no se han podido registrar",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//else
        }

        private void btnCancelarInteres_Click(object sender, EventArgs e)
        {
            new RegistrarProyecto(this.evaluador, this.proyecto, 9)
            {
                MdiParent = base.MdiParent
            }.Show();
            base.Close();
        }
    }
}
