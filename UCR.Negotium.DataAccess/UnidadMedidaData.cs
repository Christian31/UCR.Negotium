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
    public class UnidadMedidaData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public UnidadMedidaData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public DataTable GetUnidadesMedida()
        {
            String select = "SELECT * FROM UNIDAD_MEDIDA";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteDataAdapter daUnidadMedida = new SQLiteDataAdapter();
            daUnidadMedida.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsUndadMedida = new DataSet();
            daUnidadMedida.Fill(dsUndadMedida, "UnidadMedida");
            DataTable dtUnidadMedida = dsUndadMedida.Tables["UnidadMedida"];
            conexion.Close();
            return dtUnidadMedida;
        }

        public UnidadMedida GetUnidadMedida(int codUnidadMedida)
        {
            String select = "SELECT * FROM UNIDAD_MEDIDA WHERE cod_unidad=" + codUnidadMedida;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            UnidadMedida unidadMedida = null;
            if (reader.Read())
            {
                unidadMedida = new UnidadMedida();
                unidadMedida.CodUnidad = reader.GetInt32(0);
                unidadMedida.NombreUnidad = reader.GetString(1);
            }//if
            conexion.Close();
            return unidadMedida;
        }
    }
}
