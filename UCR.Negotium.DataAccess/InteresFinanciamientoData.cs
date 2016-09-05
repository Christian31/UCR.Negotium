﻿using System;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class InteresFinanciamientoData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public InteresFinanciamientoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public bool InsertarInteresFinanciamiento(InteresFinanciamiento intFinanciamiento, int codProyecto)
        {
            String insert = "INSERT INTO INTERES_FINANCIAMIENTO(cod_proyecto, porcentaje_interes_financiamiento)" +
                "VALUES(?,?)";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
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
        }//InsertarInteresFinanciamiento

        public DataTable GetInteresesFinanciamiento(int codProyecto)
        {
            SQLiteCommand command = conexion.CreateCommand();
            List<InteresFinanciamiento> listaInteresesFinanciamiento = new List<InteresFinanciamiento>();
            try
            {
                String select = "SELECT * FROM INTERESES_FINANCIAMIENTO WHERE cod_proyecto=" + codProyecto + ";";

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                command.CommandText = select;
                SQLiteDataAdapter daIntereses = new SQLiteDataAdapter();
                daIntereses.SelectCommand = new SQLiteCommand(select, conexion);
                DataSet dsIntereses = new DataSet();
                daIntereses.Fill(dsIntereses, "InteresesFinanciamiento");
                DataTable dtIntereses = dsIntereses.Tables["InteresesFinanciamiento"];
                conexion.Close();
                return dtIntereses;
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