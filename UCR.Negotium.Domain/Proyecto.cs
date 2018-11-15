using System;
using System.Collections.Generic;
using System.Linq;
using UCR.Negotium.Base.Utilidades;

namespace UCR.Negotium.Domain
{
    public class Proyecto
    {
        public int CodProyecto { get; set; }
        public bool Archivado { get; set; }
        public string NombreProyecto { get; set; }
        public string ResumenEjecutivo { get; set; }
        public string DescripcionPoblacionBeneficiaria { get; set; }
        public string CaraterizacionDelBienServicio { get; set; }
        public string DescripcionSostenibilidadDelProyecto { get; set; }
        public string DireccionExacta { get; set; }
        public string JustificacionDeMercado { get; set; }
        public int AnoInicial { get; set; }
        public int HorizonteEvaluacionEnAnos { get; set; }
        public bool PagaImpuesto { get; set; }
        public double PorcentajeImpuesto { get; set; }
        public Encargado Encargado { get; set; }
        public Provincia Provincia { get; set; }
        public Canton Canton { get; set; }
        public Distrito Distrito { get; set; }
        public OrganizacionProponente OrganizacionProponente { get; set; }
        public string ObjetoInteres { get; set; }
        public List<Inversion> Inversiones { get; set; }
        public List<Reinversion> Reinversiones { get; set; }
        public List<ProyeccionVenta> Proyecciones { get; set; }
        public List<Costo> Costos { get; set; }
        public List<VariacionAnualCosto> VariacionCostos { get; set; }
        public Financiamiento Financiamiento { get; set; }
        public double TasaCostoCapital { get; set; }
        public int PersonasParticipantes { get; set; }
        public int FamiliasInvolucradas { get; set; }
        public int PersonasBeneficiadas { get; set; }
        public bool ConFinanciamiento { get; set; }
        public TipoProyecto TipoProyecto { get; set; }
        public TipoMoneda TipoMoneda { get; set; }

        //atributo calculado
        private List<double> ingresosGenerados;
        private List<double> costosGenerados;
        private List<Depreciacion> depreciaciones;
        private List<double> totalDepreciaciones;
        private List<double> utilidadOperativa;
        private double valorResidual;

