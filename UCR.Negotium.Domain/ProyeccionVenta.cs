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
    }
}
