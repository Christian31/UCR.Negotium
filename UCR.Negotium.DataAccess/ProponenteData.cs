//@Copyright Yordan Campos Piedra
using System;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class ProponenteData 
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public ProponenteData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public int InsertarProponente(Proponente proponente, int codProyecto)
        {
            int idProponente = -1;
            object newProdID;
            string insert = "INSERT INTO PROPONENTE(nombre, apellidos, num_identificacion, telefono, " +
                "email, puesto_en_organizacion, genero, cod_organizacion, cod_proyecto, representante_individual) " +
                "VALUES(?,?,?,?,?,?,?,?,?,?); SELECT last_insert_rowid();";

            // Ejecutamos la sentencia INSERT y cerramos la conexión
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = insert;
                command.Parameters.AddWithValue("nombre", proponente.Nombre);
                command.Parameters.AddWithValue("apellidos", proponente.Apellidos);
                command.Parameters.AddWithValue("num_identificacion", proponente.NumIdentificacion);
                command.Parameters.AddWithValue("telefono", proponente.Telefono);
                command.Parameters.AddWithValue("email", proponente.Email);
                command.Parameters.AddWithValue("puesto_en_organizacion", proponente.PuestoEnOrganizacion);
                command.Parameters.AddWithValue("genero", proponente.Genero ? 'm': 'f');
                command.Parameters.AddWithValue("cod_organizacion", proponente.Organizacion.CodOrganizacion);
                command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                command.Parameters.AddWithValue("representante_individual", proponente.EsRepresentanteIndividual);

                newProdID = command.ExecuteScalar();
                idProponente = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return idProponente;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return idProponente;
            }
        }//InsertarProponente

        public Proponente GetProponente(int codProyecto)
        {
            string select = "SELECT p.nombre, p.apellidos, p.num_identificacion, "+
                "p.telefono, p.email, p.puesto_en_organizacion, p.genero, "+
                "p.cod_proponente, o.nombre_organizacion, o.cedula_juridica, "+
                "o.telefono, o.descripcion, o.cod_tipo, o.cod_organizacion, p.representante_individual, o.email " +
                "FROM PROPONENTE p, ORGANIZACION_PROPONENTE o WHERE p.cod_organizacion=o.cod_organizacion and "+
                "p.cod_proyecto=" + codProyecto;

            Proponente proponente = new Proponente();

            try
            {

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    proponente.Nombre = reader.GetString(0);
                    proponente.Apellidos = reader.GetString(1);
                    proponente.NumIdentificacion = reader.GetString(2);
                    proponente.Telefono = reader.GetString(3);
                    proponente.Email = reader.GetString(4);
                    proponente.PuestoEnOrganizacion = reader.GetString(5);
                    proponente.Genero = reader.GetChar(6).Equals('m')? true: false;
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
                conexion.Close();
                return proponente;

            }
            catch
            {
                conexion.Close();
                return proponente;
            }
        }//GetObjetoInteres

        public bool ActualizarProponente(Proponente proponente)
        {
            string query = "UPDATE PROPONENTE SET nombre=?, apellidos=?, " +
                "telefono=?, email=?, puesto_en_organizacion=?, " +
                "genero=?, representante_individual=? " +
                "WHERE num_identificacion='" + proponente.NumIdentificacion + "';";

            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                command = conexion.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("nombre", proponente.Nombre);
                command.Parameters.AddWithValue("apellidos", proponente.Apellidos);
                command.Parameters.AddWithValue("telefono", proponente.Telefono);
                command.Parameters.AddWithValue("email", proponente.Email);
                command.Parameters.AddWithValue("genero", proponente.Genero?'m':'f');
                command.Parameters.AddWithValue("puesto_en_organizacion", proponente.PuestoEnOrganizacion);
                command.Parameters.AddWithValue("representante_individual", proponente.EsRepresentanteIndividual);
                // Ejecutamos la sentencia INSERT y cerramos la conexión
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
            }
            catch
            {
                conexion.Close();
                return false;
            }
            
        }//ActualizarProponente
    }//ProponenteData
}
