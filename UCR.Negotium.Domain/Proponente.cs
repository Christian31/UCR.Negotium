//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Proponente
    {
        public int IdProponente { get; set; }
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String NumIdentificacion { get; set; }
        public String Telefono { get; set; }
        public String Email { get; set; }
        public String PuestoEnOrganizacion { get; set; }
        public char Genero { get; set; }
        private OrganizacionProponente organizacion;

        public Proponente()
        {
            organizacion = new OrganizacionProponente();
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
