using System;

namespace UCR.Negotium.Domain
{
    public class Financiamiento
    {
        public int CodFinanciamiento { get; set; }
        public double MontoFinanciamiento { get; set; }
        public bool InteresFijo { get; set; }
        public int TiempoFinanciamiento { get; set; }
        public int AnoInicialPago { get; set; }

        public Financiamiento()
        {
            
        }
    }
}
