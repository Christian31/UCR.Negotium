﻿using System;

namespace UCR.Negotium.Domain
{
    public class CostoMensual
    {
        public int CodCostoMensual { get; set; }
        public string Mes { get; set; }
        public double CostoUnitario { get; set; }
        public double Cantidad { get; set; }
        private double subtotal { get; set; }

        public CostoMensual() { }

        public double Subtotal
        {
            get
            {
                return Math.Round(this.Cantidad * this.CostoUnitario, 2);
            }
        }

        public string SubtotalFormat { get; set; }
        public string CostoUnitarioFormat { get; set; }
    }
}
