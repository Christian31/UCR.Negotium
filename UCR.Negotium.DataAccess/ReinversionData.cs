using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class ReinversionData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;

        public ReinversionData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public int InsertarRequerimientosReinversion(Reinversion requerimientoReinversion, int codProyecto)
        {
            int idReinversion = -1;
            string insert = "INSERT INTO REQUERIMIENTO_REINVERSION(ano_reinversion," +
                " descripcion_requerimiento, cantidad, " +
                "costo_unitario, depreciable, vida_util, cod_unidad_medida, cod_proyecto, cod_requerimiento_inversion) " +
            "VALUES(?,?,?,?,?,?,?,?,?); " +
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
            command.Parameters.AddWithValue("vida_util", requerimientoReinversion.VidaUtil);
            command.Parameters.AddWithValue("cod_unidad_medida", requerimientoReinversion.UnidadMedida.CodUnidad);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            command.Parameters.AddWithValue("cod_requerimiento_inversion", requerimientoReinversion.CodRequerimientoInversion);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                idReinversion = int.Parse(command.ExecuteScalar().ToString());
                conexion.Close();
                return idReinversion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return idReinversion;
            }//catch
        }//InsertarRequerimientosInvesion

        public bool EditarRequerimientoReinversion(Reinversion requerimientoReinversion)
        {
            string insert = "UPDATE REQUERIMIENTO_REINVERSION SET descripcion_requerimiento = ?, cantidad = ?, " +
                "costo_unitario = ?, cod_unidad_medida = ?, depreciable = ?, vida_util = ?,  ano_reinversion = ?, cod_requerimiento_inversion =? " +
            "WHERE cod_requerimiento_reinversion = ?; " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("descripcion_requerimiento", requerimientoReinversion.DescripcionRequerimiento);
            command.Parameters.AddWithValue("cantidad", requerimientoReinversion.Cantidad);
            command.Parameters.AddWithValue("costo_unitario", requerimientoReinversion.CostoUnitario);
            command.Parameters.AddWithValue("cod_unidad_medida", requerimientoReinversion.UnidadMedida.CodUnidad);
            command.Parameters.AddWithValue("depreciable", requerimientoReinversion.Depreciable);
            command.Parameters.AddWithValue("vida_util", requerimientoReinversion.VidaUtil);
            command.Parameters.AddWithValue("ano_reinversion", requerimientoReinversion.AnoReinversion);
            command.Parameters.AddWithValue("cod_requerimiento_inversion", requerimientoReinversion.CodRequerimientoInversion);
            command.Parameters.AddWithValue("cod_requerimiento_reinversion", requerimientoReinversion.CodRequerimientoReinversion);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                command.ExecuteScalar();
                conexion.Close();
                return true;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return false;
            }//catch
        }

        public List<Reinversion> GetRequerimientosReinversion(int codProyecto)
        {
            List<Reinversion> listaRequerimientos = new List<Reinversion>();
            try
            {
                String select = "SELECT r.cod_requerimiento_reinversion, ano_reinversion, descripcion_requerimiento, "+
                    "r.cantidad, r.costo_unitario, r.depreciable, r.vida_util, " +
                    "u.cod_unidad, u.nombre_unidad, r.cod_requerimiento_inversion " +
                    "FROM REQUERIMIENTO_REINVERSION r, UNIDAD_MEDIDA u " +
                    "WHERE r.cod_proyecto = " + codProyecto +
                    " and r.cod_unidad_medida = u.cod_unidad;";

                //String select = "SELECT * FROM REQUERIMIENTO_REINVERSION WHERE cod_proyecto=" + codProyecto;
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Reinversion requerimiento = new Reinversion();
                    requerimiento.CodRequerimientoReinversion = reader.GetInt32(0);
                    requerimiento.AnoReinversion = reader.GetInt32(1);
                    requerimiento.DescripcionRequerimiento = reader.GetString(2);
                    requerimiento.Cantidad = reader.GetDouble(3);
                    requerimiento.CostoUnitario = reader.GetDouble(4);
                    requerimiento.Depreciable = reader.GetBoolean(5);
                    requerimiento.VidaUtil = reader.GetInt32(6);
                    requerimiento.UnidadMedida.CodUnidad = reader.GetInt32(7);
                    requerimiento.UnidadMedida.NombreUnidad = reader.GetString(8);
                    requerimiento.CodRequerimientoInversion = reader.GetInt32(9);
                    listaRequerimientos.Add(requerimiento);
                }//while
                conexion.Close();
                return listaRequerimientos;
            }//try
            catch (Exception ex)
            {
                conexion.Close();
                return listaRequerimientos;
            }//catch

        }//GetRequerimientosInversion

        public Reinversion GetRequerimientoReinversion(int codReinversion)
        {
            Reinversion requerimiento = new Reinversion();
            try
            {
                string select = "SELECT r.cod_requerimiento_reinversion, ano_reinversion, descripcion_requerimiento, " +
                    "r.cantidad, r.costo_unitario, r.depreciable, r.vida_util, " +
                    "u.cod_unidad, u.nombre_unidad, r.cod_requerimiento_inversion " +
                    "FROM REQUERIMIENTO_REINVERSION r, UNIDAD_MEDIDA u " +
                    "WHERE r.cod_requerimiento_reinversion = " + codReinversion +
                    " and r.cod_unidad_medida = u.cod_unidad;";

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    requerimiento.CodRequerimientoReinversion = reader.GetInt32(0);
                    requerimiento.AnoReinversion = reader.GetInt32(1);
                    requerimiento.DescripcionRequerimiento = reader.GetString(2);
                    requerimiento.Cantidad = reader.GetDouble(3);
                    requerimiento.CostoUnitario = reader.GetDouble(4);
                    requerimiento.Depreciable = reader.GetBoolean(5);
                    requerimiento.VidaUtil = reader.GetInt32(6);
                    requerimiento.UnidadMedida.CodUnidad = reader.GetInt32(7);
                    requerimiento.UnidadMedida.NombreUnidad = reader.GetString(8);
                    requerimiento.CodRequerimientoInversion = reader.GetInt32(9);
                }//while
                conexion.Close();
                return requerimiento;
            }//try
            catch (Exception ex)
            {
                conexion.Close();
                return requerimiento;
            }//catch

        }//GetRequerimientosInversion


        public bool EliminarRequerimientoReinversion(int codRequerimiento)
        {
            try
            {
                string sqlQuery = "DELETE FROM REQUERIMIENTO_REINVERSION WHERE cod_requerimiento_reinversion =" + codRequerimiento + ";";
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = sqlQuery;
                command.ExecuteNonQuery();
                conexion.Close();

                return true;
            }
            catch
            {
                conexion.Close();
                return false;
            }
        }
    }//RequerimientoReinversionData
}
