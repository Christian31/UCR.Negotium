using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class Canton
    {
        public int CodCanton { get; set; }
        public string NombreCanton { get; set; }
        private List<Distrito> distritos;

        public Canton()
        {
            distritos = new List<Distrito>();
        }

        public override string ToString()
        {
            return NombreCanton;
        }

        public List<Distrito> Distritos
        {
            get
            {
                return distritos;
            }
            set
            {
                distritos = value;
            }
        }
    }
}
