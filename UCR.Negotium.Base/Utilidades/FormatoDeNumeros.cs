using System;

namespace UCR.Negotium.Base.Utilidades
{
    public static class FormatoDeNumeros
    {
        public static double PonderarNumero(this double numero, bool aplicarReintentos = false)
        {
            double resultado = 0;
            int decimales = 2;
            if (aplicarReintentos)
            {
                while (decimales < 5)
                {
                    resultado = Math.Round(numero, decimales);
                    if (resultado != 0)
                        decimales = 5;
                    else
                        decimales++;
                }
            }
            else
            {
                resultado = Math.Round(numero, decimales);
            }

            return resultado;
        }

        public static string FormatoMoneda(this double numero, string signoMoneda)
        {
            return signoMoneda + " " + string.Format("{0:#,##0.00}", numero);
        }

        public static double FormatoNumero(this string numero, string signoMoneda)
        {
            string numeroSinMoneda = numero.Replace(signoMoneda + " ", string.Empty);
            double resultado = 0;
            if (!string.IsNullOrEmpty(numeroSinMoneda))
            {
                double.TryParse(numeroSinMoneda, out resultado);
            }
            return resultado;
        }

        public static string FormatoPorcentaje(this double numero)
        {
            return string.Format("{0:#,##0.00}", numero) + " %";
        }
    }
}
