//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class TipoOrganizacion
    {
        String descripcion;
        int codTipo;

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

        public int CodTipo
        {
            get
            {
                return codTipo;
            }

            set
            {
                codTipo = value;
            }
        }
    }
}
