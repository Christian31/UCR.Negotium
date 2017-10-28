using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class Depreciacion
    {
        public int CodDepresiacion { get; set; }
        public string NombreDepreciacion { get; set; }
        public List<double> MontoDepreciacion { get; set; }
        public List<string> MontoDepreciacionFormat { get; set; }

        public Depreciacion()
        {
            MontoDepreciacion = new List<double>();
            MontoDepreciacionFormat = new List<string>();
        }
    }
}
