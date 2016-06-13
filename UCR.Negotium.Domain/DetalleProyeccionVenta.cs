using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class DetalleProyeccionVenta
    {
        private int codDetalle;
        private int mes;
        private double cantidad;
        private double precio;

        public int CodDetalle
        {
            get
            {
                return codDetalle;
            }

            set
            {
                codDetalle = value;
            }
        }

        public int Mes
        {
            get
            {
                return mes;
            }

            set
            {
                mes = value;
            }
        }

        public double Cantidad
        {
            get
            {
                return cantidad;
            }

            set
            {
                cantidad = value;
            }
        }

        public double Precio
        {
            get
            {
                return precio;
            }

            set
            {
                precio = value;
            }
        }
    }
}
