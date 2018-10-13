using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Base.Trace;

namespace UCR.Negotium.DataAccess
{
    public class VariacionAnualCostoData:BaseData
    {
        public VariacionAnualCostoData() { }

        public List<VariacionAnualCosto> GetVariacionAnualCostos(int codProyecto)
        {
            List<VariacionAnualCosto> variacionAnualCostos = new List<VariacionAnualCosto>();
            string select = "SELECT cod_variacion_anual, ano, porcentaje " +
                "FROM VARIACION_ANUAL_COSTO WHERE cod_proyecto=? ORDER BY ano";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VariacionAnualCosto detallecosto = new VariacionAnualCosto();
                            detallecosto.CodVariacionCosto = reader.GetInt32(0);
                            detallecosto.Ano = reader.GetInt32(1);
                            detallecosto.PorcentajeIncremento = reader.GetDouble(2);
                            variacionAnualCostos.Add(detallecosto);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    variacionAnualCostos = new List<VariacionAnualCosto>();
                }
            }

            return variacionAnualCostos;
        }

        public VariacionAnualCosto InsertarVariacionAnualCosto(VariacionAnualCosto variacion, int codProyecto)
        {
            object newProdID;
            string insert = "INSERT INTO VARIACION_ANUAL_COSTO(cod_proyecto, ano, porcentaje) " +
                "VALUES(?,?,?); SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    cmd.Parameters.AddWithValue("ano", variacion.Ano);
                    cmd.Parameters.AddWithValue("porcentaje", variacion.PorcentajeIncremento);

                    newProdID = cmd.ExecuteScalar();
                    variacion.CodVariacionCosto = int.Parse(newProdID.ToString());
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    variacion = new VariacionAnualCosto();
                }
            }

            return variacion;
        }

        public bool EditarVariacionAnualCosto(VariacionAnualCosto variacion)
        {
            int result = -1;
            string update = "UPDATE VARIACION_ANUAL_COSTO SET ano=?, porcentaje=? " +
                "WHERE cod_variacion_anual=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(update, conn);
                    cmd.Parameters.AddWithValue("ano", variacion.Ano);
                    cmd.Parameters.AddWithValue("porcentaje", variacion.PorcentajeIncremento);
                    cmd.Parameters.AddWithValue("cod_variacion_anual", variacion.CodVariacionCosto);

                    result = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        public bool EliminarVariacionAnualCostos(int codProyecto)
        {
            int result = -1;
            string select = "DELETE FROM VARIACION_ANUAL_COSTO WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    result = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }
    }
}
