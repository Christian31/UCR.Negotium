using System;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class FinanciamientoData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public FinanciamientoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public Financiamiento InsertarFinanciamiento(Financiamiento financiamiento, int codProyecto)
        {
            string insert = "INSERT INTO FINANCIAMIENTO(cod_proyecto, monto_financiamiento, interes_fijo, " +
                "ano_final_pago, ano_inicial_pago) " +
                "VALUES(?,?,?,?,?); "
                + "SELECT last_insert_rowid();";

            try
            {
                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = insert;
                command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                command.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
                command.Parameters.AddWithValue("interes_fijo", financiamiento.InteresFijo?1:0);
                command.Parameters.AddWithValue("ano_final_pago", financiamiento.AnoFinalPago);
                command.Parameters.AddWithValue("ano_inicial_pago", financiamiento.AnoInicialPago);

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                financiamiento.CodFinanciamiento = int.Parse(command.ExecuteScalar().ToString());

                return financiamiento;
            }
            catch
            {
                return new Financiamiento();
            }
            finally
            {
                conexion.Close();
            }
            
        }

        public Financiamiento GetFinanciamiento(int codProyecto)
        {
            string select = "SELECT p.cod_financiamiento, p.monto_financiamiento, p.interes_fijo, " +
                "p.ano_final_pago, p.ano_inicial_pago " +
                "FROM FINANCIAMIENTO p WHERE p.cod_proyecto=" + codProyecto;
            Financiamiento financiamiento = new Financiamiento();

            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    financiamiento = new Financiamiento();
                    financiamiento.CodFinanciamiento = reader.GetInt32(0);
                    financiamiento.MontoFinanciamiento = reader.GetDouble(1);
                    financiamiento.InteresFijo = reader.GetBoolean(2);
                    financiamiento.AnoFinalPago = reader.GetInt32(3);
                    financiamiento.AnoInicialPago = reader.GetInt32(4);
                }//if
                conexion.Close();
                return financiamiento;
            }
            catch
            {
                return financiamiento;
            }
            finally
            {
                conexion.Close();
            }
            
        }

        public bool ActualizarFinanciamiento(Financiamiento financiamiento)
        {
            string update = "UPDATE FINANCIAMIENTO SET monto_financiamiento=?, ano_final_pago=?, " +
                "ano_inicial_pago=?, interes_fijo=? " +
                "WHERE cod_financiamiento=" + financiamiento.CodFinanciamiento;

            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = update;
                command.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
                command.Parameters.AddWithValue("ano_final_pago", financiamiento.AnoFinalPago);
                command.Parameters.AddWithValue("ano_inicial_pago", financiamiento.AnoInicialPago);
                command.Parameters.AddWithValue("interes_fijo", financiamiento.InteresFijo ? 1 : 0);

                return command.ExecuteNonQuery() != -1;
            }
            catch
            {
                return false;
            }
            finally
            {
                conexion.Close();
            }
            
        }
    }
}
