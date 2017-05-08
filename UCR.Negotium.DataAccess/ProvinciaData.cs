//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class ProvinciaData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public ProvinciaData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public List<Provincia> GetProvincias()
        {
            CantonData cantonData = new CantonData();
            List<Provincia> provincias = new List<Provincia>();
            string select = "SELECT * FROM PROVINCIA";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Provincia provincia = new Provincia();
                provincia.CodProvincia = reader.GetInt32(0);
                provincia.NombreProvincia = reader.GetString(1);
                provincia.Cantones = cantonData.GetCantonesPorProvinciaAux(provincia.CodProvincia);
                provincias.Add(provincia);
            }
            conexion.Close();

            return provincias;
        }
    }//ProvinciaData
}
