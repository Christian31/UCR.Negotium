using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class ProyeccionVentaArticulo
    {
        public int CodArticulo { get; set; }
        public String NombreArticulo { get; set; }

        private List<DetalleProyeccionVenta> detallesProyeccionVenta;
        private UnidadMedida unidadMedida;

        public ProyeccionVentaArticulo() {
            unidadMedida = new UnidadMedida();
            detallesProyeccionVenta = new List<DetalleProyeccionVenta>();
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
