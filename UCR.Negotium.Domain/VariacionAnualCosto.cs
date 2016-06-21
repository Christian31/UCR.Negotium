using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class VariacionAnualCosto
    {
        private int codVariacionCosto;
        private int ano;
        private double procentajeIncremento;

        public int CodVariacionCosto
        {
            get
            {
                return codVariacionCosto;
            }

            set
            {
                codVariacionCosto = value;
            }
        }

        public int Ano
        {
            get
            {
                return ano;
            }

            set
            {
                ano = value;
            }
        }

        public double ProcentajeIncremento
        {
            get
            {
                return procentajeIncremento;
            }

            set
            {
                procentajeIncremento = value;
            }
        }
    }
}
