//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class ProponenteData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public ProponenteData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public bool InsertarProponente(Proponente proponente, int codProyecto)
        {
            String insert = "INSERT INTO PROPONENTE(nombre, apellidos, num_identificacion, telefono, "+
                "email, puesto_en_organizacion, genero, cod_organizacion, cod_proyecto) "+
                "VALUES(?,?,?,?,?,?,?,?,?)";
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
            command.Parameters.AddWithValue("genero", proponente.Genero);
            command.Parameters.AddWithValue("cod_organizacion", proponente.Organizacion.CodOrganizacion);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
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
        }//InsertarProponente

        public Proponente GetProponente(int codProyecto)
        {
            String select = "SELECT p.nombre, p.apellidos, p.num_identificacion, "+
                "p.telefono, p.email, p.puesto_en_organizacion, p.genero, p.cod_organizacion, "+
                "p.cod_proyecto, p.id_proponente, o.nombre_organizacion, o.cedula_juridica, "+
                "o.telefono, o.descripcion, o.cod_tipo, o.cod_organizacion "+
                "FROM PROPONENTE p, ORGANIZACION_PROPONENTE o WHERE p.cod_organizacion=o.cod_organizacion and "+
                "p.cod_proyecto=" + codProyecto;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            Proponente proponente = null;
            if (reader.Read())
            {
                proponente = new Proponente();
                proponente.Apellidos = reader.GetString(1);
                proponente.Email = reader.GetString(4);
                proponente.Genero = reader.GetChar(6);
                proponente.IdProponente = reader.GetInt32(9);
                proponente.Nombre = reader.GetString(0);
                proponente.NumIdentificacion = reader.GetString(2);
                proponente.Organizacion.CedulaJuridica = reader.GetString(11);
                proponente.Organizacion.CodOrganizacion = reader.GetInt32(15);
                proponente.Organizacion.Descripcion = reader.GetString(13);
                proponente.Organizacion.NombreOrganizacion = reader.GetString(10);
                proponente.Organizacion.Telefono = reader.GetString(12);
                proponente.Organizacion.Tipo.CodTipo = reader.GetInt32(14);
                proponente.PuestoEnOrganizacion = reader.GetString(5);
                proponente.Telefono = reader.GetString(4);
            }//if
            conexion.Close();
            return proponente;
        }//GetObjetoInteres

        public bool ActualizarProponente(Proponente proponente)
        {
            String update = "UPDATE PROPONENTE SET nombre=?, apellidos=?, " +
                "telefono=?, email=?, puesto_en_organizacion=?, " +
                "genero=? WHERE num_identificacion=" + proponente.NumIdentificacion;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = update;
            command.Parameters.AddWithValue("nombre", proponente.Nombre);
            command.Parameters.AddWithValue("apellidos", proponente.Apellidos);
            command.Parameters.AddWithValue("telefono", proponente.Telefono);
            command.Parameters.AddWithValue("email", proponente.Email);
            command.Parameters.AddWithValue("genero", proponente.Genero);
            command.Parameters.AddWithValue("puesto_en_organizacion", proponente.PuestoEnOrganizacion);
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
        }//ActualizarProponente
    }//ProponenteData
}
