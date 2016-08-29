using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class InteresFinanciamiento
    {
        private int codInteresFinanciamiento;
        private Double porcentajeInteresFinanciamiento;

        public InteresFinanciamiento() { }

        public int CodInteresFinanciamiento
        {
            get
            {
                return codInteresFinanciamiento;
            }

            set
            {
                codInteresFinanciamiento = value;
            }
        }

        public double PorcentajeInteresFinanciamiento
        {
            get
            {
                return porcentajeInteresFinanciamiento;
            }

            set
            {
                porcentajeInteresFinanciamiento = value;
            }
        }
    }
}
