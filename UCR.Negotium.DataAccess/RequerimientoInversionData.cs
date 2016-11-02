//@Copyright Yordan Campos Piedra
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
    public class RequerimientoInversionData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;

        public RequerimientoInversionData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
            conexion.Open();
        }

        public RequerimientoInversion InsertarRequerimientosInvesion(RequerimientoInversion requerimientoInversion, int codProyecto)
        {
            Object newProdID;
            String insert = "INSERT INTO REQUERIMIENTO_INVERSION(descripcion_requerimiento, cantidad, " +
                "costo_unitario, cod_unidad_medida, depreciable, vida_util, cod_proyecto) " +
            "VALUES(?,?,?,?,?,?,?); " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("descripcion_requerimiento", requerimientoInversion.DescripcionRequerimiento);
            command.Parameters.AddWithValue("cantidad", requerimientoInversion.Cantidad);
            command.Parameters.AddWithValue("costo_unitario", requerimientoInversion.CostoUnitario);
            command.Parameters.AddWithValue("cod_unidad_medida", requerimientoInversion.UnidadMedida.CodUnidad);
            command.Parameters.AddWithValue("depreciable", requerimientoInversion.Depreciable);
            command.Parameters.AddWithValue("vida_util", requerimientoInversion.VidaUtil);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                requerimientoInversion.CodRequerimientoInversion = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return requerimientoInversion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return requerimientoInversion;
            }//catch
        }//InsertarRequerimientosInvesion

        public List<RequerimientoInversion> GetRequerimientosInversion(int codProyecto)
        {
            List<RequerimientoInversion> listaRequerimientos = new List<RequerimientoInversion>();
            try
            {
                String select = "SELECT r.cod_requerimiento_inversion, r.descripcion_requerimiento, " +
                    "r.cantidad, r.costo_unitario, r.cod_unidad_medida, r.depreciable, " +
                    "r.vida_util, u.nombre_unidad FROM REQUERIMIENTO_INVERSION r, " +
                    "UNIDAD_MEDIDA u WHERE r.cod_proyecto = "+ codProyecto +
                    " and r.cod_unidad_medida = u.cod_unidad;";

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RequerimientoInversion requerimiento = new RequerimientoInversion();
                    requerimiento.CodRequerimientoInversion = reader.GetInt32(0);
                    requerimiento.DescripcionRequerimiento = reader.GetString(1);
                    requerimiento.Cantidad = reader.GetInt32(2);
                    requerimiento.CostoUnitario = reader.GetDouble(3);
                    requerimiento.UnidadMedida.CodUnidad = reader.GetInt32(4);
                    requerimiento.Depreciable = reader.GetBoolean(5);
                    requerimiento.VidaUtil = reader.GetInt32(6);
                    requerimiento.UnidadMedida.NombreUnidad = reader.GetString(7);
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

        public RequerimientoInversion EditarRequerimientosInvesion(RequerimientoInversion requerimientoInversion, int codProyecto)
        {
            Object newProdID;
            String insert = "UPDATE REQUERIMIENTO_INVERSION SET (descripcion_requerimiento = ?, cantidad = ?, " +
                "costo_unitario = ?, cod_unidad_medida = ?, depreciable = ?, vida_util = ?, cod_proyecto = ? " +
            "WHERE cod_requerimiento_inversion = ?; " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("descripcion_requerimiento", requerimientoInversion.DescripcionRequerimiento);
            command.Parameters.AddWithValue("cantidad", requerimientoInversion.Cantidad);
            command.Parameters.AddWithValue("costo_unitario", requerimientoInversion.CostoUnitario);
            command.Parameters.AddWithValue("cod_unidad_medida", requerimientoInversion.UnidadMedida.CodUnidad);
            command.Parameters.AddWithValue("depreciable", requerimientoInversion.Depreciable);
            command.Parameters.AddWithValue("vida_util", requerimientoInversion.VidaUtil);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            command.Parameters.AddWithValue("cod_requerimiento_inversion", requerimientoInversion.CodRequerimientoInversion);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                requerimientoInversion.CodRequerimientoInversion = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return requerimientoInversion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return requerimientoInversion;
            }//catch
        }//EditarRequerimientosInvesion

        //public List<RequerimientoInversion> EliminarRequerimientosInversion(int codProyecto)
        //{
        //    List<RequerimientoInversion> listaRequerimientos = new List<RequerimientoInversion>();
        //    try
        //    {
        //        String select = "DELETE FROM REQUERIMIENTO_INVERSION WHERE cod_proyecto = " + codProyecto;

        //        if (conexion.State != ConnectionState.Open)
        //            conexion.Open();
        //        SQLiteCommand command = conexion.CreateCommand();
        //        command = conexion.CreateCommand();
        //        command.CommandText = select;
        //        command.ExecuteReader();

        //        conexion.Close();
        //        return listaRequerimientos;
        //    }//try
        //    catch
        //    {
        //        conexion.Close();
        //        return listaRequerimientos;
        //    }//catch

        //}//EliminarRequerimientosInversion
    }//RequerimientosInversionData
}
