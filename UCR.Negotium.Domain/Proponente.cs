namespace UCR.Negotium.Domain
{
    public class Proponente
    {
        public int IdProponente { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NumIdentificacion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string PuestoEnOrganizacion { get; set; }
        public char Genero { get; set; }
        public bool EsRepresentanteIndividual { get; set; }
        private OrganizacionProponente organizacion;

        public Proponente()
        {
            organizacion = new OrganizacionProponente();
        }

        public override string ToString()
        {
            return Nombre + " " + Apellidos;
        }

        public bool GeneroBoolean
        {
            get
            {
                return Genero == 'm' ? true : false;
            }
            set
            {
                Genero = value.Equals(true) ? 'm' : 'f';
            }
        }

        public OrganizacionProponente Organizacion
        {
            get
            {
                return organizacion;
            }

            set
            {
                organizacion = value;
            }
        }
    }
}
