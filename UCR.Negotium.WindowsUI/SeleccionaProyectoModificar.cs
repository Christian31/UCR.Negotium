//@Copyright Yordan Campos Piedra
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
    public partial class SeleccionaProyectoModificar : Form
    {

        //variablesGlobales
        int idProyectoSeleccionado;
        Proyecto proyecto;
        Evaluador evaluador;
        List<Proyecto> proyectosSinFiltro;
        DataTable dtProyectos;
        ProponenteData proponenteData;

        public SeleccionaProyectoModificar(Evaluador evaluador)
        {
            proyecto = new Proyecto();
            proyectosSinFiltro = new List<Proyecto>();
            ProyectoData proyectoData = new ProyectoData();
            proponenteData = new ProponenteData();
            dtProyectos = proyectoData.GetProyectos();

            proyectosSinFiltro = (from rw in dtProyectos.AsEnumerable()
                                        select new Proyecto
                                        {
                                            CodProyecto = Convert.ToInt32(rw["cod_proyecto"]),
                                            NombreProyecto = Convert.ToString(rw["nombre_proyecto"])
                                        }).ToList();
            foreach (Proyecto proyecto in proyectosSinFiltro)
            {
                proyecto.Proponente = proponenteData.GetProponente(proyecto.CodProyecto);
            }

            InitializeComponent();
            LlenaDgvProyectos();
            this.evaluador = evaluador;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.proyecto = ObtieneProyectoParaModificar();
            RequerimientoInversionData requerimientoInversionData = new RequerimientoInversionData();
            RequerimientoReinversionData requerimientoReinversionData = new RequerimientoReinversionData();
            ProyeccionVentaArticuloData proyeccionVentaArticuloData = new ProyeccionVentaArticuloData();
            this.proyecto.RequerimientosInversion = requerimientoInversionData.GetRequerimientosInversion(proyecto.CodProyecto);
            this.proyecto.RequerimientosReinversion = requerimientoReinversionData.GetRequerimientosReinversion(proyecto.CodProyecto);
            this.proyecto.Proyecciones = proyeccionVentaArticuloData.GetProyeccionesVentaArticulo(proyecto.CodProyecto);
            RegistrarProyecto registrarProyecto = new RegistrarProyecto(evaluador, proyecto, 0);
            registrarProyecto.MdiParent = this.MdiParent;
            registrarProyecto.Show();
            this.Close();
        }

        //Este metodo obtiene el proyecto que está seleccionado actualmente en el combobox
        private Proyecto ObtieneProyectoParaModificar()
        {
            foreach (DataRow fila in dtProyectos.Rows)
            {
                if (idProyectoSeleccionado == Int32.Parse(fila["cod_proyecto"].ToString()))
                {
                    proyecto.AnoInicial = Int32.Parse(fila["ano_inicial_proyecto"].ToString());
                    proyecto.Canton.CodCanton = Int32.Parse(fila["cod_canton"].ToString());
                    proyecto.Provincia.CodProvincia = Int32.Parse(fila["cod_provincia"].ToString());
                    proyecto.Distrito.CodDistrito = Int32.Parse(fila["cod_distrito"].ToString());
                    proyecto.CaraterizacionDelBienServicio = fila["categorizacion_bien_servicio"].ToString();
                    proyecto.CodProyecto = Int32.Parse(fila["cod_proyecto"].ToString());
                    proyecto.ConIngresos = Boolean.Parse(fila["con_ingresos"].ToString());
                    proyecto.DemandaAnual = Int32.Parse(fila["demanda_anual"].ToString());
                    proyecto.DescripcionPoblacionBeneficiaria = fila["descripcion_poblacion_beneficiaria"].ToString();
                    proyecto.DescripcionSostenibilidadDelProyecto = fila["descripcion_sostenibilidad_proyecto"].ToString();
                    proyecto.DireccionExacta = fila["direccion_exacta"].ToString();
                    proyecto.Distrito.CodDistrito = Int32.Parse(fila["cod_distrito"].ToString());
                    proyecto.Evaluador = this.evaluador;
                    proyecto.HorizonteEvaluacionEnAnos = Int32.Parse(fila["horizonte_evaluacion_en_anos"].ToString());
                    proyecto.JustificacionDeMercado = fila["justificacion_de_mercado"].ToString();
                    proyecto.NombreProyecto = fila["nombre_proyecto"].ToString();
                    proyecto.ObjetoInteres = GetObjetoInteres(proyecto.CodProyecto);
                    proyecto.CrecimientosAnuales = GetCrecimientosAnuales(proyecto.CodProyecto);
                    proyecto.VariacionCostos = GetVariacionAnualCostos(proyecto.CodProyecto);
                    proyecto.Costos = GetCostos(proyecto.CodProyecto);
                    proyecto.OfertaAnual = Int32.Parse(fila["oferta_anual"].ToString());
                    proyecto.PagaImpuesto = Boolean.Parse(fila["paga_impuesto"].ToString());
                    proyecto.PorcentajeImpuesto = float.Parse(fila["porcentaje_impuesto"].ToString());
                    proyecto.Proponente = proponenteData.GetProponente(proyecto.CodProyecto);
                    proyecto.ResumenEjecutivo = fila["resumen_ejecutivo"].ToString();
                    return proyecto;
                }//if
            }//foreach
            return proyecto;
        }//ObtieneCantonParaInsertar

        //Este metodo obtiene el objeto de interes a partir del codigo del proyecto
        private ObjetoInteresProyecto GetObjetoInteres(int codProyecto)
        {
            ObjetoInteresData objetoInteresData = new ObjetoInteresData();
            return objetoInteresData.GetObjetoInteres(codProyecto);
        }

        private List<CrecimientoOfertaObjetoInteres> GetCrecimientosAnuales(int codProyecto) {
            CrecimientoOfertaObjetoInteresData crecimientoAnual = new CrecimientoOfertaObjetoInteresData();

            DataTable dt =  crecimientoAnual.GetCrecimientoOfertaObjetoIntereses(codProyecto);
            List<CrecimientoOfertaObjetoInteres> list = new List<CrecimientoOfertaObjetoInteres>();

            foreach (DataRow row in dt.Rows)
            {
                CrecimientoOfertaObjetoInteres creTemp = new CrecimientoOfertaObjetoInteres();
                creTemp.CodCrecimiento = Convert.ToInt32(row[0]);
                creTemp.AnoCrecimiento = Convert.ToInt32(row[1]);
                creTemp.PorcentajeCrecimiento = Convert.ToDouble(row[2]);

                list.Add(creTemp);
            }

            return list;
        }

        private List<VariacionAnualCosto> GetVariacionAnualCostos(int codProyecto)
        {
            VariacionAnualCostoData costosAnuales = new VariacionAnualCostoData();

            DataTable dt = costosAnuales.GetVariacionAnualCostos(codProyecto);
            List<VariacionAnualCosto> list = new List<VariacionAnualCosto>();

            foreach (DataRow row in dt.Rows)
            {
                VariacionAnualCosto creTemp = new VariacionAnualCosto();
                creTemp.CodVariacionCosto = Convert.ToInt32(row[0]);
                creTemp.Ano = Convert.ToInt32(row[1]);
                creTemp.ProcentajeIncremento = Convert.ToDouble(row[2]);

                list.Add(creTemp);
            }

            return list;
        }

        private List<Costo> GetCostos(int codProyecto)
        {
            CostoData costoData = new CostoData();
            return costoData.GetCostos(codProyecto);
        }

        private void LlenaDgvProyectos(string search = "")
        {
            List<Proyecto> proyectos = proyectosSinFiltro;
            if (!search.Equals(""))
            {
                proyectos = proyectos.Where(s => s.NombreProyecto.ToLower().Contains(search.ToLower()) ||
                    s.Proponente.Nombre.ToLower().Contains(search.ToLower()) ||
                    s.Proponente.Apellidos.ToLower().Contains(search.ToLower()) ||
                    s.Proponente.Organizacion.NombreOrganizacion.ToLower().Contains(search.ToLower())
                    ).ToList();
            }

            DataSet ds = new DataSet();
            ds.Tables.Add("Proyectos");
            ds.Tables["Proyectos"].Columns.Add("codProyecto", Type.GetType("System.Int32"));
            ds.Tables["Proyectos"].Columns.Add("nombreProyecto", Type.GetType("System.String"));
            ds.Tables["Proyectos"].Columns.Add("organizacion", Type.GetType("System.String"));
            ds.Tables["Proyectos"].Columns.Add("proponente", Type.GetType("System.String"));
            foreach (Proyecto proyecto in proyectos)
            {
                DataRow row = ds.Tables["Proyectos"].NewRow();
                row["codProyecto"] = proyecto.CodProyecto;
                row["nombreProyecto"] = proyecto.NombreProyecto;
                row["organizacion"] = proyecto.Proponente != null ? proyecto.Proponente.Organizacion.NombreOrganizacion : "";
                row["proponente"] = proyecto.Proponente != null ? proyecto.Proponente.Nombre + " " + proyecto.Proponente.Apellidos : "";
                ds.Tables["Proyectos"].Rows.Add(row);
            }//foreach
            DataTable dtProyectos = ds.Tables["Proyectos"];
            dgvProyectos.DataSource = dtProyectos;

            dgvProyectos.Columns[0].Visible = false;
        }

        private void tbBuscarProyectos_TextChanged(object sender, EventArgs e)
        {
            LlenaDgvProyectos(tbBuscarProyectos.Text);
        }

        private void dgvProyectos_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvProyectos.SelectedRows)
            {
                idProyectoSeleccionado = Convert.ToInt32(row.Cells[0].Value.ToString());
            }
        }
    }
}
