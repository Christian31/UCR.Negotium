namespace UCR.Negotium.Domain
{
    public class InteresFinanciamiento
    {
        public int CodInteresFinanciamiento { get; set; }
        public double PorcentajeInteres { get; set; }
        public int AnoInteres { get; set; }

        public InteresFinanciamiento()
        {
            PorcentajeInteres = 0;
        }
    }
}
