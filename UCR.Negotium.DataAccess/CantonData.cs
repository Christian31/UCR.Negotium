using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class CantonData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public CantonData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public List<Canton> GetCantonesPorProvincia(int codProvincia)
        {
            DistritoData distritoData = new DistritoData();
            List<Canton> cantones = new List<Canton>();
            string select = "SELECT * FROM CANTON WHERE cod_provincia=" + codProvincia + " ORDER BY nombre_canton";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Canton canton = new Canton();
                canton.CodCanton = reader.GetInt32(0);
                canton.NombreCanton = reader.GetString(2);
                canton.Distritos = distritoData.GetDistritosPorCanton(canton.CodCanton);
                cantones.Add(canton);
            }
            conexion.Close();

            return cantones;
        }
    }
}
