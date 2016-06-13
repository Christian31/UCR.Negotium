using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class CrecimientoOfertaObjetoInteres
    {
        private int codCrecimiento;
        private int anoCrecimiento;
        private double porcentajeCrecimiento;

        public int CodCrecimiento
        {
            get
            {
                return codCrecimiento;
            }

            set
            {
                codCrecimiento = value;
            }
        }

        public int AnoCrecimiento
        {
            get
            {
                return anoCrecimiento;
            }

            set
            {
                anoCrecimiento = value;
            }
        }

        public double PorcentajeCrecimiento
        {
            get
            {
                return porcentajeCrecimiento;
            }

            set
            {
                porcentajeCrecimiento = value;
            }
        }
    }
}
