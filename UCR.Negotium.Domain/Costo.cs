using System.Collections.Generic;
using System.Linq;

namespace UCR.Negotium.Domain
{
    public class Costo
    {
        public int CodCosto { get; set; }
        private UnidadMedida unidadMedida;
        public string NombreCosto { get; set; }
        public string CategoriaCosto { get; set; }
        public int AnoCosto { get; set; }
        private List<CostoMensual> costosMensuales;
        private List<VariacionAnualCostoPorTipo> variacionAnualCosto;

        public Costo()
        {
            unidadMedida = new UnidadMedida();
            costosMensuales = new List<CostoMensual>() { new CostoMensual { Mes= "Enero"},
                new CostoMensual { Mes= "Febrero"}, new CostoMensual { Mes= "Marzo"},
                new CostoMensual { Mes= "Abril"}, new CostoMensual { Mes= "Mayo"},
                new CostoMensual { Mes= "Junio"}, new CostoMensual { Mes= "Julio"},
                new CostoMensual { Mes= "Agosto"} ,new CostoMensual { Mes= "Setiembre"},
                new CostoMensual { Mes= "Octubre"}, new CostoMensual { Mes= "Noviembre"},
                new CostoMensual { Mes= "Diciembre"}};
            variacionAnualCosto = new List<VariacionAnualCostoPorTipo>();
        }

        public List<VariacionAnualCostoPorTipo> VariacionCostos
        {
            get
            {
                return variacionAnualCosto;
            }
            set
            {
                variacionAnualCosto = value;   
            }
        }

        public UnidadMedida UnidadMedida
        {
            get
            {
                return unidadMedida;
            }

            set
            {
                unidadMedida = value;
            }
        }

        public List<CostoMensual> CostosMensuales
        {
            get
            {
                return costosMensuales;
            }

            set
            {
                costosMensuales = value;
            }
        }

        public List<double> CostoGenerado(int anoinicial, int horizonteDeEvaluacion)
        {
            int anoFinal = anoinicial + horizonteDeEvaluacion;
            List<double> costos = new List<double>();
            List<IncrementosTemporales> incrementosTemporales = new List<IncrementosTemporales>();
            anoinicial = anoinicial + 1;
            while (anoinicial < AnoCosto)
            {
                costos.Add(0);
                anoinicial++;
            }

            double valorAnual = ObtengaValorAnualAcumulado(0, 0, incrementosTemporales, 0);
            double valorAnualTemp = 0;
            costos.Add(valorAnual);
            anoinicial++;

            VariacionAnualCostoPorTipo variacionesPorPrecio = VariacionCostos.FirstOrDefault(variacion => variacion.TipoVariacion == TipoAplicacionPorcentaje.PorPrecio);
            VariacionAnualCostoPorTipo variacionesPorCantidad = VariacionCostos.FirstOrDefault(variacion => variacion.TipoVariacion == TipoAplicacionPorcentaje.PorCantidad);

            double variacionPrecio = 0;
            double variacionCantidad = 0;
            while (anoinicial <= anoFinal)
            {
                if (variacionesPorPrecio != null)
                {
                    variacionPrecio = variacionesPorPrecio.VariacionAnual.
                        First(variacion => variacion.Ano == anoinicial).PorcentajeIncremento;
                }
                if (variacionesPorCantidad != null)
                {
                    variacionCantidad = variacionesPorCantidad.VariacionAnual.
                        First(variacion => variacion.Ano == anoinicial).PorcentajeIncremento;
                }

                valorAnualTemp = ObtengaValorAnualAcumulado(variacionPrecio, variacionCantidad, incrementosTemporales, anoinicial);
                if (valorAnualTemp != 0)
                    valorAnual = valorAnualTemp;

                costos.Add(valorAnual);
                anoinicial++;
            }

            return costos;
        }

