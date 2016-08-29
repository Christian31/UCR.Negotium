using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Financiamiento
    {
        private int codFinanciamiento;
        private Double montoFinanciamiento;
        private Boolean variableFinanciamiento;
        private int tiempoFinanciamiento;

        public Financiamiento()
        {
            
        }

        public int CodFinanciamiento
        {
            get
            {
                return codFinanciamiento;
            }

            set
            {
                codFinanciamiento = value;
            }
        }

        public double MontoFinanciamiento
        {
            get
            {
                return montoFinanciamiento;
            }

            set
            {
                montoFinanciamiento = value;
            }
        }

        public bool VariableFinanciamiento
        {
            get
            {
                return variableFinanciamiento;
            }

            set
            {
                variableFinanciamiento = value;
            }
        }

        public int TiempoFinanciamiento
        {
            get
            {
                return tiempoFinanciamiento;
            }

            set
            {
                tiempoFinanciamiento = value;
            }
        }
    }
}
