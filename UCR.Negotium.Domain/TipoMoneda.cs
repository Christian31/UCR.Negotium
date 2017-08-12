namespace UCR.Negotium.Domain
{
    public class TipoMoneda
    {
        public int CodMoneda { get; set; }
        public string NombreMoneda { get; set; }
        public string SignoMoneda { get; set; }

        public TipoMoneda()
        {
            
        }

        public override string ToString()
        {
            return NombreMoneda;
        }
    }
}
