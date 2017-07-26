using System;

namespace UCR.Negotium.Domain
{
    public class Inversion
    {
        public int CodRequerimientoInversion { get; set; }
        public string DescripcionRequerimiento { get; set; }
        public int Cantidad { get; set; }
        public double CostoUnitario { get; set; }
        public bool Depreciable { get; set; }
        public int VidaUtil { get; set; }
        private UnidadMedida unidadMedida;
        private double depreciacion;

        public Inversion()
        {
            //datos por defecto
            unidadMedida = new UnidadMedida();
            Cantidad = 1;
            CostoUnitario = 1;
            VidaUtil = 2;
        }

        public override string ToString()
        {
            return DescripcionRequerimiento;
        }

        public string CostoUnitarioFormat
        {
            get
            {
                return "₡ " + CostoUnitario.ToString("#,##0.##");
            }
        }

        public double Subtotal
        {
            get
            {
                return this.Cantidad * this.CostoUnitario;
            }
        }

        public string SubtotalFormat
        {
            get
            {
                return "₡ " + Subtotal.ToString("#,##0.##");
            }
        }

        public double Depreciacion
        {
            get
            {
                return Math.Round((this.CostoUnitario * this.Cantidad) / this.VidaUtil, 2);
            }
            set
            {
                this.depreciacion = value;
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
