namespace UCR.Negotium.Domain
{
    public class DetalleProyeccionVenta
    {
        public int CodDetalle { get; set; }
        public string Mes { get; set; }
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

        public string SubtotalFormat { get; set; }
    }
}
