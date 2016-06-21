using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Costo
    {
        private int codCosto;
        private UnidadMedida unidadMedida;
        private string nombreCosto;
        private List<CostoMensual> costosMensuales;

        public Costo()
        {
            unidadMedida = new UnidadMedida();
            costosMensuales = new List<CostoMensual>();
        }

        public int CodCosto
        {
            get
            {
                return codCosto;
            }

            set
            {
                codCosto = value;
            }
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

        public string NombreCosto
        {
            get
            {
                return nombreCosto;
            }

            set
            {
                nombreCosto = value;
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
