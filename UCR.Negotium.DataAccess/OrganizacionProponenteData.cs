using System;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class OrganizacionProponenteData:BaseData
    {
        public OrganizacionProponenteData() { }

        public int InsertarOrganizacionProponente(OrganizacionProponente orgProponente, int codProyecto)
        {
            int codOrganizacion = -1;
            object codOrganizacionObject;
            string insert = "INSERT INTO ORGANIZACION_PROPONENTE(nombre_organizacion, cedula_juridica, "+
                "telefono, descripcion, cod_tipo, email, cod_proyecto) VALUES(?,?,?,?,?,?,?);"
                + "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("nombre_organizacion", orgProponente.NombreOrganizacion);
                    cmd.Parameters.AddWithValue("cedula_juridica", orgProponente.CedulaJuridica);
                    cmd.Parameters.AddWithValue("telefono", orgProponente.Telefono);
                    cmd.Parameters.AddWithValue("descripcion", orgProponente.Descripcion);
                    cmd.Parameters.AddWithValue("cod_tipo", orgProponente.Tipo.CodTipo);
                    cmd.Parameters.AddWithValue("email", orgProponente.CorreoElectronico);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    codOrganizacionObject = cmd.ExecuteScalar();
                    codOrganizacion = int.Parse(codOrganizacionObject.ToString());
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
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
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        public OrganizacionProponente GetOrganizacionProponente(int codProyecto)
        {
            OrganizacionProponente orgProponente = new OrganizacionProponente();
            string select = "SELECT p.nombre, p.apellidos, p.num_identificacion, " +
                "p.telefono, p.email, p.puesto_en_organizacion, p.genero, " +
                "p.cod_proponente, o.nombre_organizacion, o.cedula_juridica, " +
                "o.telefono, o.descripcion, o.cod_tipo, o.cod_organizacion, p.representante_individual, o.email " +
                "FROM PROPONENTE p, ORGANIZACION_PROPONENTE o WHERE o.cod_proyecto=?" +
                "AND o.cod_organizacion=p.cod_organizacion ";

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
                            orgProponente.Proponente.Nombre = reader.GetString(0);
                            orgProponente.Proponente.Apellidos = reader.GetString(1);
                            orgProponente.Proponente.NumIdentificacion = reader.GetString(2);
                            orgProponente.Proponente.Telefono = reader.GetString(3);
                            orgProponente.Proponente.Email = reader.GetString(4);
                            orgProponente.Proponente.PuestoEnOrganizacion = reader.GetString(5);
                            orgProponente.Proponente.Genero = reader.GetChar(6).Equals('m') ? true : false;
                            orgProponente.Proponente.IdProponente = reader.GetInt32(7);
                            orgProponente.NombreOrganizacion = reader.GetString(8);
                            orgProponente.CedulaJuridica = reader.GetString(9);
                            orgProponente.Telefono = reader.GetString(10);
                            orgProponente.Descripcion = reader.GetString(11);
                            orgProponente.Tipo.CodTipo = reader.GetInt32(12);
                            orgProponente.CodOrganizacion = reader.GetInt32(13);
                            orgProponente.Proponente.EsRepresentanteIndividual = reader.GetBoolean(14);
                            orgProponente.CorreoElectronico = reader.GetString(15);
                        }//if
                    }
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    orgProponente = new OrganizacionProponente();
                }
            }

            return orgProponente;
        }
    }
}
