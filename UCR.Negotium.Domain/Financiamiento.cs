using System;

namespace UCR.Negotium.Domain
{
    public class Financiamiento
    {
        public int CodFinanciamiento { get; set; }
        public double MontoFinanciamiento { get; set; }
        public bool VariableFinanciamiento { get; set; }
        public int TiempoFinanciamiento { get; set; }

        public Financiamiento()
        {
            
        }
    }
}
