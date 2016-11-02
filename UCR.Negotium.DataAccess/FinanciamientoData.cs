using System;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class FinanciamientoData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public FinanciamientoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public bool InsertarFinanciamiento(Financiamiento financiamiento, int codProyecto)
        {
            String insert = "INSERT INTO FINANCIAMIENTO(cod_proyecto, monto_financiamiento, variable_financiamiento, " +
                "tiempo_financiamiento) " +
                "VALUES(?,?,?,?)";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            command.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
            command.Parameters.AddWithValue("variable_financiamiento", financiamiento.VariableFinanciamiento);
            command.Parameters.AddWithValue("tiempo_financiamiento", financiamiento.TiempoFinanciamiento);
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
        }//InsertarFinanciamiento

        public Financiamiento GetFinanciamiento(int codProyecto, bool variable)
        {
            String select = "SELECT p.cod_financiamiento, p.monto_financiamiento, p.variable_financiamiento, " +
                "p.tiempo_financiamiento " +
                "FROM FINANCIAMIENTO p WHERE p.cod_proyecto=" + codProyecto + " AND p.variable_financiamiento="+Convert.ToInt32(variable);
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            Financiamiento financiamiento = null;
            if (reader.Read())
            {
                financiamiento = new Financiamiento();
                financiamiento.CodFinanciamiento = reader.GetInt32(0);
                financiamiento.MontoFinanciamiento = reader.GetDouble(1);
                financiamiento.VariableFinanciamiento = reader.GetBoolean(2);
                financiamiento.TiempoFinanciamiento = reader.GetInt32(3);
            }//if
            conexion.Close();
            return financiamiento;
        }//GetFinanciamiento

        public bool ActualizarFinanciamiento(Financiamiento financiamiento)
        {
            String update = "UPDATE FINANCIAMIENTO SET monto_financiamiento=?, variable_financiamiento=?, " +
                "tiempo_financiamiento=? " +
                "WHERE cod_financiamiento=" + financiamiento.CodFinanciamiento;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = update;
            command.Parameters.AddWithValue("monto_financiamiento", financiamiento.MontoFinanciamiento);
            command.Parameters.AddWithValue("variable_financiamiento", financiamiento.VariableFinanciamiento);
            command.Parameters.AddWithValue("tiempo_financiamiento", financiamiento.TiempoFinanciamiento);
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
