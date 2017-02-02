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
    public partial class AgregarProyeccionDialog : Form
    {
        private Evaluador evaluador;
        private Proyecto proyecto;
        private ProyeccionVentaArticulo proyeccionNueva;
        private DetalleProyeccionVenta detalleNuevo;
        private ProyeccionVentaArticuloData proyeccionData;
        public AgregarProyeccionDialog(Evaluador evaluador, Proyecto proyecto, int codProyeccion = 0)
        {
            this.evaluador = evaluador;
            proyeccionNueva = codProyeccion == 0 ? new ProyeccionVentaArticulo(): proyecto.Proyecciones.Where(p => p.CodArticulo == codProyeccion).First();
            
            InitializeComponent();
            this.proyecto = proyecto;
            this.LlenaDgvProyeccion();
            this.LlenarComboUnidadMedida();
            this.proyeccionData = new ProyeccionVentaArticuloData();

            if(codProyeccion != 0)
            {
                tbxNombreArticuloProyeccion.Text = proyeccionNueva.NombreArticulo;
                cbxUnidadProyeccion.SelectedValue = proyeccionNueva.UnidadMedida.CodUnidad;
            }
        }

        private void LlenaDgvProyeccion()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("Proyeccion");
            ds.Tables["Proyeccion"].Columns.Add("Mes", Type.GetType("System.String"));
            ds.Tables["Proyeccion"].Columns.Add("Cantidad", Type.GetType("System.String"));
            ds.Tables["Proyeccion"].Columns.Add("Precio", Type.GetType("System.String"));

            string[] meses = new string[] { "Enero", "Febrero", "Marzo","Abril","Mayo","Junio","Julio","Agosto",
                "Setiembre","Octubre","Noviembre", "Diciembre" };

            if(proyeccionNueva.CodArticulo == 0)
            {
                for (Int32 i = 0; i < 12; i++)
                {
                    DataRow row = ds.Tables["Proyeccion"].NewRow();
                    row["mes"] = meses[i];
                    row["cantidad"] = 0;
                    row["precio"] = 0;
                    ds.Tables["Proyeccion"].Rows.Add(row);
                }
            }
            else
            {
                for (Int32 i = 0; i < 12; i++)
                {
                    DataRow row = ds.Tables["Proyeccion"].NewRow();
                    row["mes"] = meses[i];
                    row["cantidad"] = proyeccionNueva.DetallesProyeccionVenta[i].Cantidad;
                    row["precio"] = proyeccionNueva.DetallesProyeccionVenta[i].Precio;
                    ds.Tables["Proyeccion"].Rows.Add(row);
                }
            }
            
            DataTable dtProyeccion = ds.Tables["Proyeccion"];
            this.dgvProyecciones.DataSource = dtProyeccion;
        }

        private void btnAgregarProyeccion_Click(object sender, EventArgs e)
        {
            if(proyeccionNueva.CodArticulo == 0)
            {
                GuardarProyeccion();
            }
            else
            {
                EditarProyeccion();
            }
        }

        private void GuardarProyeccion()
        {
            try
            {
                proyeccionNueva.NombreArticulo = tbxNombreArticuloProyeccion.Text;
                proyeccionNueva.UnidadMedida.CodUnidad = Convert.ToInt32(cbxUnidadProyeccion.SelectedValue);
                proyeccionNueva.UnidadMedida.NombreUnidad = cbxUnidadProyeccion.Text;

                Int32 mes = 1;
                foreach (DataGridViewRow row in dgvProyecciones.Rows)
                {
                    detalleNuevo = new DetalleProyeccionVenta();
                    detalleNuevo.Mes = mes;
                    detalleNuevo.Cantidad = Convert.ToDouble(row.Cells[1].Value.ToString());
                    detalleNuevo.Precio = Convert.ToDouble(row.Cells[2].Value.ToString());
                    proyeccionNueva.DetallesProyeccionVenta.Add(detalleNuevo);

                    mes++;
                }

                proyecto.Proyecciones.Add(proyeccionData.InsertarProyeccionVenta(proyeccionNueva, this.proyecto.CodProyecto));
                MessageBox.Show("Proyección insertada con éxito",
                        "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                new RegistrarProyectoWindow(this.evaluador, this.proyecto, 5)
                {
                    MdiParent = base.MdiParent
                }.Show();
                base.Close();
            }
            catch
            {
                MessageBox.Show("Error al insertar la Proyección",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditarProyeccion()
        {
            try
            {
                proyeccionNueva.NombreArticulo = tbxNombreArticuloProyeccion.Text;
                proyeccionNueva.UnidadMedida.CodUnidad = Convert.ToInt32(cbxUnidadProyeccion.SelectedValue);
                proyeccionNueva.UnidadMedida.NombreUnidad = cbxUnidadProyeccion.Text;

                Int32 mes = 1;
                for (int i = 0; i < dgvProyecciones.Rows.Count; i++)
                {
                    proyeccionNueva.DetallesProyeccionVenta[i].Cantidad = Convert.ToDouble(dgvProyecciones.Rows[i].Cells[1].Value.ToString());
                    proyeccionNueva.DetallesProyeccionVenta[i].Precio = Convert.ToDouble(dgvProyecciones.Rows[i].Cells[2].Value.ToString());

                    mes++;
                }

                if (proyeccionData.EditarProyeccionVenta(proyeccionNueva))
                {
                    MessageBox.Show("Proyección actualizada con éxito",
                        "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    new RegistrarProyectoWindow(this.evaluador, this.proyecto, 5)
                    {
                        MdiParent = base.MdiParent
                    }.Show();
                    base.Close();
                }
                else
                {
                    MessageBox.Show("Error al actualizar la Proyección",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Error al actualizar la Proyección",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarComboUnidadMedida()
        {
            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            cbxUnidadProyeccion.DataSource = unidadMedidaData.GetUnidadesMedida();
            cbxUnidadProyeccion.DisplayMember = "nombre_unidad";
            cbxUnidadProyeccion.ValueMember = "cod_unidad";
            cbxUnidadProyeccion.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new RegistrarProyectoWindow(this.evaluador, this.proyecto, 5)
            {
                MdiParent = base.MdiParent
            }.Show();
            base.Close();
        }
    }
}
