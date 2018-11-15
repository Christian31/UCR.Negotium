using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class ProyeccionVenta
    {
        public int CodArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public int AnoArticulo { get; set; }

        private List<DetalleProyeccionVenta> detallesProyeccionVenta;
        private UnidadMedida unidadMedida;
        private List<CrecimientoOferta> crecimientoOferta;

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

            crecimientoOferta = new List<CrecimientoOferta>();
        }

        public List<CrecimientoOferta> CrecimientoOferta
        {
            get { return crecimientoOferta; }
            set { crecimientoOferta = value; }
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
            anoinicial = anoinicial + 1;
            while (anoinicial < AnoArticulo)
            {
                ingresos.Add(0);
                anoinicial++;
            }

            double valIni = 0;
            DetallesProyeccionVenta.ForEach(detArticulo => valIni += detArticulo.Subtotal);
            ingresos.Add(valIni);
            anoinicial++;
            while (anoinicial <= anoFinal)
            {
                CrecimientoOferta crec = CrecimientoOferta.Find(cre => cre.AnoCrecimiento == anoinicial);
                valIni = ((valIni * crec.PorcentajeCrecimiento) / 100) + valIni;
                ingresos.Add(valIni);
                anoinicial++;
            }

            return ingresos;
        }
    }
}
