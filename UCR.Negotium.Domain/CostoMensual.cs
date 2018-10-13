using UCR.Negotium.Base.Utilidades;

namespace UCR.Negotium.Domain
{
    public class CostoMensual
    {
        public int CodCostoMensual { get; set; }
        public string Mes { get; set; }
        public double CostoUnitario { get; set; }
        public double Cantidad { get; set; }
        private double subtotal { get; set; }

        public CostoMensual() { }

        public double Subtotal
        {
            get
            {
                return (this.Cantidad * this.CostoUnitario).PonderarNumero();
            }
        }

        public string SubtotalFormat { get; set; }
        public string CostoUnitarioFormat { get; set; }
    }
}
