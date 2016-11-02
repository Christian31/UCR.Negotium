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
    public class VariacionAnualCostoData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public VariacionAnualCostoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public DataTable GetVariacionAnualCostos(int codProyecto)
        {
            String select = "SELECT cod_variacion_anual, ano, porcentaje " +
                "FROM VARIACION_ANUAL_COSTO WHERE cod_proyecto=" + codProyecto + ";";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            //command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataAdapter daVariaciones = new SQLiteDataAdapter();
            daVariaciones.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsCostos = new DataSet();
            daVariaciones.Fill(dsCostos, "VariacionAnualCostos");
            DataTable dtCostos = dsCostos.Tables["VariacionAnualCostos"];
            conexion.Close();
            return dtCostos;
        }//GetVariacionAnualCostos

        public VariacionAnualCosto InsertarVariacionAnualCosto(VariacionAnualCosto variacion, int codProyecto)
        {
            Object newProdID;
            String insert = "INSERT INTO VARIACION_ANUAL_COSTO(cod_proyecto," +
                " ano, porcentaje) VALUES(?,?,?); " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            command.Parameters.AddWithValue("ano", variacion.Ano);
            command.Parameters.AddWithValue("porcentaje", variacion.ProcentajeIncremento);
            
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                variacion.CodVariacionCosto = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return variacion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return variacion;
            }//catch
        }//InsertarVariacionAnualCosto

        public VariacionAnualCosto EditarVariacionAnualCosto(VariacionAnualCosto variacion, int codProyecto)
        {
            Object newProdID;
            String insert = "UPDATE VARIACION_ANUAL_COSTO SET (ano = ?, porcentaje = ?, " +
                "cod_proyecto = ? WHERE cod_variacion_anual = ?; " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("ano", variacion.Ano);
            command.Parameters.AddWithValue("porcentaje", variacion.ProcentajeIncremento);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            command.Parameters.AddWithValue("cod_variacion_anual", variacion.CodVariacionCosto);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                variacion.CodVariacionCosto = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return variacion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return variacion;
            }//catch
        }//EditarVariacionAnualCosto

        public bool eliminarVariacionAnualCostos(int codProyecto)
        {
            String select = "DELETE FROM VARIACION_ANUAL_COSTO WHERE cod_proyecto=" + codProyecto + ";";

            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                //command = conexion.CreateCommand();
                command.CommandText = select;
                command.ExecuteNonQuery();

                conexion.Close();
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return false;
            }
        }//eliminarVariacionAnualCostos
    }
}
