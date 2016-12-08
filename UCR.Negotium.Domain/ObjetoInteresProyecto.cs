//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class ObjetoInteresProyecto
    {
        public int CodObjetoInteres;
        public String DescripcionObjetoInteres;
        private UnidadMedida unidadMedida;

        public ObjetoInteresProyecto()
        {
            unidadMedida = new UnidadMedida();
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
    }
}
