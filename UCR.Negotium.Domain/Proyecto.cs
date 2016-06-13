//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Proyecto
    {
        int codProyecto;
        String nombreProyecto;
        String resumenEjecutivo;
        bool conIngresos; //Verdadero si es con ingresos falso si es sin ingresos
        String descripcionPoblacionBeneficiaria;
        String caraterizacionDelBienServicio;
        String descripcionSostenibilidadDelProyecto;
        String direccionExacta;
        String justificacionDeMercado;
        int anoInicial;
        int horizonteEvaluacionEnAnos;
        int demandaAnual;
        int ofertaAnual;
        bool pagaImpuesto;
        double porcentajeImpuesto;
        Evaluador evaluador;
        Provincia provincia;
        Canton canton;
        Distrito distrito;
        Proponente proponente;
        ObjetoInteresProyecto objetoInteres;
        List<RequerimientoInversion> requerimientosInversion;
        List<RequerimientoReinversion> requerimientosReinversion;
        List<CrecimientoOfertaObjetoInteres> crecimientosAnuales;
        List<ProyeccionVentaArticulo> proyecciones;

        public Proyecto()
        {
            //TODO inicializar todos los proyectos
            this.requerimientosInversion = new List<RequerimientoInversion>();
            this.RequerimientosReinversion = new List<RequerimientoReinversion>();
            this.evaluador = new Evaluador();
            this.provincia = new Provincia();
            this.canton = new Canton();
            this.distrito = new Distrito();
            this.proponente = new Proponente();
            this.objetoInteres = new ObjetoInteresProyecto();
            this.proponente.NumIdentificacion = "-1";
        }

        public int CodProyecto
        {
            get
            {
                return codProyecto;
            }

            set
            {
                codProyecto = value;
            }
        }

        public string NombreProyecto
        {
            get
            {
                return nombreProyecto;
            }

            set
            {
                nombreProyecto = value;
            }
        }

        public string ResumenEjecutivo
        {
            get
            {
                return resumenEjecutivo;
            }

            set
            {
                resumenEjecutivo = value;
            }
        }

        public bool ConIngresos
        {
            get
            {
                return conIngresos;
            }

            set
            {
                conIngresos = value;
            }
        }

        public string DescripcionPoblacionBeneficiaria
        {
            get
            {
                return descripcionPoblacionBeneficiaria;
            }

            set
            {
                descripcionPoblacionBeneficiaria = value;
            }
        }

        public string CaraterizacionDelBienServicio
        {
            get
            {
                return caraterizacionDelBienServicio;
            }

            set
            {
                caraterizacionDelBienServicio = value;
            }
        }

        public string DescripcionSostenibilidadDelProyecto
        {
            get
            {
                return descripcionSostenibilidadDelProyecto;
            }

            set
            {
                descripcionSostenibilidadDelProyecto = value;
            }
        }

        public string DireccionExacta
        {
            get
            {
                return direccionExacta;
            }

            set
            {
                direccionExacta = value;
            }
        }

        public string JustificacionDeMercado
        {
            get
            {
                return justificacionDeMercado;
            }

            set
            {
                justificacionDeMercado = value;
            }
        }

        public int AnoInicial
        {
            get
            {
                return anoInicial;
            }

            set
            {
                anoInicial = value;
            }
        }

        public int HorizonteEvaluacionEnAnos
        {
            get
            {
                return horizonteEvaluacionEnAnos;
            }

            set
            {
                horizonteEvaluacionEnAnos = value;
            }
        }

        public int DemandaAnual
        {
            get
            {
                return demandaAnual;
            }

            set
            {
                demandaAnual = value;
            }
        }

        public int OfertaAnual
        {
            get
            {
                return ofertaAnual;
            }

            set
            {
                ofertaAnual = value;
            }
        }

        public bool PagaImpuesto
        {
            get
            {
                return pagaImpuesto;
            }

            set
            {
                pagaImpuesto = value;
            }
        }

        public double PorcentajeImpuesto
        {
            get
            {
                return porcentajeImpuesto;
            }

            set
            {
                porcentajeImpuesto = value;
            }
        }

        public Evaluador Evaluador
        {
            get
            {
                return evaluador;
            }

            set
            {
                evaluador = value;
            }
        }

        public Provincia Provincia
        {
            get
            {
                return provincia;
            }

            set
            {
                provincia = value;
            }
        }

        public Canton Canton
        {
            get
            {
                return canton;
            }

            set
            {
                canton = value;
            }
        }

        public Distrito Distrito
        {
            get
            {
                return distrito;
            }

            set
            {
                distrito = value;
            }
        }

        public Proponente Proponente
        {
            get
            {
                return proponente;
            }

            set
            {
                proponente = value;
            }
        }

        public ObjetoInteresProyecto ObjetoInteres
        {
            get
            {
                return objetoInteres;
            }

            set
            {
                objetoInteres = value;
            }
        }

        public List<RequerimientoInversion> RequerimientosInversion
        {
            get
            {
                return requerimientosInversion;
            }

            set
            {
                requerimientosInversion = value;
            }
        }

        public List<RequerimientoReinversion> RequerimientosReinversion
        {
            get
            {
                return requerimientosReinversion;
            }

            set
            {
                requerimientosReinversion = value;
            }
        }

        public List<CrecimientoOfertaObjetoInteres> CrecimientosAnuales
        {
            get
            {
                return crecimientosAnuales;
            }

            set
            {
                crecimientosAnuales = value;
            }
        }

        public List<ProyeccionVentaArticulo> Proyecciones
        {
            get
            {
                return proyecciones;
            }

            set
            {
                proyecciones = value;
            }
        }
    }
}
