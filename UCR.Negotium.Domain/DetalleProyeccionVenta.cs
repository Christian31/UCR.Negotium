﻿namespace UCR.Negotium.Domain
{
    public class DetalleProyeccionVenta
    {
        public int CodDetalle { get; set; }
        public int Mes { get; set; }
        public double Cantidad { get; set; }
        public double Precio { get; set; }  

        public DetalleProyeccionVenta() { }

        public double Subtotal
        {
            get
            {
                return this.Cantidad * this.Precio;
            }
        }

        public string SubtotalFormat
        {
            get
            {
                return "₡ " + Subtotal.ToString("#,##0.##");
            }
        }
    }
}
