﻿//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class RequerimientoReinversionData
    {

        private String cadenaConexion;
        private SQLiteConnection conexion;

        public RequerimientoReinversionData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public RequerimientoReinversion InsertarRequerimientosReinversion(RequerimientoReinversion requerimientoReinversion, int codProyecto)
        {
            Object newProdID;
            String insert = "INSERT INTO REQUERIMIENTO_REINVERSION(ano_reinversion," +
                " descripcion_requerimiento, cantidad, " +
                "costo_unitario, depreciable, cod_proyecto) " +
            "VALUES(?,?,?,?,?,?); " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("ano_reinversion", requerimientoReinversion.AnoReinversion);
            command.Parameters.AddWithValue("descripcion_requerimiento", requerimientoReinversion.DescripcionRequerimiento);
            command.Parameters.AddWithValue("cantidad", requerimientoReinversion.Cantidad);
            command.Parameters.AddWithValue("costo_unitario", requerimientoReinversion.CostoUnitario);
            command.Parameters.AddWithValue("depreciable", requerimientoReinversion.Depreciable);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                requerimientoReinversion.CodRequerimientoReinversion = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return requerimientoReinversion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return requerimientoReinversion;
            }//catch
        }//InsertarRequerimientosInvesion

        public List<RequerimientoReinversion> GetRequerimientosReinversion(int codProyecto)
        {
            List<RequerimientoReinversion> listaRequerimientos = new List<RequerimientoReinversion>();
            try
            {
                String select = "SELECT * FROM REQUERIMIENTO_REINVERSION WHERE cod_proyecto=" + codProyecto;
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RequerimientoReinversion requerimiento = new RequerimientoReinversion();
                    requerimiento.CodRequerimientoReinversion = reader.GetInt32(0);
                    requerimiento.AnoReinversion = reader.GetInt32(1);
                    requerimiento.DescripcionRequerimiento = reader.GetString(2);
                    requerimiento.Cantidad = reader.GetInt32(3);
                    requerimiento.CostoUnitario = reader.GetDouble(4);
                    requerimiento.Depreciable = reader.GetBoolean(5);
                    listaRequerimientos.Add(requerimiento);
                }//while
                conexion.Close();
                return listaRequerimientos;
            }//try
            catch
            {
                conexion.Close();
                return listaRequerimientos;
            }//catch

        }//GetRequerimientosInversion
    }//RequerimientoReinversionData
}
