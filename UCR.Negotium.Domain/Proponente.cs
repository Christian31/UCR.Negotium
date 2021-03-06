﻿namespace UCR.Negotium.Domain
{
    public class Proponente
    {
        public int IdProponente { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NumIdentificacion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string PuestoEnOrganizacion { get; set; }
        public bool Genero { get; set; } //true=masculino false=femenino
        public bool EsRepresentanteIndividual { get; set; }

        public Proponente() { }

        public override string ToString()
        {
            return Nombre + " " + Apellidos;
        }
    }
}
