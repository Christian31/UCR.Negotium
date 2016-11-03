using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Financiamiento
    {
        public int CodFinanciamiento { get; set; }
        public Double MontoFinanciamiento { get; set; }
        public Boolean VariableFinanciamiento { get; set; }
        public int TiempoFinanciamiento { get; set; }

        public Financiamiento()
        {
            
        }
    }
}
