using Microsoft.VisualBasic;
using System;
using System.Data;

namespace UCR.Negotium.Extensions
{
    public class IndicadoresSociales
    {
        private double vacDouble;
        private string vac;
        private double[] flujoCajaSinBase;
        private double montoInicial;

        public IndicadoresSociales(int horizonteEvaluacion, string signoMoneda, DataTable dtflujoCaja, double tasaCostoCapital)
        {
            double[] flujoCaja = new double[horizonteEvaluacion + 1];
            for (int i = 0; i <= horizonteEvaluacion; i++)
            {
                flujoCaja[i] = Convert.ToDouble(dtflujoCaja.Rows[5][i + 1].ToString().Replace(signoMoneda, string.Empty));
            }

            flujoCajaSinBase = new double[horizonteEvaluacion];
            montoInicial = flujoCaja[0];
            for (int k = 0; k < horizonteEvaluacion; k++)
            {
                flujoCajaSinBase[k] = flujoCaja[k + 1];
            }

            vacDouble = CalculateVAC(tasaCostoCapital);
            vac = signoMoneda + " " + (vacDouble).ToString("#,##0.##");
        }

        #region PublicMethods

        public double CalculateVAC(double tasaCostoCapital)
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

        #endregion

        #region Properties
        public string VAC
        {
            get { return vac; }
        }

        public double VACDouble
        {
            get { return vacDouble; }
        }

        #endregion

        #region PrivateMethods
        


        #endregion
    }
}
