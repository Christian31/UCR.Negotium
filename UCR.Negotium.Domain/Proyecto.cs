using System;
using System.Collections.Generic;
using System.Linq;

namespace UCR.Negotium.Domain
{
    public class Proyecto
    {
        public int CodProyecto { get; set; }
        public bool Archivado { get; set; }
        public string NombreProyecto { get; set; }
        public string ResumenEjecutivo { get; set; }
        public bool ConIngresos { get; set; } //Verdadero si es con ingresos falso si es sin ingresos
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
        public List<Inversion> RequerimientosInversion { get; set; }
        public List<Reinversion> RequerimientosReinversion { get; set; }
        public List<ProyeccionVentaArticulo> Proyecciones { get; set; }
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
            ConIngresos = true;
            this.RequerimientosInversion = new List<Inversion>();
            this.RequerimientosReinversion = new List<Reinversion>();
            this.Encargado = new Encargado();
            this.Provincia = new Provincia();
            this.Canton = new Canton();
            this.Distrito = new Distrito();
            this.OrganizacionProponente = new OrganizacionProponente();
            this.Proyecciones = new List<ProyeccionVentaArticulo>();
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
            get { return string.Concat(TasaCostoCapital.ToString("#,##0.##"), " %"); }
            set { TasaCostoCapital = Convert.ToDouble(value.Replace("%", string.Empty)); }
        }

        private List<Depreciacion> calcularDepreciaciones()
        {
            List<Inversion> inversiones = this.RequerimientosInversion.Where(inv => inv.Depreciable).ToList();
            List<Reinversion> reinversiones = this.RequerimientosReinversion.Where(reinv => reinv.Depreciable).ToList();
            List<Depreciacion> depreciaciones = new List<Depreciacion>();

            foreach (Inversion inversion in inversiones)
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

            foreach (Reinversion reinversion in reinversiones)
            {
                if (reinversion.CodRequerimientoInversion != 0)
                {
                    var inversion = inversiones.Find(inv => inv.CodRequerimientoInversion.Equals(reinversion.CodRequerimientoInversion));

                    if (inversion != null)
                    {
                        Depreciacion dep = depreciaciones.Find(s => s.CodDepresiacion.Equals(inversion.CodRequerimientoInversion));
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

                            depreciaciones.Where(s => s.CodDepresiacion.Equals(inversion.CodRequerimientoInversion)).First().MontoDepreciacion = dep.MontoDepreciacion;
                        }
                    }
                }
                else
                {
                    Depreciacion depreciacion = new Depreciacion();
                    depreciacion.NombreDepreciacion = reinversion.DescripcionRequerimiento;
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
                for (int i=0; i < this.HorizonteEvaluacionEnAnos; i++)
                {
                    listIngresos.Add(0);
                }
            }
            else
            {
                foreach (ProyeccionVentaArticulo articulo in this.Proyecciones)
                {
                    double valIni = 0;
                    List<double> listIngresosArticulo = new List<double>();
                    articulo.DetallesProyeccionVenta.ForEach(detArticulo => valIni += detArticulo.Subtotal);

                    listIngresosArticulo.Add(valIni);
                    for (int i = 0; i < articulo.CrecimientoOferta.Count; i++)
                    {
                        valIni = ((valIni * articulo.CrecimientoOferta[i].PorcentajeCrecimiento) / 100) + valIni;
                        listIngresosArticulo.Add(valIni);
                    }

                    listIngresos = SumListDoubles(listIngresos, listIngresosArticulo);
                }
            }

            return listIngresos;
        }

        private List<double> SumListDoubles(List<double> listIngresos, List<double> listIngresosArticulo)
        {
            if(listIngresos.Count > 0)
            {
                if (listIngresos.Count.Equals(listIngresosArticulo.Count))
                {
                    List<double> ingresosSumados = new List<double>();
                    for (int i=0; i < listIngresos.Count; i++)
                    {
                        ingresosSumados.Add(listIngresos[i]+listIngresosArticulo[i]);
                    }
                    return ingresosSumados;
                }
                else
                {
                    return listIngresos;
                }
            }
            else
            {
                return listIngresosArticulo;
            }
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
                foreach (Costo articulo in this.Costos.Where(costo => costo.AnoCosto.Equals(inicio)))
                {
                    var valTemp = articulo.CostosMensuales.Select(costoM => costoM.Subtotal).Sum();
                    valIni += Math.Round(valTemp, 2);
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
            List<Inversion> inversiones = this.RequerimientosInversion;
            List<Reinversion> reinversiones = this.RequerimientosReinversion;
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
}
