using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class TipoMonedaData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public TipoMonedaData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public List<TipoMoneda> GetTiposMonedas()
        {
            List<TipoMoneda> tiposMonedas = new List<TipoMoneda>();
            string select = "SELECT * FROM TIPO_MONEDA";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                TipoMoneda tipoMoneda = new TipoMoneda();
                tipoMoneda.CodMoneda = reader.GetInt32(0);
                tipoMoneda.NombreMoneda = reader.GetString(1);
                tipoMoneda.SignoMoneda = reader.GetString(2);
                tiposMonedas.Add(tipoMoneda);
            }
            conexion.Close();

            return tiposMonedas;
        }

        public TipoMoneda GetTipoMoneda(int codTipoMoneda)
        {
            string select = "SELECT * FROM TIPO_MONEDA WHERE cod_tipo_moneda=" + codTipoMoneda;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            TipoMoneda tipoMoneda = null;
            if (reader.Read())
            {
                tipoMoneda = new TipoMoneda();
                tipoMoneda.CodMoneda = reader.GetInt32(0);
                tipoMoneda.NombreMoneda = reader.GetString(1);
                tipoMoneda.SignoMoneda = reader.GetString(2);
            }//if
            conexion.Close();
            return tipoMoneda;
        }
    }
}
