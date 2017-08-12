using System;

namespace UCR.Negotium.Domain
{
    public class Reinversion
    {
        public int CodRequerimientoReinversion { get; set; }
        public int AnoReinversion { get; set; }
        public string DescripcionRequerimiento { get; set; }
        public double Cantidad { get; set; }
        public double CostoUnitario { get; set; }
        public bool Depreciable { get; set; }
        public int VidaUtil { get; set; }
        private UnidadMedida unidadMedida;
        private double depreciacion;
        private double subtotal;
        //can be null
        public int CodRequerimientoInversion { get; set; }
        
        public Reinversion()
        {
            this.UnidadMedida = new UnidadMedida();
        }

        public double Subtotal
        {
            get { return this.CostoUnitario * this.Cantidad; }
            set { this.subtotal = value; }
        }

        public string CostoUnitarioFormat { get; set; }

        public string SubtotalFormat { get; set; }

        public double Depreciacion
        {
            get { return Math.Round((this.CostoUnitario * this.Cantidad) / this.VidaUtil, 2); }
            set { this.depreciacion = value; }
        }

        public UnidadMedida UnidadMedida
        {
            get { return unidadMedida; }
            set { unidadMedida = value; }
        }
    }
}
