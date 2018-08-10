using UCR.Negotium.Domain.Extensions;

namespace UCR.Negotium.Domain
{
    public class IndicadorEconomico
    {
        private bool esPorcentaje;
        private string signoMoneda;

        public bool ConError { get; set; }
        public bool EsEvaluablePorCantidad { get; set; }
        public double Resultado { get; set; }

        public override string ToString()
        {
            if (!ConError && !double.IsNaN(Resultado))
            {
                if (esPorcentaje)
                    return Resultado.FormatoPorcentaje();
                else if (!string.IsNullOrEmpty(signoMoneda))
                    return Resultado.FormatoMoneda(signoMoneda);
                return Resultado.ToString();
            }

            return "Indefinido";
        }

        public IndicadorEconomico(bool esPorcentaje = false, string signoMoneda = "")
        {
            this.esPorcentaje = esPorcentaje;
            this.signoMoneda = signoMoneda;
        }

        public string EvaluarPorCantidad(int cantidad)
        {
            if (EsEvaluablePorCantidad)
            {
                if (!ConError && !double.IsNaN(Resultado))
                {
                    try
                    {
                        double resultado = (Resultado / cantidad).PonderarNumero();
                        if (!double.IsNaN(resultado) && !double.IsInfinity(resultado))
                        {
                            if (this.esPorcentaje)
                            {
                                return resultado.FormatoPorcentaje();
                            }
                            else if (!string.IsNullOrEmpty(signoMoneda))
                            {
                                return resultado.FormatoMoneda(signoMoneda);
                            }
                            else
                            {
                                return resultado.ToString();
                            }
                        }
                    }
                    catch { return "Indefinido"; }
                }

                return "Indefinido";
            }

            return "";
        }
    }
}
