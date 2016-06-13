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
        int idProponente;
        String nombre;
        String apellidos;
        String numIdentificacion;
        String telefono;
        String email;
        String puestoEnOrganizacion;
        char genero;
        OrganizacionProponente organizacion;

        public Proponente()
        {
            organizacion = new OrganizacionProponente();
        }

        public int IdProponente
        {
            get
            {
                return idProponente;
            }

            set
            {
                idProponente = value;
            }
        }

        public string Nombre
        {
            get
            {
                return nombre;
            }

            set
            {
                nombre = value;
            }
        }

        public string Apellidos
        {
            get
            {
                return apellidos;
            }

            set
            {
                apellidos = value;
            }
        }

        public string NumIdentificacion
        {
            get
            {
                return numIdentificacion;
            }

            set
            {
                numIdentificacion = value;
            }
        }

        public string Telefono
        {
            get
            {
                return telefono;
            }

            set
            {
                telefono = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public string PuestoEnOrganizacion
        {
            get
            {
                return puestoEnOrganizacion;
            }

            set
            {
                puestoEnOrganizacion = value;
            }
        }

        public char Genero
        {
            get
            {
                return genero;
            }

            set
            {
                genero = value;
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
