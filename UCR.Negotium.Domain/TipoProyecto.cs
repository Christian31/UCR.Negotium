namespace UCR.Negotium.Domain
{
    public class TipoProyecto
    {
        public string Nombre { get; set; }
        public int CodTipo { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
