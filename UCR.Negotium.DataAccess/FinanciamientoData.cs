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
            String insert = "INSERT INTO FINANCIAMIENTO(cod_proyecto, monto_financiamiento, interes_fijo, " +
                "tiempo_financiamiento, ano_inicial_pago) " +
                "VALUES(?,?,?,?,?)";

            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = insert;
                command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                command.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
                command.Parameters.AddWithValue("interes_fijo", financiamiento.InteresFijo);
                command.Parameters.AddWithValue("tiempo_financiamiento", financiamiento.TiempoFinanciamiento);
                command.Parameters.AddWithValue("ano_inicial_pago", financiamiento.AnoInicialPago);
                // Ejecutamos la sentencia INSERT y cerramos la conexión
                if (command.ExecuteNonQuery() != -1)
                {
                    conexion.Close();
                    return this.GetFinanciamiento(codProyecto);
                }//if
                else
                {
                    conexion.Close();
                    return new Financiamiento();
                }//else
            }
            catch
            {
                conexion.Close();
                return new Financiamiento();
            }
            
        }//InsertarFinanciamiento

        public Financiamiento GetFinanciamiento(int codProyecto)
        {
            string select = "SELECT p.cod_financiamiento, p.monto_financiamiento, p.interes_fijo, " +
                "p.tiempo_financiamiento, p.ano_inicial_pago " +
                "FROM FINANCIAMIENTO p WHERE p.cod_proyecto=" + codProyecto;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            Financiamiento financiamiento = new Financiamiento();
            if (reader.Read())
            {
                financiamiento = new Financiamiento();
                financiamiento.CodFinanciamiento = reader.GetInt32(0);
                financiamiento.MontoFinanciamiento = reader.GetDouble(1);
                financiamiento.InteresFijo = reader.GetBoolean(2);
                financiamiento.TiempoFinanciamiento = reader.GetInt32(3);
                financiamiento.AnoInicialPago = reader.GetInt32(4);
            }//if
            conexion.Close();
            return financiamiento;
        }//GetFinanciamiento

        public bool ActualizarFinanciamiento(Financiamiento financiamiento)
        {
            String update = "UPDATE FINANCIAMIENTO SET monto_financiamiento=?, tiempo_financiamiento=?, " +
                "ano_inicial_pago=?, interes_fijo=? " +
                "WHERE cod_financiamiento=" + financiamiento.CodFinanciamiento;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = update;
            command.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
            command.Parameters.AddWithValue("tiempo_financiamiento", financiamiento.TiempoFinanciamiento);
            command.Parameters.AddWithValue("ano_inicial_pago", financiamiento.AnoInicialPago);
            command.Parameters.AddWithValue("interes_fijo", financiamiento.InteresFijo);

            // Ejecutamos la sentencia INSERT y cerramos la conexión
            if (command.ExecuteNonQuery() != -1)
            {
                conexion.Close();
                return true;
            }//if
            else
            {
                conexion.Close();
                return false;
            }//else
        }//ActualizarFinanciamiento
    }
}
