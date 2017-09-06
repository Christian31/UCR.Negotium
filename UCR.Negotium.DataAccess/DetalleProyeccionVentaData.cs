using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class DetalleProyeccionVentaData:BaseData
    {
        public DetalleProyeccionVentaData() { }

        public List<DetalleProyeccionVenta> GetDetallesProyeccionVenta(int codProyeccion)
        {
            List<DetalleProyeccionVenta> listaDetalles = new List<DetalleProyeccionVenta>();
            string select = "SELECT cod_detalle, mes_proyeccion, cantidad_proyeccion, precio_proyeccion " +
               "FROM DETALLE_PROYECCION_VENTA WHERE cod_proyeccion_venta_articulo=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyeccion_venta_articulo", codProyeccion);

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
                catch
                {
                    listaDetalles = new List<DetalleProyeccionVenta>();
                }
            }

            return listaDetalles;
        } 

        public DetalleProyeccionVenta InsertarDetalleProyeccionVenta(DetalleProyeccionVenta detalleProyeccion, int codProyeccion)
        {
            object newProdID;
            string insert = "INSERT INTO DETALLE_PROYECCION_VENTA(cod_proyeccion_venta_articulo, mes_proyeccion, " +
                " cantidad_proyeccion, precio_proyeccion) VALUES(?,?,?,?); " +
            "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("cod_proyeccion_venta_articulo", codProyeccion);
                    cmd.Parameters.AddWithValue("mes_proyeccion", detalleProyeccion.Mes);
                    cmd.Parameters.AddWithValue("cantidad_proyeccion", detalleProyeccion.Cantidad);
                    cmd.Parameters.AddWithValue("precio_proyeccion", detalleProyeccion.Precio);

                    newProdID = cmd.ExecuteScalar();
                    detalleProyeccion.CodDetalle = int.Parse(newProdID.ToString());
                }
                catch
                {
                    detalleProyeccion = new DetalleProyeccionVenta();
                }
            }

            return detalleProyeccion;
        }

        public bool EditarDetalleProyeccionVenta(DetalleProyeccionVenta detalleProyeccion, int codProyeccion)
        {
            int result = -1;
            string insert = "UPDATE DETALLE_PROYECCION_VENTA SET (cod_proyeccion_venta_articulo = ?, mes_proyeccion = ?, " +
                "cantidad_proyeccion = ?, precio_proyeccion = ? WHERE cod_detalle = ?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("cod_proyeccion_venta_articulo", codProyeccion);
                    cmd.Parameters.AddWithValue("mes_proyeccion", detalleProyeccion.Mes);
                    cmd.Parameters.AddWithValue("cantidad_proyeccion", detalleProyeccion.Cantidad);
                    cmd.Parameters.AddWithValue("precio_proyeccion", detalleProyeccion.Precio);
                    cmd.Parameters.AddWithValue("cod_detalle", detalleProyeccion.CodDetalle);

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
