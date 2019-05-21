using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class VariacionAnualCostoPorTipo
    {
        private TipoAplicacionPorcentaje tipoVariacionAnual;
        private List<VariacionAnualCosto> variacionAnualCostos;

        public VariacionAnualCostoPorTipo()
        {
            variacionAnualCostos = new List<VariacionAnualCosto>();
        }

        public List<VariacionAnualCosto> VariacionAnual
        {
            get { return variacionAnualCostos; }
            set { variacionAnualCostos = value; }
        }

        public TipoAplicacionPorcentaje TipoVariacion
        {
            get { return tipoVariacionAnual; }
            set { tipoVariacionAnual = value; }
        }
    }
}
