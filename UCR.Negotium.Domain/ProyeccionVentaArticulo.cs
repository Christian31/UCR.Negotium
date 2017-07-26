using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class ProyeccionVentaArticulo
    {
        public int CodArticulo { get; set; }
        public string NombreArticulo { get; set; }

        private List<DetalleProyeccionVenta> detallesProyeccionVenta;
        private UnidadMedida unidadMedida;
        private List<CrecimientoOfertaArticulo> crecimientoOferta;

        public ProyeccionVentaArticulo()
        {
            unidadMedida = new UnidadMedida();
            detallesProyeccionVenta = new List<DetalleProyeccionVenta>() { new DetalleProyeccionVenta { Mes= "Enero"},
                new DetalleProyeccionVenta { Mes= "Febrero"}, new DetalleProyeccionVenta { Mes= "Marzo"},
                new DetalleProyeccionVenta { Mes= "Abril"}, new DetalleProyeccionVenta { Mes= "Mayo"},
                new DetalleProyeccionVenta { Mes= "Junio"}, new DetalleProyeccionVenta { Mes= "Julio"},
                new DetalleProyeccionVenta { Mes= "Agosto"} ,new DetalleProyeccionVenta { Mes= "Setiembre"},
                new DetalleProyeccionVenta { Mes= "Octubre"}, new DetalleProyeccionVenta { Mes= "Noviembre"},
                new DetalleProyeccionVenta { Mes= "Diciembre"}};

            crecimientoOferta = new List<CrecimientoOfertaArticulo>();
        }

        public List<CrecimientoOfertaArticulo> CrecimientoOferta
        {
            get
            {
                return crecimientoOferta;
            }
            set
            {
                crecimientoOferta = value;
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

        public List<DetalleProyeccionVenta> DetallesProyeccionVenta
        {
            get
            {
                return detallesProyeccionVenta;
            }

            set
            {
                detallesProyeccionVenta = value;
            }
        }
    }
}
