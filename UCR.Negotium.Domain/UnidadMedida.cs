//@Copyright Yordan Campos Piedra
using System;

namespace UCR.Negotium.Domain
{
    public class UnidadMedida
    {
        int codUnidad;
        String nombreUnidad;

        public int CodUnidad
        {
            get
            {
                return codUnidad;
            }

            set
            {
                codUnidad = value;
            }
        }

        public string NombreUnidad
        {
            get
            {
                return nombreUnidad;
            }

            set
            {
                nombreUnidad = value;
            }
        }
    }
}