using Microsoft.VisualBasic;
using System;
using System.Data;

namespace UCR.Negotium.Extensions
{
    public class IndicadoresFinancieros
    {
        private double vanDouble, relacionBC, pri;
        private string tir, van;
        private double[] flujoCajaSinBase, ventasSinBase, costosSinBase;
        private double montoInicial, ventaInicial, costoInicial;
        
        public IndicadoresFinancieros(int horizonteEvaluacion, string signoMoneda, DataTable dtflujoCaja, double tasaCostoCapital)
        {
            double[] flujoCaja = new double[horizonteEvaluacion + 1];
            for (int i = 0; i <= horizonteEvaluacion; i++)
            {
                flujoCaja[i] = Convert.ToDouble(dtflujoCaja.Rows[16][i + 1].ToString().Replace(signoMoneda, string.Empty));
            }

            flujoCajaSinBase = new double[horizonteEvaluacion];
            montoInicial = flujoCaja[0];
            for (int k = 0; k < horizonteEvaluacion; k++)
            {
                flujoCajaSinBase[k] = flujoCaja[k + 1];
            }

            ventasSinBase = new double[horizonteEvaluacion - 1];
            ventaInicial = Convert.ToDouble(dtflujoCaja.Rows[0][2].ToString().Replace(signoMoneda, string.Empty));
            for (int i = 0; i <= horizonteEvaluacion-2; i++)
            {
                ventasSinBase[i] = Convert.ToDouble(dtflujoCaja.Rows[0][i + 3].ToString().Replace(signoMoneda, string.Empty));
            }

            costosSinBase = new double[horizonteEvaluacion - 1];
            costoInicial = Convert.ToDouble(dtflujoCaja.Rows[1][2].ToString().Replace(signoMoneda, string.Empty)) * -1;
            for (int i = 0; i <= horizonteEvaluacion-2; i++)
            {
                costosSinBase[i] = Convert.ToDouble(dtflujoCaja.Rows[1][i + 3].ToString().Replace(signoMoneda, string.Empty)) * -1;
            }

            vanDouble = CalculateVAN(tasaCostoCapital);
            van = signoMoneda + " " + (vanDouble).ToString("#,##0.##");
            CalculateTIR(flujoCaja);
            CalculatePRI(flujoCaja);
            relacionBC = CalculateRelacionBC(tasaCostoCapital);
        }

        #region PublicMethods
        public double CalculateVAN(double tasaCostoCapital)
        {
            double vanResult = 0;
            double tasaTemp = tasaCostoCapital / 100;

            try
            {
                double num2 = Financial.NPV(tasaTemp, ref flujoCajaSinBase);
                vanResult = num2 + montoInicial;
            }
            catch { vanResult = 0; }

            return vanResult;
        }

        public double CalculateRelacionBC(double tasaCostoCapital)
        {
            double relacionBCResult;
            tasaCostoCapital = tasaCostoCapital / 100;

            try
            {
                //beneficio
                double num1 = Financial.NPV(tasaCostoCapital, ref ventasSinBase);
                double tempVan1 = num1 + ventaInicial;

                //costo
                double num2 = Financial.NPV(tasaCostoCapital, ref costosSinBase);
                double tempVan2 = num2 + costoInicial;

                relacionBCResult = Math.Round((tempVan1 / tempVan2), 2);
            }
            catch { relacionBCResult = 0; }

            if (Double.IsNaN(relacionBCResult))
                relacionBCResult = 0;

            return relacionBCResult;
        }
        #endregion

        #region Properties
        public string VAN
        {
            get { return van; }
        }

        public double VANDouble
        {
            get { return vanDouble; }
        }

        public string TIR
        {
            get { return tir; }
        }

        public double PRI
        {
            get { return pri; }
        }

        public double RelacionBC
        {
            get { return relacionBC; }
        }

        #endregion

        #region PrivateMethods
        private void CalculateTIR(double[] flujoCaja)
        {
            tir = "";
            try
            {
                double num1 = Financial.IRR(ref flujoCaja) * 100;
                tir = string.Concat(num1.ToString("#,##0.##"), " %");
            }
            catch { tir = "Indefinido"; }
        }

        private void CalculatePRI(double[] flujoCaja)
        {
            pri = 0;
            try
            {
                double[] flujoCajaAcumulado = new double[flujoCaja.Length];
                for (int i = 0; i < flujoCaja.Length; i++)
                {
                    if (i == 0)
                        flujoCajaAcumulado[i] = flujoCaja[i];
                    else
                        flujoCajaAcumulado[i] = flujoCajaAcumulado[i - 1] + flujoCaja[i];
                }

                int topeNegativo = 0;
                while (topeNegativo < flujoCajaAcumulado.Length && flujoCajaAcumulado[topeNegativo] < 0)
                    topeNegativo++;

                if(topeNegativo > 0)
                    topeNegativo--;

                double priTemp = topeNegativo + (Math.Abs(flujoCajaAcumulado[topeNegativo]) / flujoCaja[topeNegativo + 1]);

                pri = Math.Round(priTemp, 2);
            }
            catch { pri = 0; }

            if (Double.IsNaN(pri))
                pri = 0;
        }        
        #endregion

    }
}
