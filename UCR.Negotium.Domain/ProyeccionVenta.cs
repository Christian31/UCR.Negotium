using System.Collections.Generic;
using System.Linq;

namespace UCR.Negotium.Domain
{
    public class ProyeccionVenta
    {
        public int CodArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public int AnoArticulo { get; set; }

        private List<DetalleProyeccionVenta> detallesProyeccionVenta;
        private UnidadMedida unidadMedida;
        private List<CrecimientoOfertaPorTipo> crecimientoOfertaPorTipo;

        public ProyeccionVenta()
        {
            unidadMedida = new UnidadMedida();
            detallesProyeccionVenta = new List<DetalleProyeccionVenta>() { new DetalleProyeccionVenta { Mes= "Enero"},
                new DetalleProyeccionVenta { Mes= "Febrero"}, new DetalleProyeccionVenta { Mes= "Marzo"},
                new DetalleProyeccionVenta { Mes= "Abril"}, new DetalleProyeccionVenta { Mes= "Mayo"},
                new DetalleProyeccionVenta { Mes= "Junio"}, new DetalleProyeccionVenta { Mes= "Julio"},
                new DetalleProyeccionVenta { Mes= "Agosto"} ,new DetalleProyeccionVenta { Mes= "Setiembre"},
                new DetalleProyeccionVenta { Mes= "Octubre"}, new DetalleProyeccionVenta { Mes= "Noviembre"},
                new DetalleProyeccionVenta { Mes= "Diciembre"}};

            crecimientoOfertaPorTipo = new List<CrecimientoOfertaPorTipo>();
        }

        public List<CrecimientoOfertaPorTipo> CrecimientosOferta
        {
            get { return crecimientoOfertaPorTipo; }
            set { crecimientoOfertaPorTipo = value; }
        }

        public UnidadMedida UnidadMedida
        {
            get { return unidadMedida; }
            set { unidadMedida = value; }
        }

        public List<DetalleProyeccionVenta> DetallesProyeccionVenta
        {
            get { return detallesProyeccionVenta; }
            set { detallesProyeccionVenta = value; }
        }

        public List<double> IngresoGenerado(int anoinicial, int horizonteDeEvaluacion)
        {
            int anoFinal = anoinicial + horizonteDeEvaluacion;
            List<double> ingresos = new List<double>();
            List<IncrementosTemporales> incrementosTemporales = new List<IncrementosTemporales>();
            anoinicial = anoinicial + 1;
            while (anoinicial < AnoArticulo)
            {
                ingresos.Add(0);
                anoinicial++;
            }

            double valorAnual = ObtengaValorAnualAcumulado(0, 0, incrementosTemporales, 0);
            double valorAnualTemp = 0;
            ingresos.Add(valorAnual);
            anoinicial++;

            CrecimientoOfertaPorTipo crecimientosPorPrecio = CrecimientosOferta.FirstOrDefault(crec => crec.TipoCrecimiento == TipoAplicacionPorcentaje.PorPrecio);
            CrecimientoOfertaPorTipo crecimientosPorCantidad = CrecimientosOferta.FirstOrDefault(crec => crec.TipoCrecimiento == TipoAplicacionPorcentaje.PorCantidad);
            
            double crecimientoPrecio = 0;
            double crecimientoCantidad = 0;
            while (anoinicial <= anoFinal)
            {
                if (crecimientosPorPrecio != null)
                {
                    crecimientoPrecio = crecimientosPorPrecio.CrecimientoOferta.
                        First(crec => crec.AnoCrecimiento == anoinicial).PorcentajeCrecimiento;
                }
                if (crecimientosPorCantidad != null)
                {
                    crecimientoCantidad = crecimientosPorCantidad.CrecimientoOferta.
                        First(crec => crec.AnoCrecimiento == anoinicial).PorcentajeCrecimiento;
                }

                valorAnualTemp = ObtengaValorAnualAcumulado(crecimientoPrecio, crecimientoCantidad,
                    incrementosTemporales, anoinicial);
                if (valorAnualTemp != 0)
                    valorAnual = valorAnualTemp;
                
                ingresos.Add(valorAnual);
                anoinicial++;
            }

            return ingresos;
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
                    foreach (DetalleProyeccionVenta detalle in DetallesProyeccionVenta)
                    {
                        valorAnual += detalle.Precio * detalle.Cantidad;
                    }
                }
            }
            else if (crecimientoPrecio == 0)
            {
                foreach (DetalleProyeccionVenta detalle in DetallesProyeccionVenta)
                {
                    incrementoCantidadAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodDetalle, 
                        anoCrecimiento, TipoAplicacionPorcentaje.PorCantidad);

                    incrementoCantidad = ((detalle.Cantidad + incrementoCantidadAcumulado) * crecimientoCantidad) / 100;
                    incrementosTemporales.Add(new IncrementosTemporales(detalle.CodDetalle, anoCrecimiento,
                        incrementoCantidad, TipoAplicacionPorcentaje.PorCantidad));
                    incrementoCantidad += detalle.Cantidad + incrementoCantidadAcumulado;

                    incrementoPrecioAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodDetalle,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorPrecio);
                    valorAnual += (detalle.Precio + incrementoPrecioAcumulado) * incrementoCantidad;
                }
            }
            else if (crecimientoCantidad == 0)
            {
                foreach (DetalleProyeccionVenta detalle in DetallesProyeccionVenta)
                {
                    incrementoPrecioAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodDetalle,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorPrecio);

                    incrementoPrecio = ((detalle.Precio + incrementoPrecioAcumulado) * crecimientoPrecio) / 100;
                    incrementosTemporales.Add(new IncrementosTemporales(detalle.CodDetalle, anoCrecimiento,
                        incrementoPrecio, TipoAplicacionPorcentaje.PorPrecio));

                    incrementoPrecio += detalle.Precio + incrementoPrecioAcumulado;
                    incrementoCantidadAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodDetalle,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorCantidad);
                    valorAnual += incrementoPrecio * (detalle.Cantidad + incrementoCantidadAcumulado);                    
                }
            }
            else
            {
                foreach (DetalleProyeccionVenta detalle in DetallesProyeccionVenta)
                {
                    incrementoPrecioAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodDetalle,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorPrecio);

                    incrementoPrecio = ((detalle.Precio + incrementoPrecioAcumulado) * crecimientoPrecio) / 100;
                    incrementosTemporales.Add(new IncrementosTemporales(detalle.CodDetalle, anoCrecimiento,
                                            incrementoCantidad, TipoAplicacionPorcentaje.PorCantidad));
                    incrementoPrecio += detalle.Precio + incrementoPrecioAcumulado;

                    incrementoCantidadAcumulado = ObtengaCrecimientosAcumulados(incrementosTemporales, detalle.CodDetalle,
                        anoCrecimiento, TipoAplicacionPorcentaje.PorCantidad);

                    incrementoCantidad = ((detalle.Cantidad + incrementoCantidadAcumulado) * crecimientoCantidad) / 100;
                    incrementosTemporales.Add(new IncrementosTemporales(detalle.CodDetalle, anoCrecimiento,
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
