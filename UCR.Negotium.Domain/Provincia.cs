using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class Provincia
    {
        public int CodProvincia { get; set; }
        public string NombreProvincia { get; set; }
        private List<Canton> cantones;

        public Provincia()
        {
            cantones = new List<Canton>();
        }

        public override string ToString()
        {
            return NombreProvincia;
        }

        public List<Canton> Cantones
        {
            get
            {
                return cantones;
            }
            set
            {
                cantones = value;
            }
        }
    }
}
