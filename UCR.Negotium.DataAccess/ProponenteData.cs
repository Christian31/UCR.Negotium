using System;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class ProponenteData:BaseData
    {
        public ProponenteData() { }

        public int InsertarProponente(Proponente proponente, int codOrganizacion)
        {
            int idProponente = -1;
            object newProdID;
            string insert = "INSERT INTO PROPONENTE(nombre, apellidos, num_identificacion, telefono, " +
                "email, puesto_en_organizacion, genero, cod_organizacion, representante_individual) " +
                "VALUES(?,?,?,?,?,?,?,?,?); SELECT last_insert_rowid();";

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
                    command.Parameters.AddWithValue("cod_organizacion", codOrganizacion);
                    command.Parameters.AddWithValue("representante_individual", proponente.EsRepresentanteIndividual);

                    newProdID = command.ExecuteScalar();
                    idProponente = int.Parse(newProdID.ToString());
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    idProponente = -1;
                }
            }

            return idProponente;
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
                    command.Parameters.AddWithValue("puesto_en_organizacion", proponente.PuestoEnOrganizacion);
                    command.Parameters.AddWithValue("genero", proponente.Genero ? 'm' : 'f');
                    command.Parameters.AddWithValue("representante_individual", proponente.EsRepresentanteIndividual);
                    command.Parameters.AddWithValue("num_identificacion", proponente.NumIdentificacion);
                    command.Parameters.AddWithValue("cod_proponente", proponente.IdProponente);

                    result = command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }
    }
}
