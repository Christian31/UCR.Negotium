//@Copyright Yordan Campos Piedra
using System;

namespace UCR.Negotium.Domain
{
    public class UnidadMedida
    {
        public int CodUnidad { get; set; }
        public string NombreUnidad { get; set; }
        
        public override string ToString()
        {
            return NombreUnidad;
        }
    }
}