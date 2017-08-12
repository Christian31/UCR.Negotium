using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class DetalleProyeccionVentaData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public DetalleProyeccionVentaData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public List<DetalleProyeccionVenta> GetDetallesProyeccionVenta(int codProyeccion)
        {
            List<DetalleProyeccionVenta> listaDetalles = new List<DetalleProyeccionVenta>();
            try
            {
                String select = "SELECT cod_detalle, mes_proyeccion, cantidad_proyeccion, precio_proyeccion " +
                "FROM DETALLE_PROYECCION_VENTA WHERE cod_proyeccion_venta_articulo=" + codProyeccion + ";";

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DetalleProyeccionVenta detalleProyeccion = new DetalleProyeccionVenta();
                    detalleProyeccion.CodDetalle = reader.GetInt32(0);
                    detalleProyeccion.Mes = reader.GetString(1);
                    detalleProyeccion.Cantidad = reader.GetDouble(2);
                    detalleProyeccion.Precio = reader.GetDouble(3);
                    listaDetalles.Add(detalleProyeccion);
                }//while
                conexion.Close();
                return listaDetalles;
            }//try
            catch
            {
                conexion.Close();
                return listaDetalles;
            }//catch
        }//GetDetallesProyeccionVenta 

        public DetalleProyeccionVenta InsertarDetalleProyeccionVenta(DetalleProyeccionVenta detalleProyeccion, int codProyeccion)
        {
            Object newProdID;
            String insert = "INSERT INTO DETALLE_PROYECCION_VENTA(cod_proyeccion_venta_articulo, mes_proyeccion, " +
                " cantidad_proyeccion, precio_proyeccion) VALUES(?,?,?,?); " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("cod_proyeccion_venta_articulo", codProyeccion);
            command.Parameters.AddWithValue("mes_proyeccion", detalleProyeccion.Mes);
            command.Parameters.AddWithValue("cantidad_proyeccion", detalleProyeccion.Cantidad);
            command.Parameters.AddWithValue("precio_proyeccion", detalleProyeccion.Precio);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                detalleProyeccion.CodDetalle = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return detalleProyeccion;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return detalleProyeccion;
            }//catch
        }//InsertarDetalleProyeccionVenta

        public bool EditarCrecimientoOfertaObjetoIntereses(DetalleProyeccionVenta detalleProyeccion, int codProyeccion)
        {
            String insert = "UPDATE DETALLE_PROYECCION_VENTA SET (cod_proyeccion_venta_articulo = ?, mes_proyeccion = ?, " +
                "cantidad_proyeccion = ?, precio_proyeccion = ? WHERE cod_detalle = ?;" +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("cod_proyeccion_venta_articulo", codProyeccion);
            command.Parameters.AddWithValue("mes_proyeccion", detalleProyeccion.Mes);
            command.Parameters.AddWithValue("cantidad_proyeccion", detalleProyeccion.Cantidad);
            command.Parameters.AddWithValue("precio_proyeccion", detalleProyeccion.Precio);
            command.Parameters.AddWithValue("cod_detalle", detalleProyeccion.CodDetalle);
            // Ejecutamos la sentencia UPDATE y cerramos la conexión
            if (command.ExecuteNonQuery() != -1)
            {
                conexion.Close();
                return true;
            }//if
            else
            {
                conexion.Close();
                return false;
            }//else
        }//EditarCrecimientoOfertaObjetoIntereses
    }
}
