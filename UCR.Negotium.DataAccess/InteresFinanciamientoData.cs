using System.Data.SQLite;
using System.Collections.Generic;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class InteresFinanciamientoData:BaseData
    {
        public InteresFinanciamientoData() { }

        public bool InsertarInteresFinanciamiento(InteresFinanciamiento intFinanciamiento, int codProyecto)
        {
            int result = -1;
            string insert = "INSERT INTO INTERES_FINANCIAMIENTO(cod_proyecto, porcentaje_interes, ano_interes) " +
                "VALUES(?,?,?)";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    cmd.Parameters.AddWithValue("porcentaje_interes", intFinanciamiento.PorcentajeInteres);
                    cmd.Parameters.AddWithValue("ano_intereses", intFinanciamiento.AnoInteres);

                    result = cmd.ExecuteNonQuery();
                }
                catch
                {
                    result = -1;
                }
            }

            return result != -1;
        }

        public List<InteresFinanciamiento> GetInteresesFinanciamiento(int codProyecto)
        {
            List<InteresFinanciamiento> listaInteresesFinanciamiento = new List<InteresFinanciamiento>();
            string select = "SELECT * FROM INTERES_FINANCIAMIENTO WHERE cod_proyecto=?";

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
                            InteresFinanciamiento interes = new InteresFinanciamiento();
                            interes.CodInteresFinanciamiento = reader.GetInt32(0);
                            interes.PorcentajeInteres = reader.GetDouble(2);
                            interes.AnoInteres = reader.GetInt32(3);
                            listaInteresesFinanciamiento.Add(interes);
                        }
                    }
                }
                catch
                {
                    listaInteresesFinanciamiento = new List<InteresFinanciamiento>();
                }
            }

            return listaInteresesFinanciamiento;
        }

        public bool EditarInteresFinanciamiento(InteresFinanciamiento intFinanciamiento)
        {
            int result = -1;
            string update = "UPDATE INTERES_FINANCIAMIENTO SET porcentaje_interes=? " +
                "WHERE cod_interes_financiamiento=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(update, conn);
                    cmd.Parameters.AddWithValue("cod_interes_financiamiento", intFinanciamiento.CodInteresFinanciamiento);
                    cmd.Parameters.AddWithValue("porcentaje_interes_financiamiento", intFinanciamiento.PorcentajeInteres);

                    result = cmd.ExecuteNonQuery();
                }
                catch
                {
                    result = -1;
                }
            }

            return result != -1;
        }

        public bool EliminarInteresFinanciamiento(int codProyecto)
        {
            int result = -1;
            string delete = "DELETE FROM INTERES_FINANCIAMIENTO WHERE cod_proyecto=?";
            
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(delete, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    result = cmd.ExecuteNonQuery();
                }
                catch
                {
                    result = -1;
                }
            }

            return result != -1;
        }
    }
}
