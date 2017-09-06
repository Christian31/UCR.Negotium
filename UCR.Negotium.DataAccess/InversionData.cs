using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class InversionData:BaseData
    {
        public InversionData() { }

        public int InsertarInvesion(Inversion inversion, int codProyecto)
        {
            object newProdID;
            int idInversion = -1;
            string insert = "INSERT INTO REQUERIMIENTO_INVERSION(descripcion_requerimiento, cantidad, " +
                "costo_unitario, cod_unidad_medida, depreciable, vida_util, cod_proyecto) " +
                "VALUES(?,?,?,?,?,?,?); SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(insert, conn);
                    command.Parameters.AddWithValue("descripcion_requerimiento", inversion.DescripcionRequerimiento);
                    command.Parameters.AddWithValue("cantidad", inversion.Cantidad);
                    command.Parameters.AddWithValue("costo_unitario", inversion.CostoUnitario);
                    command.Parameters.AddWithValue("cod_unidad_medida", inversion.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("depreciable", inversion.Depreciable);
                    command.Parameters.AddWithValue("vida_util", inversion.VidaUtil);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    newProdID = command.ExecuteScalar();
                    idInversion = int.Parse(newProdID.ToString());
                }
                catch
                {
                    idInversion = -1;
                }
            }

            return idInversion;
        }

        public List<Inversion> GetInversiones(int codProyecto)
        {
            List<Inversion> listaRequerimientos = new List<Inversion>();
            string select = "SELECT r.cod_requerimiento_inversion, r.descripcion_requerimiento, " +
                "r.cantidad, r.costo_unitario, r.cod_unidad_medida, r.depreciable, " +
                "r.vida_util, u.nombre_unidad FROM REQUERIMIENTO_INVERSION r, " +
                "UNIDAD_MEDIDA u WHERE r.cod_proyecto=? " +
                "AND r.cod_unidad_medida=u.cod_unidad;";

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
                            Inversion requerimiento = new Inversion();
                            requerimiento.CodRequerimientoInversion = reader.GetInt32(0);
                            requerimiento.DescripcionRequerimiento = reader.GetString(1);
                            requerimiento.Cantidad = reader.GetDouble(2);
                            requerimiento.CostoUnitario = reader.GetDouble(3);
                            requerimiento.UnidadMedida.CodUnidad = reader.GetInt32(4);
                            requerimiento.Depreciable = reader.GetBoolean(5);
                            requerimiento.VidaUtil = reader.GetInt32(6);
                            requerimiento.UnidadMedida.NombreUnidad = reader.GetString(7);
                            listaRequerimientos.Add(requerimiento);
                        }
                    }
                }
                catch
                {
                    listaRequerimientos = new List<Inversion>();
                }
            }

            return listaRequerimientos;
        }

        public Inversion GetInversion(int codInversion)
        {
            Inversion inversion = new Inversion();
            string select = "SELECT r.cod_requerimiento_inversion, r.descripcion_requerimiento, " +
                "r.cantidad, r.costo_unitario, r.cod_unidad_medida, r.depreciable, " +
                "r.vida_util, u.nombre_unidad FROM REQUERIMIENTO_INVERSION r, " +
                "UNIDAD_MEDIDA u WHERE r.cod_requerimiento_inversion=? " +
                "AND r.cod_unidad_medida=u.cod_unidad";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_requerimiento_inversion", codInversion);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inversion.CodRequerimientoInversion = reader.GetInt32(0);
                            inversion.DescripcionRequerimiento = reader.GetString(1);
                            inversion.Cantidad = reader.GetDouble(2);
                            inversion.CostoUnitario = reader.GetDouble(3);
                            inversion.UnidadMedida.CodUnidad = reader.GetInt32(4);
                            inversion.Depreciable = reader.GetBoolean(5);
                            inversion.VidaUtil = reader.GetInt32(6);
                            inversion.UnidadMedida.NombreUnidad = reader.GetString(7);
                        }
                    }
                }
                catch
                {
                    inversion = new Inversion();
                }
            }

            return inversion;
        }

        public bool EditarInvesion(Inversion inversion)
        {
            int result = -1;
            string update = "UPDATE REQUERIMIENTO_INVERSION SET descripcion_requerimiento=?, cantidad=?, " +
                "costo_unitario=?, cod_unidad_medida=?, depreciable=?, vida_util=? " +
                "WHERE cod_requerimiento_inversion=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update, conn);
                    command.Parameters.AddWithValue("descripcion_requerimiento", inversion.DescripcionRequerimiento);
                    command.Parameters.AddWithValue("cantidad", inversion.Cantidad);
                    command.Parameters.AddWithValue("costo_unitario", inversion.CostoUnitario);
                    command.Parameters.AddWithValue("cod_unidad_medida", inversion.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("depreciable", inversion.Depreciable);
                    command.Parameters.AddWithValue("vida_util", inversion.VidaUtil);
                    command.Parameters.AddWithValue("cod_requerimiento_inversion", inversion.CodRequerimientoInversion);

                    result = command.ExecuteNonQuery();
                }
                catch
                {
                    result = -1;
                }
            }

            return result != -1;
        }

        public bool EliminarInversion(int codInversion)
        {
            int result = -1;
            string delete = "DELETE FROM REQUERIMIENTO_INVERSION WHERE cod_requerimiento_inversion=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(delete, conn);
                    cmd.Parameters.AddWithValue("cod_requerimiento_inversion", codInversion);

                    result = cmd.ExecuteNonQuery();
                }
                catch
                {
                    result = -1;
                }
            }

            return result != -1;
        }
    }
}
