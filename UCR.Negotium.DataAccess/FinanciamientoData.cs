using System;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class FinanciamientoData:BaseData
    {
        public FinanciamientoData() { }

        public Financiamiento InsertarFinanciamiento(Financiamiento financiamiento, int codProyecto)
        {
            object newProdID;
            string insert = "INSERT INTO FINANCIAMIENTO(cod_proyecto, monto_financiamiento, interes_fijo, " +
                "ano_final_pago, ano_inicial_pago) VALUES(?,?,?,?,?); " +
                "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    cmd.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
                    cmd.Parameters.AddWithValue("interes_fijo", financiamiento.InteresFijo ? 1 : 0);
                    cmd.Parameters.AddWithValue("ano_final_pago", financiamiento.AnoFinalPago);
                    cmd.Parameters.AddWithValue("ano_inicial_pago", financiamiento.AnoInicialPago);

                    newProdID = cmd.ExecuteScalar();
                    financiamiento.CodFinanciamiento = int.Parse(newProdID.ToString());
                }
                catch(Exception ex)
                {
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

        public bool EditarFinanciamiento(Financiamiento financiamiento)
        {
            int result = -1;
            string update = "UPDATE FINANCIAMIENTO SET monto_financiamiento=?, ano_final_pago=?, " +
                "ano_inicial_pago=?, interes_fijo=? WHERE cod_financiamiento=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(update, conn);
                    cmd.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
                    cmd.Parameters.AddWithValue("ano_final_pago", financiamiento.AnoFinalPago);
                    cmd.Parameters.AddWithValue("ano_inicial_pago", financiamiento.AnoInicialPago);
                    cmd.Parameters.AddWithValue("interes_fijo", financiamiento.InteresFijo ? 1 : 0);
                    cmd.Parameters.AddWithValue("cod_financiamiento", financiamiento.CodFinanciamiento);

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
