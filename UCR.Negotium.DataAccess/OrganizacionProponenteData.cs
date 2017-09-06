using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class OrganizacionProponenteData:BaseData
    {
        public OrganizacionProponenteData() { }

        public int InsertarOrganizacionProponente(OrganizacionProponente organizacionProponente)
        {
            int codOrganizacion = -1;
            object codOrganizacionObject;
            string insert = "INSERT INTO ORGANIZACION_PROPONENTE(nombre_organizacion, cedula_juridica, "+
                "telefono, descripcion, cod_tipo, email) VALUES(?,?,?,?,?,?);"
                + "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("nombre_organizacion", organizacionProponente.NombreOrganizacion);
                    cmd.Parameters.AddWithValue("cedula_juridica", organizacionProponente.CedulaJuridica);
                    cmd.Parameters.AddWithValue("telefono", organizacionProponente.Telefono);
                    cmd.Parameters.AddWithValue("descripcion", organizacionProponente.Descripcion);
                    cmd.Parameters.AddWithValue("cod_tipo", organizacionProponente.Tipo.CodTipo);
                    cmd.Parameters.AddWithValue("email", organizacionProponente.CorreoElectronico);

                    codOrganizacionObject = cmd.ExecuteScalar();
                    codOrganizacion = int.Parse(codOrganizacionObject.ToString());
                }
                catch
                {
                    codOrganizacion = -1;
                }
            }

            return codOrganizacion;
        }

        public bool EditarOrganizacionProponente(OrganizacionProponente organizacion)
        {
            int result = -1;
            string update = "UPDATE ORGANIZACION_PROPONENTE SET nombre_organizacion=?, " +
                "cedula_juridica=?, telefono=?, descripcion=?, cod_tipo=?, email=? "+
                "WHERE cod_organizacion=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(update, conn);
                    cmd.Parameters.AddWithValue("nombre_organizacion", organizacion.NombreOrganizacion);
                    cmd.Parameters.AddWithValue("cedula_juridica", organizacion.CedulaJuridica);
                    cmd.Parameters.AddWithValue("telefono", organizacion.Telefono);
                    cmd.Parameters.AddWithValue("descripcion", organizacion.Descripcion);
                    cmd.Parameters.AddWithValue("cod_tipo", organizacion.Tipo.CodTipo);
                    cmd.Parameters.AddWithValue("email", organizacion.CorreoElectronico);
                    cmd.Parameters.AddWithValue("cod_organizacion", organizacion.CodOrganizacion);

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
