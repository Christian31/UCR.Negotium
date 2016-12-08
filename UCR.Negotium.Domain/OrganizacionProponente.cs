//@Copyright Yordan Campos Piedra
using System;

namespace UCR.Negotium.Domain
{
    public class OrganizacionProponente
    {
        public int CodOrganizacion { get; set; }
        public String NombreOrganizacion { get; set; }
        public String CedulaJuridica { get; set; }
        public String Telefono { get; set; }
        public String Descripcion { get; set; }
        public TipoOrganizacion tipo;

        public OrganizacionProponente()
        {
            tipo = new TipoOrganizacion();
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
