using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class ReinversionData:BaseData
    {
        public ReinversionData() { }

        public int InsertarReinversion(Reinversion reinversion, int codProyecto)
        {
            object newProdID;
            int idReinversion = -1;
            string insert = "INSERT INTO REQUERIMIENTO_REINVERSION(ano_reinversion, " +
                "descripcion_requerimiento, cantidad, costo_unitario, depreciable, vida_util, " +
                "cod_unidad_medida, cod_proyecto, cod_requerimiento_inversion) " +
                "VALUES(?,?,?,?,?,?,?,?,?); SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(insert, conn);
                    command.Parameters.AddWithValue("ano_reinversion", reinversion.AnoReinversion);
                    command.Parameters.AddWithValue("descripcion_requerimiento", reinversion.Descripcion);
                    command.Parameters.AddWithValue("cantidad", reinversion.Cantidad);
                    command.Parameters.AddWithValue("costo_unitario", reinversion.CostoUnitario);
                    command.Parameters.AddWithValue("depreciable", reinversion.Depreciable);
                    command.Parameters.AddWithValue("vida_util", reinversion.VidaUtil);
                    command.Parameters.AddWithValue("cod_unidad_medida", reinversion.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    command.Parameters.AddWithValue("cod_requerimiento_inversion", reinversion.CodInversion);

                    newProdID = command.ExecuteScalar();
                    idReinversion = int.Parse(newProdID.ToString());
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    idReinversion = -1;
                }
            }

            return idReinversion;
        }

        public bool EditarReinversion(Reinversion reinversion)
        {
            int result = -1;
            string update = "UPDATE REQUERIMIENTO_REINVERSION SET descripcion_requerimiento=?, " +
                "cantidad=?, costo_unitario=?, cod_unidad_medida=?, depreciable=?, " +
                "vida_util=?, ano_reinversion=?, cod_requerimiento_inversion=? " +
                "WHERE cod_requerimiento_reinversion=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update, conn);
                    command.Parameters.AddWithValue("descripcion_requerimiento", reinversion.Descripcion);
                    command.Parameters.AddWithValue("cantidad", reinversion.Cantidad);
                    command.Parameters.AddWithValue("costo_unitario", reinversion.CostoUnitario);
                    command.Parameters.AddWithValue("cod_unidad_medida", reinversion.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("depreciable", reinversion.Depreciable);
                    command.Parameters.AddWithValue("vida_util", reinversion.VidaUtil);
                    command.Parameters.AddWithValue("ano_reinversion", reinversion.AnoReinversion);
                    command.Parameters.AddWithValue("cod_requerimiento_inversion", reinversion.CodInversion);
                    command.Parameters.AddWithValue("cod_requerimiento_reinversion", reinversion.CodReinversion);

                    result = command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        public List<Reinversion> GetReinversiones(int codProyecto)
        {
            List<Reinversion> listaReinversiones = new List<Reinversion>();
            string select = "SELECT r.cod_requerimiento_reinversion, ano_reinversion, descripcion_requerimiento, " +
                    "r.cantidad, r.costo_unitario, r.depreciable, r.vida_util, " +
                    "u.cod_unidad, u.nombre_unidad, r.cod_requerimiento_inversion " +
                    "FROM REQUERIMIENTO_REINVERSION r, UNIDAD_MEDIDA u " +
                    "WHERE r.cod_proyecto=? AND r.cod_unidad_medida=u.cod_unidad";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reinversion requerimiento = new Reinversion();
                            requerimiento.CodReinversion = reader.GetInt32(0);
                            requerimiento.AnoReinversion = reader.GetInt32(1);
                            requerimiento.Descripcion = reader.GetString(2);
                            requerimiento.Cantidad = reader.GetDouble(3);
                            requerimiento.CostoUnitario = reader.GetDouble(4);
                            requerimiento.Depreciable = reader.GetBoolean(5);
                            requerimiento.VidaUtil = reader.GetInt32(6);
                            requerimiento.UnidadMedida.CodUnidad = reader.GetInt32(7);
                            requerimiento.UnidadMedida.NombreUnidad = reader.GetString(8);
                            requerimiento.CodInversion = reader.GetInt32(9);
                            listaReinversiones.Add(requerimiento);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    listaReinversiones = new List<Reinversion>();
                }
            }

            return listaReinversiones;
        }

        public Reinversion GetReinversion(int codReinversion)
        {
            Reinversion reinversion = new Reinversion();
            string select = "SELECT r.cod_requerimiento_reinversion, ano_reinversion, descripcion_requerimiento, " +
                    "r.cantidad, r.costo_unitario, r.depreciable, r.vida_util, " +
                    "u.cod_unidad, u.nombre_unidad, r.cod_requerimiento_inversion " +
                    "FROM REQUERIMIENTO_REINVERSION r, UNIDAD_MEDIDA u " +
                    "WHERE r.cod_requerimiento_reinversion=? AND r.cod_unidad_medida=u.cod_unidad";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_requerimiento_reinversion", codReinversion);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reinversion.CodReinversion = reader.GetInt32(0);
                            reinversion.AnoReinversion = reader.GetInt32(1);
                            reinversion.Descripcion = reader.GetString(2);
                            reinversion.Cantidad = reader.GetDouble(3);
                            reinversion.CostoUnitario = reader.GetDouble(4);
                            reinversion.Depreciable = reader.GetBoolean(5);
                            reinversion.VidaUtil = reader.GetInt32(6);
                            reinversion.UnidadMedida.CodUnidad = reader.GetInt32(7);
                            reinversion.UnidadMedida.NombreUnidad = reader.GetString(8);
                            reinversion.CodInversion = reader.GetInt32(9);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    reinversion = new Reinversion();
                }
            }

            return reinversion;
        }

        public bool EliminarReinversion(int codReinversion)
        {
            int result = -1;
            string delete = "DELETE FROM REQUERIMIENTO_REINVERSION WHERE cod_requerimiento_reinversion=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(delete, conn);
                    cmd.Parameters.AddWithValue("cod_requerimiento_reinversion", codReinversion);

                    result = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }
    }
}
