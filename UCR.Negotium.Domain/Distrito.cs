namespace UCR.Negotium.Domain
{
    public class Distrito
    {
        public int CodDistrito { get; set; }
        public string NombreDistrito { get; set; }

        public override string ToString()
        {
            return NombreDistrito;
        }

    }
}
