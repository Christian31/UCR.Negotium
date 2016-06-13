//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Provincia
    {
        int codProvincia;
        String nombreProvincia;

        public int CodProvincia
        {
            get
            {
                return codProvincia;
            }

            set
            {
                codProvincia = value;
            }
        }

        public string NombreProvincia
        {
            get
            {
                return nombreProvincia;
            }

            set
            {
                nombreProvincia = value;
            }
        }
    }
}
