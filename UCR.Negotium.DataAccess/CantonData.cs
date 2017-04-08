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
    public class CantonData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public CantonData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        // Extrae todos los los cantones de una provincia, este metodo es funcional para hacer el filtrado 
        // de cantones en el combobox de cantones cada vez que la provincia cambia de opción
        public DataTable GetCantonesPorProvincia(int codProvincia)
        {
            string select = "SELECT * FROM CANTON WHERE cod_provincia="+codProvincia +" ORDER BY nombre_canton";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataAdapter daCantones = new SQLiteDataAdapter();
            daCantones.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsCantones = new DataSet();
            daCantones.Fill(dsCantones, "Cantones");
            DataTable dtCantones = dsCantones.Tables["Cantones"];
            conexion.Close();
            return dtCantones;
        }

        public List<Canton> GetCantonesPorProvinciaAux(int codProvincia)
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
                canton.Distritos = distritoData.GetDistritosPorCantonAux(canton.CodCanton);
                cantones.Add(canton);
            }
            conexion.Close();

            return cantones;
        }
    }
}
