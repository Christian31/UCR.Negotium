namespace UCR.Negotium.Domain
{
    public class TipoOrganizacion
    {
        public string Descripcion { get; set; }
        public int CodTipo { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
