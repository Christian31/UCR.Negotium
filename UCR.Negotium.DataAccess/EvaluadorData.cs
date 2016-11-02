//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class EvaluadorData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;
        
        public EvaluadorData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        //El siguiente metodo extrae un evaluador de la base de datos
        //Se realiza de la siguiente forma para cumplir con varios obj
        //por ejemplo en el Login para poder saber si un evaluador existe o no
        //además nos va a servir para extraer los datos de un evaluador en especifico
        //cuando se requiera.
        public Evaluador GetEvaluador(String correo, String password)
        {
            String select = "SELECT nombre,apellidos,telefono,email,num_identificacion," +
                "id_evaluador, organizacion FROM Evaluador WHERE email='"
                + correo + "' and password='" + password + "'";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            Evaluador evaluador = null;
            if (reader.Read())
            {
                evaluador = new Evaluador();
                evaluador.Nombre = reader.GetString(0);
                evaluador.Apellidos = reader.GetString(1);
                evaluador.Telefono = reader.GetString(2);
                evaluador.Email = reader.GetString(3);
                evaluador.NumIdentificacion = reader.GetString(4);
                evaluador.IdEvaluador = reader.GetInt32(5);
                evaluador.Organizacion = reader.GetString(6);
            }
            conexion.Close();
            return evaluador;
        }//existeEvaluador

        public bool InsertarEvaluador(Evaluador evaluador)
        {
            String insert = "INSERT INTO Evaluador(nombre, apellidos,"+
                " telefono, email, num_identificacion, password, organizacion) "+
                "VALUES(?,?,?,?,?,?,?)";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("nombre", evaluador.Nombre);
            command.Parameters.AddWithValue("apellidos", evaluador.Apellidos);
            command.Parameters.AddWithValue("telefono", evaluador.Telefono);
            command.Parameters.AddWithValue("correo", evaluador.Email);
            command.Parameters.AddWithValue("num_identificacion", evaluador.NumIdentificacion);
            command.Parameters.AddWithValue("password", evaluador.Password);
            command.Parameters.AddWithValue("organizacion", evaluador.Organizacion);
            if (command.ExecuteNonQuery() != -1)
            {
                conexion.Close();
                return true;
            }//if
            else
            {
                conexion.Close();
                return false;
            }//else
        }//InsertarEvaluador

        //Este metodo es utilizado para recuperar la contraseña el cual utiliza el email para recuperar
        public Evaluador GetEvaluadorPorCorreo(String correo)
        {
            String select = "SELECT * FROM Evaluador WHERE email='"+ correo + "'";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            Evaluador evaluador = null;
            if (reader.Read())
            {
                evaluador = new Evaluador();
                evaluador.Nombre = reader.GetString(0);
                evaluador.Apellidos = reader.GetString(1);
                evaluador.Telefono = reader.GetString(2);
                evaluador.Email = reader.GetString(3);
                evaluador.NumIdentificacion = reader.GetString(4);
                evaluador.Password = reader.GetString(5);
                evaluador.IdEvaluador = reader.GetInt32(6);
            }
            conexion.Close();
            return evaluador;
        }//GetEvaluadorPorCorreo
    }//Class EvaluadorData
}
