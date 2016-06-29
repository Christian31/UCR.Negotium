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
    public partial class AgregarProyeccion : Form
    {
        private Evaluador evaluador;
        private Proyecto proyecto;
        private ProyeccionVentaArticulo proyeccionNueva;
        private DetalleProyeccionVenta detalleNuevo;
        private ProyeccionVentaArticuloData proyeccionData;
        public AgregarProyeccion(Evaluador evaluador, Proyecto proyecto)
        {
            this.evaluador = evaluador;
            proyeccionNueva = new ProyeccionVentaArticulo();
            
            InitializeComponent();
            this.proyecto = proyecto;
            this.LlenaDgvProyeccion();
            this.LlenarComboUnidadMedida();
            this.proyeccionData = new ProyeccionVentaArticuloData();
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
            for(Int32 i=0; i<12; i++)
            {
                DataRow row = ds.Tables["Proyeccion"].NewRow();
                row["mes"] = meses[i];
                row["cantidad"] = 0;
                row["precio"] = 0;
                ds.Tables["Proyeccion"].Rows.Add(row);
            }
            DataTable dtProyeccion = ds.Tables["Proyeccion"];
            this.dgvProyecciones.DataSource = dtProyeccion;
        }

        private void btnAgregarProyeccion_Click(object sender, EventArgs e)
        {
            try
            {
                proyeccionNueva.NombreArticulo = tbxNombreArticuloProyeccion.Text;
                proyeccionNueva.UnidadMedida.CodUnidad = Convert.ToInt32(cbxUnidadProyeccion.SelectedValue);
                proyeccionNueva.UnidadMedida.NombreUnidad = cbxUnidadProyeccion.Text;
                //proyeccionNueva.UnidadMedida.NombreUnidad = ((UnidadMedida)cbxUnidadProyeccion.SelectedItem).NombreUnidad;

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

                new RegistrarProyecto(this.evaluador, this.proyecto)
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
            cbxUnidadProyeccion.DataSource = unidadMedidaData.GetUnidadesMedida();
            cbxUnidadProyeccion.DisplayMember = "nombre_unidad";
            cbxUnidadProyeccion.ValueMember = "cod_unidad";
            cbxUnidadProyeccion.SelectedIndex = 0;
        }
    }
}
