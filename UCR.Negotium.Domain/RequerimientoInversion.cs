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
        private int codRequerimientoInversion;
        private String descripcionRequerimiento;
        private int cantidad;
        private double costoUnitario;
        private bool depreciable;
        private int vidaUtil;
        private UnidadMedida unidadMedida;

        public RequerimientoInversion()
        {
            unidadMedida = new UnidadMedida();
        }

        public int CodRequerimientoInversion
        {
            get
            {
                return codRequerimientoInversion;
            }

            set
            {
                codRequerimientoInversion = value;
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
