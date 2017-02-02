//@Copyright Yordan Campos Piedra

namespace UCR.Negotium.Domain
{
    public class ObjetoInteresProyecto
    {
        public int CodObjetoInteres { get; set; }
        public string DescripcionObjetoInteres { get; set; }
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
