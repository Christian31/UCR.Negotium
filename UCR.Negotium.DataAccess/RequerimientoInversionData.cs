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
    public class RequerimientoInversionData : BaseData
    {

        public RequerimientoInversionData() { }

        public int InsertarRequerimientosInvesion(RequerimientoInversion requerimientoInversion, int codProyecto)
        {
            int idInversion = -1;
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
                idInversion = int.Parse(command.ExecuteScalar().ToString());
                conexion.Close();
                return idInversion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return idInversion;
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

        public RequerimientoInversion GetRequerimientoInversion(int codInversion)
        {
            RequerimientoInversion requerimiento = new RequerimientoInversion();
            try
            {
                string select = "SELECT r.cod_requerimiento_inversion, r.descripcion_requerimiento, " +
                    "r.cantidad, r.costo_unitario, r.cod_unidad_medida, r.depreciable, " +
                    "r.vida_util, u.nombre_unidad FROM REQUERIMIENTO_INVERSION r, " +
                    "UNIDAD_MEDIDA u WHERE r.cod_requerimiento_inversion = " + codInversion +
                    " and r.cod_unidad_medida = u.cod_unidad;";

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    
                    requerimiento.CodRequerimientoInversion = reader.GetInt32(0);
                    requerimiento.DescripcionRequerimiento = reader.GetString(1);
                    requerimiento.Cantidad = reader.GetInt32(2);
                    requerimiento.CostoUnitario = reader.GetDouble(3);
                    requerimiento.UnidadMedida.CodUnidad = reader.GetInt32(4);
                    requerimiento.Depreciable = reader.GetBoolean(5);
                    requerimiento.VidaUtil = reader.GetInt32(6);
                    requerimiento.UnidadMedida.NombreUnidad = reader.GetString(7);
                }//while
                conexion.Close();
                return requerimiento;
            }//try
            catch
            {
                conexion.Close();
                return requerimiento;
            }//catch

        }//GetRequerimientosInversion

        public bool EditarRequerimientosInvesion(RequerimientoInversion requerimientoInversion)
        {
            string insert = "UPDATE REQUERIMIENTO_INVERSION SET descripcion_requerimiento = ?, cantidad = ?, " +
                "costo_unitario = ?, cod_unidad_medida = ?, depreciable = ?, vida_util = ? " +
            "WHERE cod_requerimiento_inversion = ?; ";
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
            command.Parameters.AddWithValue("cod_requerimiento_inversion", requerimientoInversion.CodRequerimientoInversion);
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
        }//EditarRequerimientosInvesion

        public bool EliminarRequerimientoInversion(int codRequerimiento)
        {
            try
            {
                string sqlQuery = "DELETE FROM REQUERIMIENTO_INVERSION WHERE cod_requerimiento_inversion =" + codRequerimiento + ";";
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command = conexion.CreateCommand();
                command.CommandText = sqlQuery;
                command.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }

        }
    }//RequerimientosInversionData
}
