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
        private bool variableInteres;

        public InteresFinanciamiento() { porcentajeInteresFinanciamiento = 0; }

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

        public bool VariableInteres
        {
            get
            {
                return variableInteres;
            }

            set
            {
                variableInteres = value;
            }
        }
    }
}
