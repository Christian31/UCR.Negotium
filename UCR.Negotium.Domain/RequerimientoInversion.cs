//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class RequerimientoInversion
    {
        public int CodRequerimientoInversion { get; set; }
        public String DescripcionRequerimiento { get; set; }
        public int Cantidad { get; set; }
        public double CostoUnitario { get; set; }
        public bool Depreciable { get; set; }
        public int VidaUtil { get; set; }
        private UnidadMedida unidadMedida;
        private double depreciacion;

        public RequerimientoInversion()
        {
            unidadMedida = new UnidadMedida();
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
