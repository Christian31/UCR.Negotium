using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class ProyeccionVentaData:BaseData
    {
        private UnidadMedidaData unidadMedidaData;
        private CrecimientoOfertaData crecimientoOfertaData;

        public ProyeccionVentaData()
        {
            unidadMedidaData = new UnidadMedidaData();
            crecimientoOfertaData = new CrecimientoOfertaData();
        }

        public List<ProyeccionVenta> GetProyeccionesVenta(int codProyecto)
        {
            List<ProyeccionVenta> listaProyecciones = new List<ProyeccionVenta>();
            string select = "SELECT * FROM PROYECCION_VENTA WHERE cod_proyecto=?";

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
                            ProyeccionVenta proyeccion = new ProyeccionVenta();
                            proyeccion.CodArticulo = reader.GetInt32(0);
                            proyeccion.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(1));
                            proyeccion.NombreArticulo = reader.GetString(2);
                            proyeccion.DetallesProyeccionVenta = GetDetallesProyeccionVenta(proyeccion.CodArticulo);
                            proyeccion.CrecimientoOferta = crecimientoOfertaData.GetCrecimientoOfertaObjetoIntereses(reader.GetInt32(0));
                            listaProyecciones.Add(proyeccion);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    listaProyecciones = new List<ProyeccionVenta>();
                }
            }

            return listaProyecciones;
        } 

        public ProyeccionVenta GetProyeccionVenta(int codProyeccion)
        {
            ProyeccionVenta proyeccion = new ProyeccionVenta();
            string select = "SELECT * FROM PROYECCION_VENTA WHERE cod_proyeccion=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyeccion", codProyeccion);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            proyeccion.CodArticulo = reader.GetInt32(0);
                            proyeccion.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(1));
                            proyeccion.NombreArticulo = reader.GetString(2);
                            proyeccion.DetallesProyeccionVenta = GetDetallesProyeccionVenta(proyeccion.CodArticulo);
                            proyeccion.CrecimientoOferta = crecimientoOfertaData.GetCrecimientoOfertaObjetoIntereses(reader.GetInt32(0));
                        }//if
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    proyeccion = new ProyeccionVenta();
                }
            }

            return proyeccion;
        }

        public ProyeccionVenta InsertarProyeccionVenta(ProyeccionVenta proyeccion, int codProyecto)
        {
            object newProdID, newProdID2;

            string insert1 = "INSERT INTO PROYECCION_VENTA(cod_unidad_medida, nombre_articulo, " +
                "cod_proyecto) VALUES(?,?,?); SELECT last_insert_rowid();";

            string insert2 = "INSERT INTO DETALLE_PROYECCION_VENTA(cod_proyeccion, mes_proyeccion, " +
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
                        command2.Parameters.AddWithValue("cod_proyeccion", proyeccion.CodArticulo);
                        command2.Parameters.AddWithValue("mes_proyeccion", detTemp.Mes);
                        command2.Parameters.AddWithValue("cantidad_proyeccion", detTemp.Cantidad);
                        command2.Parameters.AddWithValue("precio_proyeccion", detTemp.Precio);
                        newProdID2 = command2.ExecuteScalar();
                        detTemp.CodDetalle = int.Parse(newProdID2.ToString());
                    }

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    proyeccion = new ProyeccionVenta();
                    transaction.Rollback();
                }
            }

            return proyeccion;
        }

        public bool EditarProyeccionVenta(ProyeccionVenta proyeccion)
        {
            int result = -1;

            string update1 = "UPDATE PROYECCION_VENTA SET cod_unidad_medida=?, nombre_articulo=? " +
                "WHERE cod_proyeccion=?";

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
                    command.Parameters.AddWithValue("cod_proyeccion", proyeccion.CodArticulo);

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
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        public bool EliminarProyeccionVenta(int codProyeccion)
        {
            int result = -1;
            string sqlQuery = "DELETE FROM PROYECCION_VENTA, DETALLE_PROYECCION_VENTA WHERE cod_proyeccion=?";
            
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();

                    SQLiteCommand command = new SQLiteCommand(sqlQuery, conn);
                    command.Parameters.AddWithValue("cod_proyeccion", codProyeccion);

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

        private List<DetalleProyeccionVenta> GetDetallesProyeccionVenta(int codProyeccion)
        {
            List<DetalleProyeccionVenta> listaDetalles = new List<DetalleProyeccionVenta>();
            string select = "SELECT cod_detalle, mes_proyeccion, cantidad_proyeccion, precio_proyeccion " +
               "FROM DETALLE_PROYECCION_VENTA WHERE cod_proyeccion=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyeccion", codProyeccion);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetalleProyeccionVenta detalleProyeccion = new DetalleProyeccionVenta();
                            detalleProyeccion.CodDetalle = reader.GetInt32(0);
                            detalleProyeccion.Mes = reader.GetString(1);
                            detalleProyeccion.Cantidad = reader.GetDouble(2);
                            detalleProyeccion.Precio = reader.GetDouble(3);
                            listaDetalles.Add(detalleProyeccion);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    listaDetalles = new List<DetalleProyeccionVenta>();
                }
            }

            return listaDetalles;
        }
    }
}