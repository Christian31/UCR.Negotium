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
        bool segundaSeleccionProyecto = false;
        Proyecto proyecto;
        Evaluador evaluador;

        public SeleccionaProyectoModificar(Evaluador evaluador)
        {
            InitializeComponent();
            LlenaComboProyecto();
            this.evaluador = evaluador;
        }

        //Metodo para llenar el comboBox del proyecto
        public void LlenaComboProyecto()
        {
            ProyectoData proyectoData = new ProyectoData();
            cbxProyecto.DataSource = proyectoData.GetProyectos();
            cbxProyecto.DisplayMember = "nombre_proyecto";
            cbxProyecto.ValueMember = "cod_proyecto";
            idProyectoSeleccionado = Int32.Parse(cbxProyecto.SelectedValue.ToString());
        }

        private void cbxProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Esta validacion evita un error que ocurre cuando se carga el combobox por primera vez pero
            //aun no se han cargado los proyectos
            if (segundaSeleccionProyecto == false)
            {
                segundaSeleccionProyecto = true;
            }
            else
            {
                idProyectoSeleccionado = Int32.Parse(cbxProyecto.SelectedValue.ToString());
                Console.WriteLine("El id es:" + idProyectoSeleccionado);
            }//else
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
            RegistrarProyecto registrarProyecto = new RegistrarProyecto(evaluador, proyecto);
            registrarProyecto.MdiParent = this.MdiParent;
            registrarProyecto.Show();
            this.Close();
        }

        //Este metodo obtiene el proyecto que está seleccionado actualmente en el combobox
        private Proyecto ObtieneProyectoParaModificar()
        {
            DataTable dtProyecto = (DataTable)cbxProyecto.DataSource;
            Proyecto proyecto = new Proyecto();
            ProponenteData proponenteData = new ProponenteData();
            foreach (DataRow fila in dtProyecto.Rows)
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
    }
}
