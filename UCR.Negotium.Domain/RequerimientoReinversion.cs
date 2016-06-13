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
        

        public RequerimientoReinversion()
        {
            
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
    }
}
