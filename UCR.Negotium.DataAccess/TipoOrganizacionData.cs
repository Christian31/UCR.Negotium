using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class TipoOrganizacionData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public TipoOrganizacionData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public List<TipoOrganizacion> GetTipoOrganizaciones()
        {
            List<TipoOrganizacion> tipoOrganizaciones = new List<TipoOrganizacion>();
            string select = "SELECT * FROM TIPO_ORGANIZACION";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                TipoOrganizacion tipoOrganizacion = new TipoOrganizacion();
                tipoOrganizacion.CodTipo = reader.GetInt32(0);
                tipoOrganizacion.Descripcion = reader.GetString(1);
                tipoOrganizaciones.Add(tipoOrganizacion);
            }
            conexion.Close();

            return tipoOrganizaciones;
        }
    }
}
