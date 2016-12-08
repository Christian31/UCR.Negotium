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
        public int CodProyecto { get; set; }
        public String NombreProyecto { get; set; }
        public String ResumenEjecutivo { get; set; }
        public bool ConIngresos { get; set; } //Verdadero si es con ingresos falso si es sin ingresos
        public String DescripcionPoblacionBeneficiaria { get; set; }
        public String CaraterizacionDelBienServicio { get; set; }
        public String DescripcionSostenibilidadDelProyecto { get; set; }
        public String DireccionExacta { get; set; }
        public String JustificacionDeMercado { get; set; }
        public int AnoInicial { get; set; }
        public int HorizonteEvaluacionEnAnos { get; set; }
        public int DemandaAnual { get; set; }
        public int OfertaAnual { get; set; }
        public bool PagaImpuesto { get; set; }
        public double PorcentajeImpuesto { get; set; }
        public Evaluador Evaluador { get; set; }
        public Provincia Provincia { get; set; }
        public Canton Canton { get; set; }
        public Distrito Distrito { get; set; }
        public Proponente Proponente { get; set; }
        public ObjetoInteresProyecto ObjetoInteres { get; set; }
        public List<RequerimientoInversion> RequerimientosInversion { get; set; }
        public List<RequerimientoReinversion> RequerimientosReinversion { get; set; }
        public List<CrecimientoOfertaObjetoInteres> CrecimientosAnuales { get; set; }
        public List<ProyeccionVentaArticulo> Proyecciones { get; set; }
        public List<Costo> Costos { get; set; }
        public List<VariacionAnualCosto> VariacionCostos { get; set; }
        public List<InteresFinanciamiento> InteresesFinanciamientoIV { get; set; }
        public Financiamiento FinanciamientoIV { get; set; }
        public InteresFinanciamiento InteresFinanciamientoIF { get; set; }
        public Financiamiento FinanciamientoIF { get; set; }

        private List<double> ingresosGenerados; //atributo calculado
        private List<double> costosGenerados; //atributo calculado

        //atributo calculado
        //string nombre depreciacion
        //double depreciacion
        private List<Depreciacion> depreciaciones;
        private List<double> totalDepreciaciones;
        private List<double> utilidadOperativa;

        public Proyecto()
        {
            //TODO inicializar todos los proyectos
            this.RequerimientosInversion = new List<RequerimientoInversion>();
            this.RequerimientosReinversion = new List<RequerimientoReinversion>();
            this.Evaluador = new Evaluador();
            this.Provincia = new Provincia();
            this.Canton = new Canton();
            this.Distrito = new Distrito();
            this.Proponente = new Proponente();
            this.ObjetoInteres = new ObjetoInteresProyecto();
            this.Proponente.NumIdentificacion = "-1";
            this.CrecimientosAnuales = new List<CrecimientoOfertaObjetoInteres>();
            this.Proyecciones = new List<ProyeccionVentaArticulo>();
            this.IngresosGenerados = new List<double>();
            this.Costos = new List<Costo>();
            this.VariacionCostos = new List<VariacionAnualCosto>();
            this.InteresesFinanciamientoIV = new List<InteresFinanciamiento>();
            this.InteresFinanciamientoIF = new InteresFinanciamiento();
            this.FinanciamientoIV = new Financiamiento();
            this.FinanciamientoIF = new Financiamiento();
            this.depreciaciones = new List<Depreciacion>();
            this.totalDepreciaciones = new List<double>();
            this.utilidadOperativa = new List<double>();
        }

        public List<double> UtilidadOperativa
        {
            get
            {
                return calcularUtilidadOperativa();
            }
            set
            {
                utilidadOperativa = value;
            }
        }

        public List<double> TotalDepreciaciones
        {
            get
            {
                return this.calcularTotalDepreciaciones();
            }
            set
            {
                totalDepreciaciones = value;
            }
        }

        public List<Depreciacion> Depreciaciones
        {
            get
            {
                return this.calcularDepreciaciones();
            }
            set
            {
                depreciaciones = value;
            }
        }

        public List<double> IngresosGenerados
        {
            get
            {
                return this.calcularIngresosGenerados();
            }
            set
            {
                ingresosGenerados = value;
            }
        }

        private List<Depreciacion> calcularDepreciaciones()
        {
            List<RequerimientoInversion> inversiones = this.RequerimientosInversion;
            List<RequerimientoReinversion> reinversiones = this.RequerimientosReinversion;
            List<Depreciacion> depreciaciones = new List<Depreciacion>();

            foreach (RequerimientoInversion inversion in inversiones)
            {
                if (inversion.Depreciable && inversion.VidaUtil > 0)
                {
                    Depreciacion depreciacion = new Depreciacion();
                    depreciacion.NombreDepreciacion = inversion.DescripcionRequerimiento;
                    depreciacion.CodDepresiacion = inversion.CodRequerimientoInversion;
                    int count = 0;
                    while (count < this.HorizonteEvaluacionEnAnos)
                    {
                        if (count < inversion.VidaUtil)
                        {
                            depreciacion.MontoDepreciacion.Add(inversion.Depreciacion);
                        }
                        else
                        {
                            depreciacion.MontoDepreciacion.Add(0);
                        }
                        count++;
                    }
                    depreciaciones.Add(depreciacion);
                }
            }

            foreach (RequerimientoReinversion reinversion in reinversiones)
            {
                if (reinversion.Depreciable && reinversion.VidaUtil > 0)
                {
                    if (reinversion.CodRequerimientoInversion != 0)
                    {
                        for (int i = 0; i < inversiones.Count; i++)
                        {
                            if (reinversion.CodRequerimientoInversion.Equals(inversiones[i].CodRequerimientoInversion) && inversiones[i].Depreciable && inversiones[i].VidaUtil > 0)
                            {
                                Depreciacion dep = depreciaciones.Where(s => s.CodDepresiacion.Equals(inversiones[i].CodRequerimientoInversion)).First();
                                if (!dep.Equals(null))
                                {
                                    List<double> montosTemp = new List<double>();
                                    int count2 = reinversion.AnoReinversion - this.AnoInicial;
                                    int count = count2;
                                    while (count > 0)
                                    {
                                        montosTemp.Add(0);
                                        count--;
                                    }

                                    int countF = count2 + reinversion.VidaUtil;
                                    while (count2 < this.HorizonteEvaluacionEnAnos - 1)
                                    {
                                        if (count2 < countF)
                                        {
                                            montosTemp.Add(reinversion.Depreciacion);
                                        }
                                        else
                                        {
                                            montosTemp.Add(0);
                                        }
                                        count2++;
                                    }
                                    montosTemp.Add(reinversion.Depreciacion);

                                    List<double> m = montosTemp;
                                    for (int ite = 0; ite < montosTemp.Count; ite++)
                                    {
                                        dep.MontoDepreciacion[ite] = dep.MontoDepreciacion[ite] + montosTemp[ite];
                                    }

                                    depreciaciones.Where(s => s.CodDepresiacion.Equals(inversiones[i].CodRequerimientoInversion)).First().MontoDepreciacion = dep.MontoDepreciacion;
                                }
                            }
                        }
                    }
                    else
                    {
                        Depreciacion depreciacion = new Depreciacion();
                        depreciacion.NombreDepreciacion = reinversion.DescripcionRequerimiento;
                        int count2 = reinversion.AnoReinversion - this.AnoInicial;
                        int count = count2;
                        while (count > 0)
                        {
                            depreciacion.MontoDepreciacion.Add(0);
                            count--;
                        }

                        int countF = count2 + reinversion.VidaUtil;
                        while (count2 < this.HorizonteEvaluacionEnAnos)
                        {
                            if (count2 < countF)
                            {
                                depreciacion.MontoDepreciacion.Add(reinversion.Depreciacion);
                            }
                            else
                            {
                                depreciacion.MontoDepreciacion.Add(0);
                            }
                            count2++;
                        }
                        depreciaciones.Add(depreciacion);
                    }
                }
            }

            return depreciaciones;
        }

        private List<double> calcularIngresosGenerados()
        {
            double valIni = 0;
            List<double> listIngresos = new List<double>();
            foreach (ProyeccionVentaArticulo articulo in this.Proyecciones)
            {
                foreach (DetalleProyeccionVenta detArticulo in articulo.DetallesProyeccionVenta)
                {
                    valIni = valIni + (detArticulo.Cantidad * detArticulo.Precio);
                }
            }
            listIngresos.Add(valIni);
            for (int i =0; i<this.CrecimientosAnuales.Count; i++)
            {
                valIni = ((valIni * CrecimientosAnuales[i].PorcentajeCrecimiento) / 100) + valIni;
                listIngresos.Add(valIni);
            }

            return listIngresos;
        }

        public List<double> CostosGenerados
        {
            get
            {
                return this.calcularCostosGenerados();
            }
            set
            {
                costosGenerados = value;
            }
        }

        private List<double> calcularCostosGenerados()
        {
            double valIni = 0;
            List<double> listCostos = new List<double>();
            int inicio = this.AnoInicial;
            while (inicio <= (this.AnoInicial + this.HorizonteEvaluacionEnAnos))
            {
                foreach (Costo articulo in this.Costos)
                {
                    if (articulo.AnoCosto > this.AnoInicial || articulo.AnoCosto < this.AnoInicial+this.HorizonteEvaluacionEnAnos)
                    {
                        foreach (CostoMensual detArticulo in articulo.CostosMensuales)
                        {
                            valIni = valIni + detArticulo.Subtotal;
                        }
                    }
                }
                listCostos.Add(valIni);

                for (int i = 0; i < this.VariacionCostos.Count; i++)
                {
                    valIni = ((valIni * VariacionCostos[i].PorcentajeIncremento) / 100) + valIni;
                    listCostos.Add(valIni);
                }

                inicio++;
            }

            

            return listCostos;
        }

        public List<double> calcularTotalDepreciaciones()
        {
            List<double> totalDep = new List<double>();

            for (int i=0; i< HorizonteEvaluacionEnAnos; i++)
            {
                double montoAnual = 0;
                for (int a=0; a < Depreciaciones.Count; a++)
                {
                    montoAnual = montoAnual + Depreciaciones[a].MontoDepreciacion[i];
                }
                totalDep.Add(montoAnual);
            }

            return totalDep;
        }

        public List<double> calcularUtilidadOperativa()
        {
            List<double> utilidadOperativa = new List<double>();
            for (int i = 0; i < HorizonteEvaluacionEnAnos; i++)
            {
                utilidadOperativa.Add(-TotalDepreciaciones[i] + IngresosGenerados[i] - CostosGenerados[i]);
            }
            return utilidadOperativa;
        }
    }
}
