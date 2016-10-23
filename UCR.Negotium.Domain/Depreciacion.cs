using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Depreciacion
    {
        public int CodDepresiacion { get; set; }
        public string NombreDepreciacion { get; set; }
        public List<double> MontoDepreciacion { get; set; }

        public Depreciacion()
        {
            MontoDepreciacion = new List<double>();
        }
    }
}
