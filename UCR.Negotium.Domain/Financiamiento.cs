using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class Financiamiento
    {
        public int CodFinanciamiento { get; set; }
        public double MontoFinanciamiento { get; set; }
        public bool InteresFijo { get; set; }
        public int AnoInicialPago { get; set; }
        public int AnoFinalPago { get; set; }

        private List<InteresFinanciamiento> tasaIntereses;

        public Financiamiento()
        {
            tasaIntereses = new List<InteresFinanciamiento>();
        }

        public List<InteresFinanciamiento> TasaIntereses
        {
            get { return tasaIntereses; }
            set { tasaIntereses = value; }
        }

        public int TiempoFinanciamiento
        {
            get { return AnoFinalPago - AnoInicialPago + 1; }
        }
    }
}
