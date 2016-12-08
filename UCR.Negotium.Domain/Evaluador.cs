//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain
{
    public class Evaluador
    {
        public int IdEvaluador { get; set; }
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String Telefono { get; set; }
        public String Email { get; set; }
        public String NumIdentificacion { get; set; }
        public String Password { get; set; }
        public String Organizacion { get; set; }

        public Evaluador()
        {

        }

        public Evaluador(int idEvaluador, String nombre, String apellidos, 
            String telefono, String email, String numIdentificacion)
        {
            this.IdEvaluador = idEvaluador;
            this.Nombre = nombre;
            this.Apellidos = apellidos;
            this.Telefono = telefono;
            this.Email = email;
            this.NumIdentificacion = numIdentificacion;
        }
    }
}
