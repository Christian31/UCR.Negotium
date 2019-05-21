using System.Collections.Generic;

namespace UCR.Negotium.Domain
{
    public class CrecimientoOfertaPorTipo
    {
        private List<CrecimientoOferta> crecimientoOferta;
        private TipoAplicacionPorcentaje tipoCrecimiento;

        public CrecimientoOfertaPorTipo()
        {
            crecimientoOferta = new List<CrecimientoOferta>();
        }

        public List<CrecimientoOferta> CrecimientoOferta
        {
            get { return crecimientoOferta; }
            set { crecimientoOferta = value; }
        }

        public TipoAplicacionPorcentaje TipoCrecimiento
        {
            get { return tipoCrecimiento; }
            set { tipoCrecimiento = value; }
        }
    }

    public enum TipoAplicacionPorcentaje
    {
        PorPrecio = 1,
        PorCantidad = 2
    }
}
