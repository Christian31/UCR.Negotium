//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Canton
    {
        int codCanton;
        String nombreCanton;

        public int CodCanton
        {
            get
            {
                return codCanton;
            }

            set
            {
                codCanton = value;
            }
        }

        public string NombreCanton
        {
            get
            {
                return nombreCanton;
            }

            set
            {
                nombreCanton = value;
            }
        }
    }
}
