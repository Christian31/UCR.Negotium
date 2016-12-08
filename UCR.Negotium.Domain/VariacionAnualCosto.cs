using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class VariacionAnualCosto
    {
        public int CodVariacionCosto { get; set; }
        public int Ano { get; set; }
        public double PorcentajeIncremento { get; set; }
    }
}
