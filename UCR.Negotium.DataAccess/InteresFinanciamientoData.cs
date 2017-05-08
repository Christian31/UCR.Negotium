using System;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class InteresFinanciamientoData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public InteresFinanciamientoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public bool InsertarInteresFinanciamiento(InteresFinanciamiento intFinanciamiento, int codProyecto)
        {
            String insert = "INSERT INTO INTERES_FINANCIAMIENTO(cod_proyecto, porcentaje_interes_financiamiento, variable_interes)" +
                "VALUES(?,?,?)";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            command.Parameters.AddWithValue("porcentaje_interes_financiamiento", intFinanciamiento.PorcentajeInteresFinanciamiento);
            command.Parameters.AddWithValue("variable_intereses", intFinanciamiento.VariableInteres);
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
        }//InsertarInteresFinanciamiento

        public List<InteresFinanciamiento> GetInteresesFinanciamiento(int codProyecto)
        {
            command = conexion.CreateCommand();
            List<InteresFinanciamiento> listaInteresesFinanciamiento = new List<InteresFinanciamiento>();
            try
            {
                String select = "SELECT * FROM INTERES_FINANCIAMIENTO"+
                    " WHERE cod_proyecto=" + codProyecto + ";";

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                InteresFinanciamiento interes = null;
                while (reader.Read())
                {
                    interes = new InteresFinanciamiento();
                    interes.CodInteresFinanciamiento = reader.GetInt32(0);
                    interes.PorcentajeInteresFinanciamiento = reader.GetDouble(2);
                    interes.VariableInteres = reader.GetBoolean(3);
                    listaInteresesFinanciamiento.Add(interes);
                }//if
                conexion.Close();
                return listaInteresesFinanciamiento;
            }//try
            catch
            {
                conexion.Close();
                return null;
            }//catch
        }//GetInteresesFinanciamiento

        public bool ActualizarInteresFinanciamiento(InteresFinanciamiento intFinanciamiento)
        {
            String update = "UPDATE INTERES_FINANCIAMIENTO SET porcentaje_interes_financiamiento=? " +
                "WHERE cod_interes_financiamiento=" + intFinanciamiento.CodInteresFinanciamiento;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = update;
            command.Parameters.AddWithValue("porcentaje_interes_financiamiento", intFinanciamiento.PorcentajeInteresFinanciamiento);
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
        }//ActualizarInteresFinanciamiento
    }
}
