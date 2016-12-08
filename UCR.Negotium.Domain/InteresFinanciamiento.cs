using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class InteresFinanciamiento
    {
        public int CodInteresFinanciamiento;
        public Double PorcentajeInteresFinanciamiento;
        public bool VariableInteres;

        public InteresFinanciamiento()
        {
            PorcentajeInteresFinanciamiento = 0;
        }
    }
}
