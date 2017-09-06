using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class EncargadoData: BaseData
    {
        public EncargadoData() { }

        public int InsertarEncargado(Encargado encargado)
        {
            int idEncargado = -1;
            object newProdID;
            string insert = "INSERT INTO ENCARGADO(nombre, apellidos, telefono, email, organizacion) " +
                "VALUES(?,?,?,?,?); SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("nombre", encargado.Nombre);
                    cmd.Parameters.AddWithValue("apellidos", encargado.Apellidos);
                    cmd.Parameters.AddWithValue("telefono", encargado.Telefono);
                    cmd.Parameters.AddWithValue("email", encargado.Email);
                    cmd.Parameters.AddWithValue("organizacion", encargado.Organizacion);

                    newProdID = cmd.ExecuteScalar();
                    idEncargado = int.Parse(newProdID.ToString());
                }
                catch
                {
                    idEncargado = -1;
                }
            }

            return idEncargado;
        }

        public bool EditarEncargado(Encargado encargado)
        {
            int result = -1;
            string update = "UPDATE ENCARGADO SET nombre=?, apellidos=?, telefono=?, email=?, organizacion=? "
                        + "WHERE cod_encargado=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(update, conn);
                    cmd.Parameters.AddWithValue("nombre", encargado.Nombre);
                    cmd.Parameters.AddWithValue("apellidos", encargado.Apellidos);
                    cmd.Parameters.AddWithValue("telefono", encargado.Telefono);
                    cmd.Parameters.AddWithValue("email", encargado.Email);
                    cmd.Parameters.AddWithValue("organizacion", encargado.Organizacion);
                    cmd.Parameters.AddWithValue("cod_encargado", encargado.IdEncargado);

                    result = cmd.ExecuteNonQuery();
                }
                catch
                {
                    result = -1;
                }
            }

            return result != -1;
        }

        public Encargado GetEncargado(int codEncargado)
        {
            Encargado encargado = new Encargado();
            string sqlQuery = "SELECT * FROM ENCARGADO WHERE cod_encargado=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("cod_encargado", codEncargado);

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
                catch
                {
                    encargado = new Encargado();
                }
            }

            return encargado;
        }

        public bool EliminarEncargado(int codEncargado)
        {
            int result = -1;
            string delete = "DELETE FROM Encargado WHERE cod_encargado=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(delete, conn);
                    cmd.Parameters.AddWithValue("cod_encargado", codEncargado);

                    result = cmd.ExecuteNonQuery();
                }
                catch
                {
                    result = -1;
                }
            }

            return result != -1;
        }
    }
}
