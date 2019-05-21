using System;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Base.Trace;

namespace UCR.Negotium.DataAccess
{
    public class OrganizacionProponenteData:BaseData
    {
        public OrganizacionProponente InsertarOrganizacionProponente(OrganizacionProponente orgProponente, int codProyecto)
        {
            object newProdID, newProdID2;

            string insert1 = "INSERT INTO ORGANIZACION_PROPONENTE(nombre_organizacion, cedula_juridica, "+
                "telefono, descripcion, cod_tipo, email, cod_proyecto) VALUES(?,?,?,?,?,?,?);"
                + "SELECT last_insert_rowid();";

            string insert2 = "INSERT INTO PROPONENTE(nombre, apellidos, num_identificacion, telefono, " +
                "email, puesto_en_organizacion, genero, cod_organizacion, representante_individual) " +
                "VALUES(?,?,?,?,?,?,?,?,?); SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert1, conn);
                    SQLiteCommand command2 = null;

                    cmd.Parameters.AddWithValue("nombre_organizacion", orgProponente.NombreOrganizacion);
                    cmd.Parameters.AddWithValue("cedula_juridica", orgProponente.CedulaJuridica);
                    cmd.Parameters.AddWithValue("telefono", orgProponente.Telefono);
                    cmd.Parameters.AddWithValue("descripcion", orgProponente.Descripcion);
                    cmd.Parameters.AddWithValue("cod_tipo", orgProponente.Tipo.CodTipo);
                    cmd.Parameters.AddWithValue("email", orgProponente.CorreoElectronico);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    transaction = conn.BeginTransaction();

                    newProdID = cmd.ExecuteScalar();
                    orgProponente.CodOrganizacion = int.Parse(newProdID.ToString());

                    command2 = new SQLiteCommand(insert2, conn);
                    command2.Parameters.AddWithValue("nombre", orgProponente.Proponente.Nombre);
                    command2.Parameters.AddWithValue("apellidos", orgProponente.Proponente.Apellidos);
                    command2.Parameters.AddWithValue("num_identificacion", orgProponente.Proponente.NumIdentificacion);
                    command2.Parameters.AddWithValue("telefono", orgProponente.Proponente.Telefono);
                    command2.Parameters.AddWithValue("email", orgProponente.Proponente.Email);
                    command2.Parameters.AddWithValue("puesto_en_organizacion", orgProponente.Proponente.PuestoEnOrganizacion);
                    command2.Parameters.AddWithValue("genero", orgProponente.Proponente.Genero ? 'm' : 'f');
                    command2.Parameters.AddWithValue("cod_organizacion", orgProponente.CodOrganizacion);
                    command2.Parameters.AddWithValue("representante_individual", orgProponente.Proponente.EsRepresentanteIndividual);

                    newProdID2 = command2.ExecuteScalar();
                    orgProponente.Proponente.IdProponente = int.Parse(newProdID2.ToString());
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    ex.TraceExceptionAsync();
                    orgProponente = new OrganizacionProponente();
                }
            }

            return orgProponente;
        }

        public bool EditarOrganizacionProponente(OrganizacionProponente organizacion)
        {
            int result = -1;

            string update1 = "UPDATE ORGANIZACION_PROPONENTE SET nombre_organizacion=?, " +
                "cedula_juridica=?, telefono=?, descripcion=?, cod_tipo=?, email=? "+
                "WHERE cod_organizacion=?";

            string update2 = "UPDATE PROPONENTE SET nombre=?, apellidos=?, telefono=?, " +
                "email=?, puesto_en_organizacion=?, genero=?, representante_individual=?, " +
                "num_identificacion=? WHERE cod_proponente=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand command1 = new SQLiteCommand(update1, conn);
                    SQLiteCommand command2 = null;

                    command1.Parameters.AddWithValue("nombre_organizacion", organizacion.NombreOrganizacion);
                    command1.Parameters.AddWithValue("cedula_juridica", organizacion.CedulaJuridica);
                    command1.Parameters.AddWithValue("telefono", organizacion.Telefono);
                    command1.Parameters.AddWithValue("descripcion", organizacion.Descripcion);
                    command1.Parameters.AddWithValue("cod_tipo", organizacion.Tipo.CodTipo);
                    command1.Parameters.AddWithValue("email", organizacion.CorreoElectronico);
                    command1.Parameters.AddWithValue("cod_organizacion", organizacion.CodOrganizacion);

                    transaction = conn.BeginTransaction();

                    result = command1.ExecuteNonQuery();
                    if(result != -1)
                    {
                        command2 = new SQLiteCommand(update2, conn);
                        command2.Parameters.AddWithValue("nombre", organizacion.Proponente.Nombre);
                        command2.Parameters.AddWithValue("apellidos", organizacion.Proponente.Apellidos);
                        command2.Parameters.AddWithValue("telefono", organizacion.Proponente.Telefono);
                        command2.Parameters.AddWithValue("email", organizacion.Proponente.Email);
                        command2.Parameters.AddWithValue("puesto_en_organizacion", organizacion.Proponente.PuestoEnOrganizacion);
                        command2.Parameters.AddWithValue("genero", organizacion.Proponente.Genero ? 'm' : 'f');
                        command2.Parameters.AddWithValue("representante_individual", organizacion.Proponente.EsRepresentanteIndividual);
                        command2.Parameters.AddWithValue("num_identificacion", organizacion.Proponente.NumIdentificacion);
                        command2.Parameters.AddWithValue("cod_proponente", organizacion.Proponente.IdProponente);

                        result = command2.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
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
