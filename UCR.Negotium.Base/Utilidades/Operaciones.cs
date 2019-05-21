
using System.Collections.Generic;

namespace UCR.Negotium.Base.Utilidades
{
    public static class Operaciones
    {
        public static List<double> SumarListas(List<double> listaInicial, List<double> listaASumar)
        {
            List<double> listasSumadas = new List<double>();
            if (listaInicial.Count > 0)
            {
                if (listaInicial.Count.Equals(listaASumar.Count))
                {
                    for (int i = 0; i < listaInicial.Count; i++)
                    {
                        listasSumadas.Add(listaInicial[i] + listaASumar[i]);
                    }
                }
                else
                {
                    listasSumadas = listaInicial;
                }
            }
            else
            {
                listasSumadas = listaASumar;
            }

            return listasSumadas;
        }

        public static List<double> ObtenerListaPorDefecto(int tamanoLista)
        {
            List<double> listaPorDefecto = new List<double>();
            for (int i = 0; i < tamanoLista; i++)
            {
                listaPorDefecto.Add(0);
            }

            return listaPorDefecto;
        }
    }
}
