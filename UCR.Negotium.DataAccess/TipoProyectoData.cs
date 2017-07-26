using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class TipoProyectoData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public TipoProyectoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public List<TipoProyecto> GetTipoProyectos()
        {
            List<TipoProyecto> tipoProyectos = new List<TipoProyecto>();
            string select = "SELECT * FROM TIPO_PROYECTO";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                TipoProyecto tipoProyecto = new TipoProyecto();
                tipoProyecto.CodTipo = reader.GetInt32(0);
                tipoProyecto.Nombre = reader.GetString(1);
                tipoProyectos.Add(tipoProyecto);
            }
            conexion.Close();

            return tipoProyectos;
        }
    }
}
