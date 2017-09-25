namespace UCR.Negotium.Domain
{
    public class OrganizacionProponente
    {
        public int CodOrganizacion { get; set; }
        public string NombreOrganizacion { get; set; }
        public string CedulaJuridica { get; set; }
        public string Telefono { get; set; }
        public string Descripcion { get; set; }
        public string CorreoElectronico { get; set; }

        private TipoOrganizacion tipo;
        private Proponente proponente;

        public OrganizacionProponente()
        {
            tipo = new TipoOrganizacion();
            proponente = new Proponente();
        }

        public Proponente Proponente
        {
            get
            {
                return proponente;
            }

            set
            {
                proponente = value;
            }
        }

        public TipoOrganizacion Tipo
        {
            get
            {
                return tipo;
            }

            set
            {
                tipo = value;
            }
        }
    }
}
