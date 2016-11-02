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
    public class ProvinciaData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public ProvinciaData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
            conexion.Open();
        }

        //El siguiente metodo nos retorna todas las provincias de Costa Rica 
        //existentes en la base de datos
        public DataTable GetProvincias()
        {
            String select = "SELECT * FROM PROVINCIA";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataAdapter daProvincias = new SQLiteDataAdapter();
            daProvincias.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsProvincia = new DataSet();
            daProvincias.Fill(dsProvincia, "Provincia");
            DataTable dtProvincias = dsProvincia.Tables["Provincia"];
            conexion.Close();
            return dtProvincias;
        }//GetProvincias
    }//ProvinciaData
}
