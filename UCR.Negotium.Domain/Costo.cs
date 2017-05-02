using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class Costo
    {
        public int CodCosto { get; set; }
        private UnidadMedida unidadMedida;
        public string NombreCosto { get; set; }
        public string CategoriaCosto { get; set; }
        public int AnoCosto { get; set; }
        private List<CostoMensual> costosMensuales;

        public Costo()
        {
            unidadMedida = new UnidadMedida();
            costosMensuales = new List<CostoMensual>() { new CostoMensual { Mes= "Enero"},
                new CostoMensual { Mes= "Febrero"}, new CostoMensual { Mes= "Marzo"},
                new CostoMensual { Mes= "Abril"}, new CostoMensual { Mes= "Mayo"},
                new CostoMensual { Mes= "Junio"}, new CostoMensual { Mes= "Julio"},
                new CostoMensual { Mes= "Agosto"} ,new CostoMensual { Mes= "Setiembre"},
                new CostoMensual { Mes= "Octubre"}, new CostoMensual { Mes= "Noviembre"},
                new CostoMensual { Mes= "Diciembre"}};
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
