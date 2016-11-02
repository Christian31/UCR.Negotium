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
    public class RequerimientoReinversionData
    {

        private String cadenaConexion;
        private SQLiteConnection conexion;

        public RequerimientoReinversionData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public RequerimientoReinversion InsertarRequerimientosReinversion(RequerimientoReinversion requerimientoReinversion, int codProyecto)
        {
            Object newProdID;
            String insert = "INSERT INTO REQUERIMIENTO_REINVERSION(ano_reinversion," +
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
                    RequerimientoReinversion requerimiento = new RequerimientoReinversion();
                    requerimiento.CodRequerimientoReinversion = reader.GetInt32(0);
                    requerimiento.AnoReinversion = reader.GetInt32(1);
                    requerimiento.DescripcionRequerimiento = reader.GetString(2);
                    requerimiento.Cantidad = reader.GetInt32(3);
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
    }//RequerimientoReinversionData
}
