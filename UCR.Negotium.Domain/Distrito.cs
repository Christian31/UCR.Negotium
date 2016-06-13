//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Distrito
    {
        int codDistrito;
        String nombreDistrito;

        public int CodDistrito
        {
            get
            {
                return codDistrito;
            }

            set
            {
                codDistrito = value;
            }
        }

        public string NombreDistrito
        {
            get
            {
                return nombreDistrito;
            }

            set
            {
                nombreDistrito = value;
            }
        }
    }
}
