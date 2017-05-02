using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class ProyeccionVentaArticulo
    {
        public int CodArticulo { get; set; }
        public string NombreArticulo { get; set; }

        private List<DetalleProyeccionVenta> detallesProyeccionVenta;
        private UnidadMedida unidadMedida;
        private List<CrecimientoOfertaObjetoInteres> crecimientoOferta;

        public ProyeccionVentaArticulo()
        {
            unidadMedida = new UnidadMedida();
            detallesProyeccionVenta = new List<DetalleProyeccionVenta>();
            crecimientoOferta = new List<CrecimientoOfertaObjetoInteres>();
        }

        public List<CrecimientoOfertaObjetoInteres> CrecimientoOferta
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
