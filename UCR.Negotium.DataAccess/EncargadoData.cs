using System;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class EncargadoData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;
        
        public EncargadoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public int InsertarEncargado(Encargado encargado)
        {
            int idEncargado = -1;
            object newProdID;
            string insert = "INSERT INTO ENCARGADO(nombre, apellidos, telefono, email, organizacion) " +
                "VALUES(?,?,?,?,?); SELECT last_insert_rowid();";
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

        public bool EditarEncargado(Encargado encargado)
        {
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE ENCARGADO "
                        + "SET nombre =?, apellidos =?, telefono =?, email =?, organizacion =?"
                        + "WHERE cod_encargado =?";
                    cmd.Prepare();
                    command.Parameters.AddWithValue("nombre", encargado.Nombre);
                    command.Parameters.AddWithValue("apellidos", encargado.Apellidos);
                    command.Parameters.AddWithValue("telefono", encargado.Telefono);
                    command.Parameters.AddWithValue("email", encargado.Email);
                    command.Parameters.AddWithValue("organizacion", encargado.Organizacion);
                    command.Parameters.AddWithValue("cod_encargado", encargado.IdEncargado);

                    try
                    {
                        if (cmd.ExecuteNonQuery() != -1)
                        {
                            return true;
                        } 
                    }
                    catch (SQLiteException)
                    {
                        return false;
                    }
                }
                conn.Close();
            }
            return false;
        }

        public Encargado GetEncargado(int codEncargado)
        {
            Encargado encargado = new Encargado();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
                {
                    conn.Open();
                    string sqlQuery = "SELECT * FROM ENCARGADO WHERE cod_encargado =" + codEncargado;

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlQuery, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                encargado.Nombre = reader.GetString(0);
                                encargado.Apellidos = reader.GetString(1);
                                encargado.Telefono = reader.GetString(2);
                                encargado.Email = reader.GetString(3);
                                encargado.IdEncargado = reader.GetInt32(4);
                                encargado.Organizacion = reader.GetString(5);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch
            {
                return null;
            }

            return encargado;
        }

        public bool EliminarEncargado(int codEncargado)
        {
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM Encargado WHERE cod_encargado =?";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("cod_encargado", codEncargado);
                    try
                    {
                        if(cmd.ExecuteNonQuery() != -1)
                        {
                            return true;
                        }
                    }
                    catch (SQLiteException e)
                    {
                        return false;
                    }
                }
                conn.Close();
            }
            return false;
        }
    }
}
