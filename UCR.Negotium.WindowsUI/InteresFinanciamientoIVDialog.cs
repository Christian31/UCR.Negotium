using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class InteresFinanciamientoIVDialog : Form
    {
        private Proyecto proyecto;
        private Encargado evaluador;
        private int tiempoFinanciamiento;
        private InteresFinanciamientoData interesFinanciamientoData; 
        public InteresFinanciamientoIVDialog(Encargado evaluador, Proyecto proyecto)
        {
            this.proyecto = proyecto;
            this.evaluador = evaluador;

            tiempoFinanciamiento = proyecto.FinanciamientoIV.TiempoFinanciamiento;
            interesFinanciamientoData = new InteresFinanciamientoData();
            InitializeComponent();
            LlenaDgvInteresFinanciamiento();
        }

        private void LlenaDgvInteresFinanciamiento()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("InteresFinanciamiento");
            ds.Tables["InteresFinanciamiento"].Columns.Add("Codigo", Type.GetType("System.String"));
            ds.Tables["InteresFinanciamiento"].Columns.Add("Año", Type.GetType("System.String"));
            ds.Tables["InteresFinanciamiento"].Columns.Add("Porcentaje", Type.GetType("System.String"));

            if (this.proyecto.InteresesFinanciamientoIV != null && this.proyecto.InteresesFinanciamientoIV.Count > 0)
            {

                for (Int32 i = 0; i < proyecto.InteresesFinanciamientoIV.Count; i++)
                {
                    DataRow row = ds.Tables["InteresFinanciamiento"].NewRow();
                    row["Codigo"] = proyecto.InteresesFinanciamientoIV[i].CodInteresFinanciamiento;
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
                else
                {
                    DataRow row = ds.Tables["InteresFinanciamiento"].NewRow();
                    row["Año"] = "Año General";
                    row["Porcentaje"] = 0;
                    ds.Tables["InteresFinanciamiento"].Rows.Add(row);
                }
            }
            DataTable dtIntereses = ds.Tables["InteresFinanciamiento"];
            dgvInteresesFinanciamiento.DataSource = dtIntereses;
            dgvInteresesFinanciamiento.Columns[0].Visible = false;
            dgvInteresesFinanciamiento.Columns[1].ReadOnly = true;
            dgvInteresesFinanciamiento.Columns[2].HeaderText = "Porcentaje de interés";

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
                try
                {
                    //if (interesFinanciamientoData.eliminarCrecimientoObjetoInteres(this.proyecto.CodProyecto))
                    //{
                    for (int i = 0; i < dgvInteresesFinanciamiento.RowCount; i++)
                    {
                        if (Convert.ToInt32(this.dgvInteresesFinanciamiento.Rows[i].Cells["Porcentaje"].Value) > 0)
                        {
                            InteresFinanciamiento interesIV = new InteresFinanciamiento();
                            interesIV.CodInteresFinanciamiento =
                            Int32.Parse(this.dgvInteresesFinanciamiento.Rows[i].Cells["Porcentaje"].Value.ToString());

                            interesIV.PorcentajeInteresFinanciamiento =
                                Int32.Parse(this.dgvInteresesFinanciamiento.Rows[i].Cells["Porcentaje"].Value.ToString());

                            if (interesFinanciamientoData.ActualizarInteresFinanciamiento(interesIV))
                            {
                                validaInsersion = 1;
                                listaCrecimientos.Add(interesIV);
                            }
                            else
                            {
                                validaInsersion = 2;
                            }
                        }//if
                    }//for
                    //}//if
                }//try
                catch (Exception ex)
                {
                    validaInsersion = 2;
                    Console.WriteLine(ex);
                }//catch
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
            new RegistrarProyectoWindow(evaluador, proyecto, 9)
            {
                MdiParent = base.MdiParent
            }.Show();
            Close();
        }

        private void dgvInteresesFinanciamiento_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            List<int> intList = new List<int>(new int[] { 2 });
            string valueToValidate = string.Empty;
            bool isInList = false;
            if (e.RowIndex >= 0)
            {
                valueToValidate = dgvInteresesFinanciamiento[e.ColumnIndex, e.RowIndex].Value.ToString();
                isInList = intList.IndexOf(e.ColumnIndex) != -1;
                bool isValid = isInList ? ValidaNumeros(valueToValidate) : true;

                if (!isValid)
                {
                    dgvInteresesFinanciamiento[e.ColumnIndex, e.RowIndex].Value = 0;
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
