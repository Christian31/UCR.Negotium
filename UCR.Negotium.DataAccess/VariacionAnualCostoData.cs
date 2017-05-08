using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class VariacionAnualCostoData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public VariacionAnualCostoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public List<VariacionAnualCosto> GetVariacionAnualCostos(int codProyecto)
        {
            List<VariacionAnualCosto> variacionAnualCostos = new List<VariacionAnualCosto>();
             
            string select = "SELECT cod_variacion_anual, ano, porcentaje " +
                "FROM VARIACION_ANUAL_COSTO WHERE cod_proyecto=" + codProyecto + ";";
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                //command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    VariacionAnualCosto detallecosto = new VariacionAnualCosto();
                    detallecosto.CodVariacionCosto = reader.GetInt32(0);
                    detallecosto.Ano = reader.GetInt32(1);
                    detallecosto.PorcentajeIncremento = reader.GetDouble(2);
                    variacionAnualCostos.Add(detallecosto);
                }//while
                conexion.Close();
                return variacionAnualCostos;
            }
            catch
            {
                return variacionAnualCostos;
            }
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
            command.Parameters.AddWithValue("porcentaje", variacion.PorcentajeIncremento);
            
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

        public bool EditarVariacionAnualCosto(VariacionAnualCosto variacion)
        {
            string insert = "UPDATE VARIACION_ANUAL_COSTO SET ano = ?, porcentaje = ? " +
                "WHERE cod_variacion_anual = ?;";
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = insert;
                command.Parameters.AddWithValue("ano", variacion.Ano);
                command.Parameters.AddWithValue("porcentaje", variacion.PorcentajeIncremento);
                command.Parameters.AddWithValue("cod_variacion_anual", variacion.CodVariacionCosto);

                if (command.ExecuteNonQuery() != -1)
                {
                    conexion.Close();
                    return true;
                }

                conexion.Close();
                return false;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return false;
            }//catch
        }//EditarVariacionAnualCosto

        public bool EliminarVariacionAnualCostos(int codProyecto)
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
