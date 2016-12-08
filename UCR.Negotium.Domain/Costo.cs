using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Costo
    {
        public int CodCosto { get; set; }
        private UnidadMedida unidadMedida;
        public string NombreCosto { get; set; }
        public Boolean CostoVariable { get; set; }
        public string CategoriaCosto { get; set; }
        public int AnoCosto { get; set; }
        private List<CostoMensual> costosMensuales;

        public Costo()
        {
            unidadMedida = new UnidadMedida();
            costosMensuales = new List<CostoMensual>();
        }

        public UnidadMedida UnidadMedida
        {
            get
            {
                return unidadMedida;
            }

            set
            {
                unidadMedida = value;
            }
        }

        public List<CostoMensual> CostosMensuales
        {
            get
            {
                return costosMensuales;
            }

            set
            {
                costosMensuales = value;
            }
        }
    }
}
