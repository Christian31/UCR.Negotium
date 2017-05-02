//@Copyright Yordan Campos Piedra
using System;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class OrganizacionProponenteData : BaseData
    {

        public OrganizacionProponenteData() { }

        public int InsertarOrganizacionProponente(OrganizacionProponente organizacionProponente)
        {
            int codOrganizacion = -1;
            Object codOrganizacionObject;
            String insert = "INSERT INTO ORGANIZACION_PROPONENTE(nombre_organizacion, cedula_juridica, "+
                "telefono, descripcion, cod_tipo, email) VALUES(?,?,?,?,?,?);"
                + "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("nombre_organizacion", organizacionProponente.NombreOrganizacion);
            command.Parameters.AddWithValue("cedula_juridica", organizacionProponente.CedulaJuridica);
            command.Parameters.AddWithValue("telefono", organizacionProponente.Telefono);
            command.Parameters.AddWithValue("descripcion", organizacionProponente.Descripcion);
            command.Parameters.AddWithValue("cod_tipo", organizacionProponente.Tipo.CodTipo);
            command.Parameters.AddWithValue("email", organizacionProponente.CorreoElectronico);
            // Ejecutamos la sentencia INSERT y cerramos la conexión
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                codOrganizacionObject = command.ExecuteScalar();
                codOrganizacion = Int32.Parse(codOrganizacionObject.ToString());
                conexion.Close();
                return codOrganizacion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return codOrganizacion;
            }//catch
        }//InsertarOrganizacion

        public bool ActualizarOrganizacionProponente(OrganizacionProponente organizacion)
        {
            String update = "UPDATE ORGANIZACION_PROPONENTE SET nombre_organizacion=?, " +
                "cedula_juridica=?, telefono=?, descripcion=?, cod_tipo=?, email=? WHERE cod_organizacion=" 
                + organizacion.CodOrganizacion;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = update;
            command.Parameters.AddWithValue("nombre_organizacion", organizacion.NombreOrganizacion);
            command.Parameters.AddWithValue("cedula_juridica", organizacion.CedulaJuridica);
            command.Parameters.AddWithValue("telefono", organizacion.Telefono);
            command.Parameters.AddWithValue("descripcion", organizacion.Descripcion);
            command.Parameters.AddWithValue("cod_tipo", organizacion.Tipo.CodTipo);
            command.Parameters.AddWithValue("email", organizacion.CorreoElectronico);
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
    }//OrganizacionProponenteData
}
