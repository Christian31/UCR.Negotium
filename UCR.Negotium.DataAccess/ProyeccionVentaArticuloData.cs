using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class ProyeccionVentaArticuloData:BaseData
    {
        private UnidadMedidaData unidadMedidaData;
        private DetalleProyeccionVentaData detalleProyeccionData;
        private CrecimientoOfertaArticuloData crecimientoOfertaData;

        public ProyeccionVentaArticuloData()
        {
            unidadMedidaData = new UnidadMedidaData();
            detalleProyeccionData = new DetalleProyeccionVentaData();
            crecimientoOfertaData = new CrecimientoOfertaArticuloData();
        }

        public List<ProyeccionVentaArticulo> GetProyeccionesVentaArticulo(int codProyecto)
        {
            List<ProyeccionVentaArticulo> listaProyecciones = new List<ProyeccionVentaArticulo>();
            string select = "SELECT * FROM PROYECCION_VENTA_POR_ARTICULO WHERE cod_proyecto=?";

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
                            ProyeccionVentaArticulo proyeccion = new ProyeccionVentaArticulo();
                            proyeccion.CodArticulo = reader.GetInt32(0);
                            proyeccion.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(1));
                            proyeccion.NombreArticulo = reader.GetString(2);
                            proyeccion.DetallesProyeccionVenta = detalleProyeccionData.GetDetallesProyeccionVenta(reader.GetInt32(0));
                            proyeccion.CrecimientoOferta = crecimientoOfertaData.GetCrecimientoOfertaObjetoIntereses(reader.GetInt32(0));
                            listaProyecciones.Add(proyeccion);
                        }
                    }
                }
                catch
                {
                    listaProyecciones = new List<ProyeccionVentaArticulo>();
                }
            }

            return listaProyecciones;
        } 

        public ProyeccionVentaArticulo GetProyeccionVentaArticulo(int codProyeccion)
        {
            ProyeccionVentaArticulo proyeccion = new ProyeccionVentaArticulo();
            string select = "SELECT * FROM PROYECCION_VENTA_POR_ARTICULO WHERE cod_articulo=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_articulo", codProyeccion);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            proyeccion.CodArticulo = reader.GetInt32(0);
                            proyeccion.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(1));
                            proyeccion.NombreArticulo = reader.GetString(2);
                            proyeccion.DetallesProyeccionVenta = detalleProyeccionData.GetDetallesProyeccionVenta(reader.GetInt32(0));
                            proyeccion.CrecimientoOferta = crecimientoOfertaData.GetCrecimientoOfertaObjetoIntereses(reader.GetInt32(0));
                        }//if
                    }
                    }
                catch
                {
                    proyeccion = new ProyeccionVentaArticulo();
                }
            }

            return proyeccion;
        }

        public ProyeccionVentaArticulo InsertarProyeccionVenta(ProyeccionVentaArticulo proyeccion, int codProyecto)
        {
            object newProdID, newProdID2;

            string insert1 = "INSERT INTO PROYECCION_VENTA_POR_ARTICULO(cod_unidad_medida, " +
                "nombre_articulo, cod_proyecto) VALUES(?,?,?); " +
                "SELECT last_insert_rowid();";

            string insert2 = "INSERT INTO DETALLE_PROYECCION_VENTA(cod_proyeccion_venta_articulo, mes_proyeccion, " +
                "cantidad_proyeccion, precio_proyeccion) VALUES(?,?,?,?); " +
                "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(insert1, conn);
                    SQLiteCommand command2 = null;

                    command.Parameters.AddWithValue("cod_unidad_medida", proyeccion.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("nombre_articulo", proyeccion.NombreArticulo);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    transaction = conn.BeginTransaction();

                    newProdID = command.ExecuteScalar();
                    proyeccion.CodArticulo = int.Parse(newProdID.ToString());
                    foreach (DetalleProyeccionVenta detTemp in proyeccion.DetallesProyeccionVenta)
                    {
                        command2 = new SQLiteCommand(insert2, conn);
                        command2.Parameters.AddWithValue("cod_proyeccion_venta_articulo", proyeccion.CodArticulo);
                        command2.Parameters.AddWithValue("mes_proyeccion", detTemp.Mes);
                        command2.Parameters.AddWithValue("cantidad_proyeccion", detTemp.Cantidad);
                        command2.Parameters.AddWithValue("precio_proyeccion", detTemp.Precio);
                        newProdID2 = command2.ExecuteScalar();
                        detTemp.CodDetalle = int.Parse(newProdID2.ToString());
                    }

                    transaction.Commit();
                }
                catch
                {
                    proyeccion = new ProyeccionVentaArticulo();
                    transaction.Rollback();
                }
            }

            return proyeccion;
        }

        public bool EditarProyeccionVenta(ProyeccionVentaArticulo proyeccion)
        {
            int result = -1;

            string update1 = "UPDATE PROYECCION_VENTA_POR_ARTICULO SET cod_unidad_medida=?, nombre_articulo=? " +
                "WHERE cod_articulo=?";

            string update2 = "UPDATE DETALLE_PROYECCION_VENTA SET cantidad_proyeccion=?, precio_proyeccion=? " +
                "WHERE cod_detalle=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update1, conn);
                    SQLiteCommand command2 = null;

                    command.Parameters.AddWithValue("cod_unidad_medida", proyeccion.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("nombre_articulo", proyeccion.NombreArticulo);
                    command.Parameters.AddWithValue("cod_articulo", proyeccion.CodArticulo);

                    transaction = conn.BeginTransaction();

                    result = command.ExecuteNonQuery();
                    if (result != -1)
                    {
                        foreach (DetalleProyeccionVenta detTemp in proyeccion.DetallesProyeccionVenta)
                        {
                            command2 = new SQLiteCommand(update2, conn);
                            command2.Parameters.AddWithValue("cantidad_proyeccion", detTemp.Cantidad);
                            command2.Parameters.AddWithValue("precio_proyeccion", detTemp.Precio);
                            command2.Parameters.AddWithValue("cod_detalle", detTemp.CodDetalle);
                            result = command2.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                catch
                {
                    result = -1;
                    transaction.Rollback();
                }
            }

            return result != -1;
        }

        public bool EliminarProyeccionVenta(int codProyeccion)
        {
            int result = -1;
            string sqlQuery2 = "DELETE FROM PROYECCION_VENTA_POR_ARTICULO WHERE cod_articulo=?";
            string sqlQuery1 = "DELETE FROM DETALLE_PROYECCION_VENTA WHERE cod_proyeccion_venta_articulo=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();

                    SQLiteCommand command = new SQLiteCommand(sqlQuery1, conn);
                    SQLiteCommand command2 = new SQLiteCommand(sqlQuery2, conn);

                    command.Parameters.AddWithValue("cod_articulo", codProyeccion);
                    command2.Parameters.AddWithValue("cod_proyeccion_venta_articulo", codProyeccion);

                    transaction = conn.BeginTransaction();

                    result = command.ExecuteNonQuery();
                    if (result != -1)
                    {
                        result = command2.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
                catch
                {
                    result = -1;
                    transaction.Rollback();
                }
            }

            return result != -1;
        }
    }
}
