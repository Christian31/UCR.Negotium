using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class DetalleProyeccionVentaData:BaseData
    {
        public DetalleProyeccionVentaData() { }

        public List<DetalleProyeccionVenta> GetDetallesProyeccionVenta(int codProyeccion)
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
