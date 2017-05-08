//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class DistritoData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public DistritoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        // Extrae todos los los distritos de un canton, este metodo es funcional para hacer el filtrado 
        // de distritos en el combobox de distritos cada vez que el canton cambia de opción
        public DataTable GetDistritosPorCanton(int codCanton)
        {
            string select = "SELECT * FROM DISTRITO WHERE cod_canton="+codCanton +" ORDER BY nombre_distrito";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataAdapter daDistritos = new SQLiteDataAdapter();
            daDistritos.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsDistritos = new DataSet();
            daDistritos.Fill(dsDistritos, "Cantones");
            DataTable dtDistritos = dsDistritos.Tables["Cantones"];
            conexion.Close();
            return dtDistritos;
        }

        public List<Distrito> GetDistritosPorCantonAux(int codCanton)
        {
            List<Distrito> cantones = new List<Distrito>();
            string select = "SELECT * FROM DISTRITO WHERE cod_canton=" + codCanton + " ORDER BY nombre_distrito";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Distrito distrito = new Distrito();
                distrito.CodDistrito = reader.GetInt32(0);
                distrito.NombreDistrito = reader.GetString(2);
                cantones.Add(distrito);
            }
            conexion.Close();

            return cantones;
        }
    }
}
