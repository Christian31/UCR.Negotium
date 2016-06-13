using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class ProyeccionVentaArticulo
    {
        private int codArticulo;
        private UnidadMedida unidadMedida;
        private String nombreArticulo;
        private List<DetalleProyeccionVenta> detallesProyeccionVenta;

        public ProyeccionVentaArticulo() {
            unidadMedida = new UnidadMedida();
            detallesProyeccionVenta = new List<DetalleProyeccionVenta>();
        }

        public int CodArticulo
        {
            get
            {
                return codArticulo;
            }

            set
            {
                codArticulo = value;
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

        public string NombreArticulo
        {
            get
            {
                return nombreArticulo;
            }

            set
            {
                nombreArticulo = value;
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
