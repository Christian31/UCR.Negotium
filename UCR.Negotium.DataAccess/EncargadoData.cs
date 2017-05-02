using System;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class EncargadoData :BaseData
    {
        private SQLiteCommand command;
        
        public EncargadoData() { }

        public int InsertarEncargado(Encargado encargado)
        {
            int idEncargado = -1;
            object newProdID;
            string insert = "INSERT INTO ENCARGADO(nombre, apellidos, telefono, email, organizacion) " +
                "VALUES(?,?,?,?,?)";
            try
            {
                command = conexion.CreateCommand();
                command.CommandText = insert;
                command.Parameters.AddWithValue("nombre", encargado.Nombre);
                command.Parameters.AddWithValue("apellidos", encargado.Apellidos);
                command.Parameters.AddWithValue("telefono", encargado.Telefono);
                command.Parameters.AddWithValue("email", encargado.Email);
                command.Parameters.AddWithValue("organizacion", encargado.Organizacion);

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                idEncargado = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return idEncargado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return idEncargado;
            }
        }

        //TODDO HERE
        public bool EditarEncargado(Encargado encargado)
        {
            return true;
        }

        public Encargado GetEncargado(int codEncargado)
        {
            string select = "SELECT * FROM ENCARGADO WHERE cod_encargado=" + codEncargado;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            Encargado encargado = new Encargado();
            if (reader.Read())
            {
                encargado.Nombre = reader.GetString(0);
                encargado.Apellidos = reader.GetString(1);
                encargado.Telefono = reader.GetString(2);
                encargado.Email = reader.GetString(3);
                encargado.IdEncargado = reader.GetInt32(6);
            }
            conexion.Close();
            return encargado;
        }
    }
}
