//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.DataAccess
{
    public class DistritoData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public DistritoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            conexion = new SQLiteConnection(cadenaConexion);
        }

        // Extrae todos los los distritos de un canton, este metodo es funcional para hacer el filtrado 
        // de distritos en el combobox de distritos cada vez que el canton cambia de opción
        public DataTable GetDistritosPorCanton(int codCanton)
        {
            String select = "SELECT * FROM DISTRITO WHERE cod_canton="+codCanton +" ORDER BY nombre_distrito";

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
    }
}
