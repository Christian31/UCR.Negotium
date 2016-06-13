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
        private int codObjetoInteres;
        private String descripcionObjetoInteres;
        private UnidadMedida unidadMedida;

        public int CodObjetoInteres
        {
            get
            {
                return codObjetoInteres;
            }

            set
            {
                codObjetoInteres = value;
            }
        }

        public string DescripcionObjetoInteres
        {
            get
            {
                return descripcionObjetoInteres;
            }

            set
            {
                descripcionObjetoInteres = value;
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
    }
}
