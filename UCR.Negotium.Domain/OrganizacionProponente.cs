//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class OrganizacionProponente
    {
        int codOrganizacion;
        String nombreOrganizacion, cedulaJuridica, telefono, descripcion;
        TipoOrganizacion tipo;

        public OrganizacionProponente()
        {
            tipo = new TipoOrganizacion();
        }

        public string CedulaJuridica
        {
            get
            {
                return cedulaJuridica;
            }

            set
            {
                cedulaJuridica = value;
            }
        }

        public int CodOrganizacion
        {
            get
            {
                return codOrganizacion;
            }

            set
            {
                codOrganizacion = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return descripcion;
            }

            set
            {
                descripcion = value;
            }
        }

        public string NombreOrganizacion
        {
            get
            {
                return nombreOrganizacion;
            }

            set
            {
                nombreOrganizacion = value;
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
