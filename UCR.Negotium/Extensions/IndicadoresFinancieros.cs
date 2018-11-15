using Microsoft.VisualBasic;
using System;
using System.Data;
using UCR.Negotium.Base.Utilidades;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Extensions
{
    public class IndicadoresFinancieros
    {
        private IndicadorEconomico tir, pri, relacionBC, van, relacionBCInvInicial;
        private string signoMoneda;
        private double[] flujoCajaSinBase, ventasSinBase, costosSinBase, ventas, costos;
        private double montoInicial, ventaInicial, costoInicial, inversionInicial;
        
        public IndicadoresFinancieros(int horizonteEvaluacion, string signoMoneda, DataTable dtflujoCaja, double tasaCostoCapital)
        {
            this.signoMoneda = signoMoneda;
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

            ventas = new double[horizonteEvaluacion];
            for (int i = 0; i <= horizonteEvaluacion - 1; i++)
            {
                ventas[i] = Convert.ToDouble(dtflujoCaja.Rows[0][i + 2].ToString().Replace(signoMoneda, string.Empty));
            }

            ventasSinBase = new double[horizonteEvaluacion - 1];
            ventaInicial = ventas[0];
            for (int i = 0; i <= ventas.Length -2; i++)
            {
                ventasSinBase[i] = ventas[i + 1];
            }

            costos = new double[horizonteEvaluacion];

            for (int i = 0; i <= horizonteEvaluacion - 1; i++)
            {
                costos[i] = Convert.ToDouble(dtflujoCaja.Rows[1][i + 2].ToString().Replace(signoMoneda, string.Empty)) * -1;
            }

            costosSinBase = new double[horizonteEvaluacion - 1];
            costoInicial = costos[0];
            for (int i = 0; i <= costos.Length -2; i++)
            {
                costosSinBase[i] = costos[i + 1];
            }

            inversionInicial = Convert.ToDouble(dtflujoCaja.Rows[10][1].ToString().Replace(signoMoneda, string.Empty)) * -1;

            van = CalculateVAN(tasaCostoCapital);
            CalculateTIR(flujoCaja);
            CalculatePRI(flujoCaja);
            relacionBC = CalculateRelacionBC(tasaCostoCapital);
            relacionBCInvInicial = CalculateRelacionBCInvInicial(tasaCostoCapital);
        }

        #region PublicMethods
        public IndicadorEconomico CalculateVAN(double tasaCostoCapital)
        {
            IndicadorEconomico vanResult = new IndicadorEconomico(signoMoneda: signoMoneda);
            vanResult.EsEvaluablePorCantidad = true;
            double tasaTemp = tasaCostoCapital / 100;

            try
            {
                double num2 = Financial.NPV(tasaTemp, ref flujoCajaSinBase);
                vanResult.Resultado = num2 + montoInicial;
            }
            catch
            {
                vanResult.ConError = true;
            }

            return vanResult;
        }

        public IndicadorEconomico CalculateRelacionBC(double tasaCostoCapital)
        {
            IndicadorEconomico relacionBCResult = new IndicadorEconomico();
            tasaCostoCapital = tasaCostoCapital / 100;

            try
            {
                //beneficio
                double num1 = Financial.NPV(tasaCostoCapital, ref ventasSinBase);
                double tempVan1 = num1 + ventaInicial;

                //costo
                double num2 = Financial.NPV(tasaCostoCapital, ref costosSinBase);
                double tempVan2 = num2 + costoInicial;

                relacionBCResult.Resultado = (tempVan1 / tempVan2).PonderarNumero(true);
            }
            catch
            {
                relacionBCResult.ConError = true;
            }

            return relacionBCResult;
        }

        public IndicadorEconomico CalculateRelacionBCInvInicial(double tasaCostoCapital)
        {
            IndicadorEconomico relacionBCResultInvInicial = new IndicadorEconomico();
            tasaCostoCapital = tasaCostoCapital / 100;

            try
            {
                //beneficio
                double tempVan1 = Financial.NPV(tasaCostoCapital, ref ventas);

                //costo
                double num2 = Financial.NPV(tasaCostoCapital, ref costos);
                double tempVan2 = num2 + inversionInicial;

                relacionBCResultInvInicial.Resultado = (tempVan1 / tempVan2).PonderarNumero(true);
            }
            catch
            {
                relacionBCResultInvInicial.ConError = true;
            }

            return relacionBCResultInvInicial;
        }
        #endregion

        #region Properties
        public IndicadorEconomico VAN
        {
            get { return van; }
        }

        public IndicadorEconomico TIR
        {
            get { return tir; }
        }

        public IndicadorEconomico PRI
        {
            get { return pri; }
        }

        public IndicadorEconomico RelacionBC
        {
            get { return relacionBC; }
        }

        public IndicadorEconomico RelacionBCInversionInicial
        {
            get { return relacionBCInvInicial; }
        }

        #endregion

        #region PrivateMethods
        private void CalculateTIR(double[] flujoCaja)
        {
            tir = new IndicadorEconomico(esPorcentaje: true);
            try
            {
                tir.Resultado = Financial.IRR(ref flujoCaja) * 100;
            }
            catch
            {
                tir.ConError = true;
            }
        }

        private void CalculatePRI(double[] flujoCaja)
        {
            pri = new IndicadorEconomico();
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
                pri.Resultado = priTemp.PonderarNumero(true);                  
            }
            catch
            {
                pri.ConError = true;
            }
        }       
        #endregion

    }
}
