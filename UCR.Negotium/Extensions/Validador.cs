using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Extensions
{
    public static class Validador
    {
        public static bool ValideEntreRangoDeNumero(int menor, int mayor, int valor)
        {
            bool resultado = false;
            if (valor < menor || valor > mayor)
            {
                resultado = true;
            }

            return resultado;
        }
    }
}
