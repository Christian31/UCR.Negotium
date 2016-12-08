namespace UCR.Negotium.Domain
{
    public class CostoMensual
    {
        public int CodCostoMensual { get; set; }
        public int Mes { get; set; }
        public double CostoUnitario { get; set; }
        public double Cantidad { get; set; }
        private double subtotal { get; set; }

        public CostoMensual() { }

        public double Subtotal
        {
            get
            {
                return this.Cantidad * this.CostoUnitario;
            }
        }
    }
}
