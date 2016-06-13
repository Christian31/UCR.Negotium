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
        int idEvaluador;
        String nombre, apellidos, telefono, email, numIdentificacion, password, organizacion;

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

        public string Apellidos
        {
            get
            {
                return apellidos;
            }

            set
            {
                apellidos = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public int IdEvaluador
        {
            get
            {
                return idEvaluador;
            }

            set
            {
                idEvaluador = value;
            }
        }

        public string Nombre
        {
            get
            {
                return nombre;
            }

            set
            {
                nombre = value;
            }
        }

        public string NumIdentificacion
        {
            get
            {
                return numIdentificacion;
            }

            set
            {
                numIdentificacion = value;
            }
        }

        public string Organizacion
        {
            get
            {
                return organizacion;
            }

            set
            {
                organizacion = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Telefono
        {
            get
            {
                return telefono;
            }

            set
            {
                telefono = value;
            }
        }
    }
}
