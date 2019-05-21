using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Base.Trace;

namespace UCR.Negotium.DataAccess
{
    public class FinanciamientoData:BaseData
    {
        public FinanciamientoData() { }

        public Financiamiento InsertarFinanciamiento(Financiamiento financiamiento, int codProyecto)
        {
            object newProdID, newProdID2;

            string insert1 = "INSERT INTO FINANCIAMIENTO(cod_proyecto, monto_financiamiento, interes_fijo, " +
                "ano_final_pago, ano_inicial_pago) VALUES(?,?,?,?,?); " +
                "SELECT last_insert_rowid();";

            string insert2 = "INSERT INTO INTERES_FINANCIAMIENTO(cod_proyecto, porcentaje_interes, ano_interes) " +
                "VALUES(?,?,?); " +
                "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert1, conn);
                    SQLiteCommand cmd2 = null;

                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    cmd.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
                    cmd.Parameters.AddWithValue("interes_fijo", financiamiento.InteresFijo ? 1 : 0);
                    cmd.Parameters.AddWithValue("ano_final_pago", financiamiento.AnoFinalPago);
                    cmd.Parameters.AddWithValue("ano_inicial_pago", financiamiento.AnoInicialPago);

                    transaction = conn.BeginTransaction();

                    newProdID = cmd.ExecuteScalar();
                    financiamiento.CodFinanciamiento = int.Parse(newProdID.ToString());                    
                    foreach (InteresFinanciamiento detTemp in financiamiento.TasaIntereses)
                    {
                        cmd2 = new SQLiteCommand(insert2, conn);
                        cmd2.Parameters.AddWithValue("cod_proyecto", codProyecto);
                        cmd2.Parameters.AddWithValue("porcentaje_interes", detTemp.PorcentajeInteres);
                        cmd2.Parameters.AddWithValue("ano_intereses", detTemp.AnoInteres);
                        newProdID2 = cmd2.ExecuteScalar();
                        detTemp.CodInteresFinanciamiento = int.Parse(newProdID2.ToString());
                    }

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    ex.TraceExceptionAsync();
                    financiamiento = new Financiamiento();
                }
            }

            return financiamiento;
        }

        public Financiamiento GetFinanciamiento(int codProyecto)
        {
            Financiamiento financiamiento = new Financiamiento();
            string select = "SELECT cod_financiamiento, monto_financiamiento, interes_fijo, " +
                "ano_final_pago, ano_inicial_pago FROM FINANCIAMIENTO WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            financiamiento.CodFinanciamiento = reader.GetInt32(0);
                            financiamiento.MontoFinanciamiento = reader.GetDouble(1);
                            financiamiento.InteresFijo = reader.GetBoolean(2);
                            financiamiento.AnoFinalPago = reader.GetInt32(3);
                            financiamiento.AnoInicialPago = reader.GetInt32(4);

                            financiamiento.TasaIntereses = GetInteresesFinanciamiento(codProyecto);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    financiamiento = new Financiamiento();
                }
            }

            return financiamiento;
        }

        public bool EditarFinanciamiento(Financiamiento financiamiento, int codProyecto)
        {
            int result = -1;
            object newProdID;

            string update = "UPDATE FINANCIAMIENTO SET monto_financiamiento=?, ano_final_pago=?, " +
                "ano_inicial_pago=?, interes_fijo=? WHERE cod_financiamiento=?";

            string delete1 = "DELETE FROM INTERES_FINANCIAMIENTO WHERE cod_proyecto=?";

            string insert2 = "INSERT INTO INTERES_FINANCIAMIENTO(cod_proyecto, porcentaje_interes, ano_interes) " +
                "VALUES(?,?,?); " +
                "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(update, conn);
                    SQLiteCommand cmd2 = null;

                    cmd.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
                    cmd.Parameters.AddWithValue("ano_final_pago", financiamiento.AnoFinalPago);
                    cmd.Parameters.AddWithValue("ano_inicial_pago", financiamiento.AnoInicialPago);
                    cmd.Parameters.AddWithValue("interes_fijo", financiamiento.InteresFijo ? 1 : 0);
                    cmd.Parameters.AddWithValue("cod_financiamiento", financiamiento.CodFinanciamiento);

                    transaction = conn.BeginTransaction();

                    result = cmd.ExecuteNonQuery();
                    if (result == -1)
                        transaction.Rollback();

                    cmd = new SQLiteCommand(delete1, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    cmd.ExecuteNonQuery();
                    if (result == -1)
                        transaction.Rollback();

                    foreach (InteresFinanciamiento detTemp in financiamiento.TasaIntereses)
                    {
                        cmd2 = new SQLiteCommand(insert2, conn);
                        cmd2.Parameters.AddWithValue("cod_proyecto", codProyecto);
                        cmd2.Parameters.AddWithValue("porcentaje_interes", detTemp.PorcentajeInteres);
                        cmd2.Parameters.AddWithValue("ano_intereses", detTemp.AnoInteres);
                        newProdID = cmd2.ExecuteScalar();
                        detTemp.CodInteresFinanciamiento = int.Parse(newProdID.ToString());
                    }

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        private List<InteresFinanciamiento> GetInteresesFinanciamiento(int codProyecto)
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
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    listaInteresesFinanciamiento = new List<InteresFinanciamiento>();
                }
            }

            return listaInteresesFinanciamiento;
        }
    }
}
