using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class ProponenteData:BaseData
    {
        public ProponenteData() { }

        public int InsertarProponente(Proponente proponente, int codProyecto)
        {
            int idProponente = -1;
            object newProdID;
            string insert = "INSERT INTO PROPONENTE(nombre, apellidos, num_identificacion, telefono, " +
                "email, puesto_en_organizacion, genero, cod_organizacion, cod_proyecto, representante_individual) " +
                "VALUES(?,?,?,?,?,?,?,?,?,?); SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(insert, conn);
                    command.Parameters.AddWithValue("nombre", proponente.Nombre);
                    command.Parameters.AddWithValue("apellidos", proponente.Apellidos);
                    command.Parameters.AddWithValue("num_identificacion", proponente.NumIdentificacion);
                    command.Parameters.AddWithValue("telefono", proponente.Telefono);
                    command.Parameters.AddWithValue("email", proponente.Email);
                    command.Parameters.AddWithValue("puesto_en_organizacion", proponente.PuestoEnOrganizacion);
                    command.Parameters.AddWithValue("genero", proponente.Genero ? 'm' : 'f');
                    command.Parameters.AddWithValue("cod_organizacion", proponente.Organizacion.CodOrganizacion);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    command.Parameters.AddWithValue("representante_individual", proponente.EsRepresentanteIndividual);

                    newProdID = command.ExecuteScalar();
                    idProponente = int.Parse(newProdID.ToString());
                }
                catch
                {
                    idProponente = -1;
                }
            }

            return idProponente;
        }

        public Proponente GetProponente(int codProyecto)
        {
            Proponente proponente = new Proponente();
            string select = "SELECT p.nombre, p.apellidos, p.num_identificacion, "+
                "p.telefono, p.email, p.puesto_en_organizacion, p.genero, "+
                "p.cod_proponente, o.nombre_organizacion, o.cedula_juridica, "+
                "o.telefono, o.descripcion, o.cod_tipo, o.cod_organizacion, p.representante_individual, o.email " +
                "FROM PROPONENTE p, ORGANIZACION_PROPONENTE o WHERE p.cod_organizacion=o.cod_organizacion "+
                "AND p.cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            proponente.Nombre = reader.GetString(0);
                            proponente.Apellidos = reader.GetString(1);
                            proponente.NumIdentificacion = reader.GetString(2);
                            proponente.Telefono = reader.GetString(3);
                            proponente.Email = reader.GetString(4);
                            proponente.PuestoEnOrganizacion = reader.GetString(5);
                            proponente.Genero = reader.GetChar(6).Equals('m') ? true : false;
                            proponente.IdProponente = reader.GetInt32(7);
                            proponente.Organizacion.NombreOrganizacion = reader.GetString(8);
                            proponente.Organizacion.CedulaJuridica = reader.GetString(9);
                            proponente.Organizacion.Telefono = reader.GetString(10);
                            proponente.Organizacion.Descripcion = reader.GetString(11);
                            proponente.Organizacion.Tipo.CodTipo = reader.GetInt32(12);
                            proponente.Organizacion.CodOrganizacion = reader.GetInt32(13);
                            proponente.EsRepresentanteIndividual = reader.GetBoolean(14);
                            proponente.Organizacion.CorreoElectronico = reader.GetString(15);
                        }//if
                    }
                }
                catch
                {
                    proponente = new Proponente();
                }
            }

            return proponente;
        }

        public bool EditarProponente(Proponente proponente)
        {
            int result = -1;
            string update = "UPDATE PROPONENTE SET nombre=?, apellidos=?, telefono=?, " +
                "email=?, puesto_en_organizacion=?, genero=?, representante_individual=?, " +
                "num_identificacion=? WHERE cod_proponente=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update, conn);
                    command.Parameters.AddWithValue("nombre", proponente.Nombre);
                    command.Parameters.AddWithValue("apellidos", proponente.Apellidos);
                    command.Parameters.AddWithValue("telefono", proponente.Telefono);
                    command.Parameters.AddWithValue("email", proponente.Email);
                    command.Parameters.AddWithValue("genero", proponente.Genero ? 'm' : 'f');
                    command.Parameters.AddWithValue("puesto_en_organizacion", proponente.PuestoEnOrganizacion);
                    command.Parameters.AddWithValue("representante_individual", proponente.EsRepresentanteIndividual);
                    command.Parameters.AddWithValue("num_identificacion", proponente.NumIdentificacion);
                    command.Parameters.AddWithValue("cod_proponente", proponente.IdProponente);

                    result = command.ExecuteNonQuery();
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
