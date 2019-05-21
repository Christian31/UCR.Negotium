using UCR.Negotium.Base.Utilidades;

namespace UCR.Negotium.Domain
{
    public class Inversion
    {
        public int CodInversion { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; set; }
        public double CostoUnitario { get; set; }
        public bool Depreciable { get; set; }
        public int VidaUtil { get; set; }
        private UnidadMedida unidadMedida;
        private double depreciacion;

        public Inversion()
        {
            //datos por defecto
            unidadMedida = new UnidadMedida();
            VidaUtil = 2;
        }

        public override string ToString()
        {
            return Descripcion;
        }

        public string CostoUnitarioFormat { get; set; }

        public double Subtotal
        {
            get
            {
                return (this.Cantidad * this.CostoUnitario).PonderarNumero();
            }
        }

        public string SubtotalFormat { get; set; }

        public double Depreciacion
        {
            get
            {
                return ((this.CostoUnitario * this.Cantidad) / this.VidaUtil).PonderarNumero();
            }
            set
            {
                this.depreciacion = value;
            }
        }

        public UnidadMedida UnidadMedida
        {
            get { return unidadMedida; }
            set { unidadMedida = value; }
        }
    }
}
