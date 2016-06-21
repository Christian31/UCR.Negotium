using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class CostoMensual
    {
        private int codCostoMensual;
        private int mes;
        private double costoUnitario;
        private double cantidad;

        public int CodCostoMensual
        {
            get
            {
                return codCostoMensual;
            }

            set
            {
                codCostoMensual = value;
            }
        }

        public int Mes
        {
            get
            {
                return mes;
            }

            set
            {
                mes = value;
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

        public double Cantidad
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
    }
}
