//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class RequerimientoReinversion
    {
        private int codRequerimientoReinversion;
        private int anoReinversion;
        private String descripcionRequerimiento;
        private int cantidad;
        private double costoUnitario;
        private bool depreciable;
        private int vidaUtil;
        private UnidadMedida unidadMedida;
        private double depreciacion;

        //can be null
        public int CodRequerimientoInversion { get; set; }
        
        public RequerimientoReinversion()
        {
            this.UnidadMedida = new UnidadMedida();
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

        public int CodRequerimientoReinversion
        {
            get
            {
                return codRequerimientoReinversion;
            }

            set
            {
                codRequerimientoReinversion = value;
            }
        }

        public int AnoReinversion
        {
            get
            {
                return anoReinversion;
            }

            set
            {
                anoReinversion = value;
            }
        }

        public string DescripcionRequerimiento
        {
            get
            {
                return descripcionRequerimiento;
            }

            set
            {
                descripcionRequerimiento = value;
            }
        }

        public int Cantidad
        {
            get
            {
                return cantidad;
            }

            set
            {
                cantidad = value;
            }
        }

        public double CostoUnitario
        {
            get
            {
                return costoUnitario;
            }

            set
            {
                costoUnitario = value;
            }
        }

        public bool Depreciable
        {
            get
            {
                return depreciable;
            }

            set
            {
                depreciable = value;
            }
        }

        public int VidaUtil
        {
            get
            {
                return vidaUtil;
            }

            set
            {
                vidaUtil = value;
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
