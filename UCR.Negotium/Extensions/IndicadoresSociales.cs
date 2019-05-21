using Microsoft.VisualBasic;
using System;
using System.Data;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Extensions
{
    public class IndicadoresSociales
    {
        private IndicadorEconomico vac;
        private string signoMoneda;
        private double[] flujoCajaSinBase;
        private double montoInicial;

        public IndicadoresSociales(int horizonteEvaluacion, string signoMoneda, DataTable dtflujoCaja, double tasaCostoCapital)
        {
            this.signoMoneda = signoMoneda;
            double[] flujoCaja = new double[horizonteEvaluacion + 1];
            for (int i = 0; i <= horizonteEvaluacion; i++)
            {
                flujoCaja[i] = Convert.ToDouble(dtflujoCaja.Rows[6][i + 1].ToString().Replace(signoMoneda, string.Empty));
            }

            flujoCajaSinBase = new double[horizonteEvaluacion];
            montoInicial = flujoCaja[0];
            for (int k = 0; k < horizonteEvaluacion; k++)
            {
                flujoCajaSinBase[k] = flujoCaja[k + 1];
            }

            vac = CalculateVAC(tasaCostoCapital);
        }

        #region PublicMethods

        public IndicadorEconomico CalculateVAC(double tasaCostoCapital)
        {
            IndicadorEconomico vacResult = new IndicadorEconomico(signoMoneda: signoMoneda);
            vacResult.EsEvaluablePorCantidad = true;
            double tasaTemp = tasaCostoCapital / 100;

            try
            {
                double num2 = Financial.NPV(tasaTemp, ref flujoCajaSinBase);
                vacResult.Resultado = num2 + montoInicial;
            }
            catch
            {
                vacResult.ConError = true;
            }

            return vacResult;
        }

        #endregion

        #region Properties
        public IndicadorEconomico VAC
        {
            get { return vac; }
        }
        #endregion
    }
}