        public Proyecto()
        {
            Archivado = false;
            this.Inversiones = new List<Inversion>();
            this.Reinversiones = new List<Reinversion>();
            this.Encargado = new Encargado();
            this.Provincia = new Provincia();
            this.Canton = new Canton();
            this.Distrito = new Distrito();
            this.OrganizacionProponente = new OrganizacionProponente();
            this.Proyecciones = new List<ProyeccionVenta>();
            this.IngresosGenerados = new List<double>();
            this.Costos = new List<Costo>();
            this.VariacionCostos = new List<VariacionAnualCosto>();
            this.Financiamiento = new Financiamiento();
            this.TipoProyecto = new TipoProyecto();
            this.TipoMoneda = new TipoMoneda() { CodMoneda = 1 };
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

        public double ValorResidual
        {
            get
            {
                return calcularValorResidual();
            }
            set
            {
                this.valorResidual = value;
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

        public string TasaCostoCapitalString
        {
            get { return TasaCostoCapital.FormatoPorcentaje(); }
            set { TasaCostoCapital = Convert.ToDouble(value.Replace("%", string.Empty)); }
        }

        private List<Depreciacion> calcularDepreciaciones()
        {
            List<Inversion> inversiones = this.Inversiones.Where(inv => inv.Depreciable).ToList();
            List<Reinversion> reinversiones = this.Reinversiones.Where(reinv => reinv.Depreciable).ToList();
            List<Depreciacion> depreciaciones = new List<Depreciacion>();

            foreach (Inversion inversion in inversiones)
            {
                Depreciacion depreciacion = new Depreciacion();
                depreciacion.NombreDepreciacion = inversion.Descripcion;
                depreciacion.CodDepresiacion = inversion.CodInversion;
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

            foreach (Reinversion reinversion in reinversiones)
            {
                if (reinversion.CodInversion != 0)
                {
                    var inversion = inversiones.Find(inv => inv.CodInversion.Equals(reinversion.CodInversion));

                    if (inversion != null)
                    {
                        Depreciacion dep = depreciaciones.Find(s => s.CodDepresiacion.Equals(inversion.CodInversion));
                        if (dep != null)
                        {
                            List<double> montosTemp = new List<double>();
                            int count2 = reinversion.AnoReinversion - this.AnoInicial;
                            int countF = reinversion.VidaUtil;
                            int count = 0;
                            while (count < this.HorizonteEvaluacionEnAnos)
                            {
                                if (count < count2 || countF == 0)
                                    montosTemp.Add(0);
                                else
                                {
                                    montosTemp.Add(reinversion.Depreciacion);
                                    countF--;
                                }

                                count++;
                            }

                            List<double> m = montosTemp;
                            for (int ite = 0; ite < montosTemp.Count; ite++)
                            {
                                dep.MontoDepreciacion[ite] = dep.MontoDepreciacion[ite] + montosTemp[ite];
                            }

                            depreciaciones.Where(s => s.CodDepresiacion.Equals(inversion.CodInversion)).First().MontoDepreciacion = dep.MontoDepreciacion;
                        }
                    }
                }
                else
                {
                    Depreciacion depreciacion = new Depreciacion();
                    depreciacion.NombreDepreciacion = reinversion.Descripcion;
                    int count2 = reinversion.AnoReinversion - this.AnoInicial;
                    int countF = reinversion.VidaUtil;
                    int count = 0;
                    while (count < this.HorizonteEvaluacionEnAnos)
                    {
                        if (count < count2 || countF == 0)
                            depreciacion.MontoDepreciacion.Add(0);
                        else
                        {
                            depreciacion.MontoDepreciacion.Add(reinversion.Depreciacion);
                            countF--;
                        }

                        count++;
                    }
                    depreciaciones.Add(depreciacion);
                }
            }

            return depreciaciones;
        }

        private List<double> calcularIngresosGenerados()
        {
            List<double> listIngresos = new List<double>();
            if (this.Proyecciones.Count.Equals(0))
            {
                listIngresos = Operaciones.ObtenerListaPorDefecto(this.HorizonteEvaluacionEnAnos);
            }
            else
            {
                foreach (ProyeccionVenta proyeccion in this.Proyecciones)
                {
                    listIngresos = Operaciones.SumarListas(listIngresos, proyeccion.IngresoGenerado(this.AnoInicial, this.HorizonteEvaluacionEnAnos));
                }
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
            double porcentaje = 0;
            List<double> listCostos = new List<double>();
            int inicio = this.AnoInicial+1;
            int count = 0;
            while (inicio <= (this.AnoInicial + this.HorizonteEvaluacionEnAnos))
            {
                foreach (Costo costo in this.Costos.Where(costo => costo.AnoCosto.Equals(inicio)))
                {
                    var valTemp = costo.CostosMensuales.Select(costoM => costoM.Subtotal).Sum();
                    valIni += valTemp.PonderarNumero();
                }

                if (count > 0)
                {
                    porcentaje = VariacionCostos.Count > 0 ? ((listCostos[listCostos.Count - 1] + valIni) * VariacionCostos[count].PorcentajeIncremento) / 100 : 0;
                    listCostos.Add(listCostos[listCostos.Count - 1] + valIni + porcentaje);
                }
                else
                {
                    porcentaje = VariacionCostos.Count > 0 ? (valIni * VariacionCostos[count].PorcentajeIncremento) / 100 : 0;
                    listCostos.Add(valIni + porcentaje);
                }

                valIni = 0;
                inicio++;
                count++;
            }

            return listCostos;
        }

        public List<double> calcularTotalDepreciaciones()
        {
            List<double> totalDep = new List<double>();

            for (int i=0; i< HorizonteEvaluacionEnAnos; i++)
            {
                double montoAnual = 0;
                Depreciaciones.ForEach(dep => montoAnual += dep.MontoDepreciacion[i]);
                montoAnual = montoAnual.PonderarNumero();
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

        public double calcularValorResidual()
        {
            List<Inversion> inversiones = this.Inversiones;
            List<Reinversion> reinversiones = this.Reinversiones;
            double valorRes = 0;
            foreach (Inversion inversion in inversiones)
            {
                if (inversion.Depreciable) { 
                    if (inversion.VidaUtil > this.HorizonteEvaluacionEnAnos)
                    {
                        int res = inversion.VidaUtil - this.HorizonteEvaluacionEnAnos;
                        valorRes += inversion.Depreciacion * res;
                    }
                }
            }

            foreach(Reinversion reinversion in reinversiones)
            {
                if (reinversion.Depreciable)
                {
                    if ((reinversion.AnoReinversion + reinversion.VidaUtil) > (this.AnoInicial + this.HorizonteEvaluacionEnAnos))
                    {
                        int res = (reinversion.AnoReinversion + reinversion.VidaUtil) - (this.AnoInicial + this.HorizonteEvaluacionEnAnos);
                        valorRes += reinversion.Depreciacion * res;
                    }
                }
            }

            return valorRes;
        }
    }

    public class ProyectoLite
    {
        public int CodProyecto { get; set; }
        public int AnoInicial { get; set; }
        public int HorizonteEvaluacionEnAnos { get; set; }
        public int CodTipoProyecto { get; set; }
        public bool ConFinanciamiento { get; set; }
    }
}