        private double ObtengaValorAnualAcumulado(double crecimientoPrecio, double crecimientoCantidad,
            List<IncrementosTemporales> incrementosTemporales, int anoCrecimiento)
        {
            double valorAnual = 0;
            double incrementoPrecio = 0;
            double incrementoCantidad = 0;
            double incrementoPrecioAcumulado = 0;
            double incrementoCantidadAcumulado = 0;
            if (crecimientoPrecio == 0 && crecimientoCantidad == 0)
            {
                if (incrementosTemporales.Count == 0)
                {
                    foreach (CostoMensual detalle in CostosMensuales)
                    {
                        valorAnual += detalle.CostoUnitario * detalle.Cantidad;
                    }
                }
            }
            else if (crecimientoPrecio == 0)
            {
                foreach (CostoMensual detalle in CostosMensuales)
                {
                    incrementoCantidadAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodCostoMensual,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorCantidad);

                    incrementoCantidad = ((detalle.Cantidad + incrementoCantidadAcumulado) * crecimientoCantidad) / 100;
                    incrementosTemporales.Add(new IncrementosTemporales(detalle.CodCostoMensual, anoCrecimiento,
                        incrementoCantidad, TipoAplicacionPorcentaje.PorCantidad));
                    incrementoCantidad += detalle.Cantidad + incrementoCantidadAcumulado;

                    incrementoPrecioAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodCostoMensual,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorPrecio);
                    valorAnual += (detalle.CostoUnitario + incrementoPrecioAcumulado) * incrementoCantidad;
                }
            }
            else if (crecimientoCantidad == 0)
            {
                foreach (CostoMensual detalle in CostosMensuales)
                {
                    incrementoPrecioAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodCostoMensual,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorPrecio);

                    incrementoPrecio = ((detalle.CostoUnitario + incrementoPrecioAcumulado) * crecimientoPrecio) / 100;
                    incrementosTemporales.Add(new IncrementosTemporales(detalle.CodCostoMensual, anoCrecimiento,
                        incrementoPrecio, TipoAplicacionPorcentaje.PorPrecio));

                    incrementoPrecio += detalle.CostoUnitario + incrementoPrecioAcumulado;
                    incrementoCantidadAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodCostoMensual,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorCantidad);
                    valorAnual += incrementoPrecio * (detalle.Cantidad + incrementoCantidadAcumulado);
                }
            }
            else
            {
                foreach (CostoMensual detalle in CostosMensuales)
                {
                    incrementoPrecioAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodCostoMensual,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorPrecio);

                    incrementoPrecio = ((detalle.CostoUnitario + incrementoPrecioAcumulado) * crecimientoPrecio) / 100;
                    incrementosTemporales.Add(new IncrementosTemporales(detalle.CodCostoMensual, anoCrecimiento,
                                            incrementoCantidad, TipoAplicacionPorcentaje.PorCantidad));
                    incrementoPrecio += detalle.CostoUnitario + incrementoPrecioAcumulado;

                    incrementoCantidadAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodCostoMensual,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorCantidad);

                    incrementoCantidad = ((detalle.Cantidad + incrementoCantidadAcumulado) * crecimientoCantidad) / 100;
                    incrementosTemporales.Add(new IncrementosTemporales(detalle.CodCostoMensual, anoCrecimiento,
                                            incrementoPrecio, TipoAplicacionPorcentaje.PorPrecio));
                    incrementoCantidad += detalle.Cantidad + incrementoCantidadAcumulado;

                    valorAnual += incrementoPrecio * incrementoCantidad;
                }
            }

            return valorAnual;
        }

        private double ObtengaCrecimientosAcumulados(List<IncrementosTemporales> incrementos, int id,
            int anoActual, TipoAplicacionPorcentaje tipoIncremento)
        {
            double incrementosSumados = 0;
            List<IncrementosTemporales> incrementosAcumulados = incrementos.
                Where(inc => inc.Id == id && inc.AnoIncremento < anoActual && inc.TipoIncremento == tipoIncremento).ToList();

            if (incrementosAcumulados.Count > 0)
            {
                incrementosSumados = incrementosAcumulados.Select(inc => inc.Incremento).Sum();
            }

            return incrementosSumados;
        }
    }
}
