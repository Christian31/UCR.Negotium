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
    public class ProyeccionVentaArticuloData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private UnidadMedidaData unidadMedidaData;
        private DetalleProyeccionVentaData detalleProyeccionData;

        public ProyeccionVentaArticuloData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
            unidadMedidaData = new UnidadMedidaData();
            detalleProyeccionData = new DetalleProyeccionVentaData();
        }

        public List<ProyeccionVentaArticulo> GetProyeccionesVentaArticulo(int codProyecto)
        {
            SQLiteCommand command = conexion.CreateCommand();
            List<ProyeccionVentaArticulo> listaProyecciones = new List<ProyeccionVentaArticulo>();
            try
            {
                String select = "SELECT * FROM PROYECCION_VENTA_POR_ARTICULO WHERE cod_proyecto="+codProyecto+";";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
                
            command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProyeccionVentaArticulo proyeccion = new ProyeccionVentaArticulo();
                    proyeccion.CodArticulo = reader.GetInt32(0);
                    proyeccion.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(1));
                    proyeccion.NombreArticulo = reader.GetString(2);
                    proyeccion.DetallesProyeccionVenta = detalleProyeccionData.GetDetallesProyeccionVenta(reader.GetInt32(0));
                    listaProyecciones.Add(proyeccion);
                }//while
                conexion.Close();
            return listaProyecciones;
            }//try
            catch
            {
                conexion.Close();
                return listaProyecciones;
            }//catch
        }//GetProyeccionesVentaArticulo 

        public ProyeccionVentaArticulo InsertarProyeccionVenta(ProyeccionVentaArticulo proyeccion, int codProyecto)
        {
            SQLiteTransaction transaction = null;
            //transaction = conexion.BeginTransaction();
            SQLiteCommand command = conexion.CreateCommand();
            SQLiteCommand command2 = conexion.CreateCommand();
            try
            {
                Object newProdID;
                Object newProdID2;
                String insert1 = "INSERT INTO PROYECCION_VENTA_POR_ARTICULO(cod_unidad_medida, " +
                "nombre_articulo, cod_proyecto) VALUES(?,?,?); " +
            "SELECT last_insert_rowid();";

                String insert2 = "INSERT INTO DETALLE_PROYECCION_VENTA(cod_proyeccion_venta_articulo, mes_proyeccion, " +
                " cantidad_proyeccion, precio_proyeccion) VALUES(?,?,?,?); " +
            "SELECT last_insert_rowid();";
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                command.CommandText = insert1;
                command.Parameters.AddWithValue("cod_unidad_medida", proyeccion.UnidadMedida.CodUnidad);
                command.Parameters.AddWithValue("nombre_articulo", proyeccion.NombreArticulo);
                command.Parameters.AddWithValue("cod_proyecto", codProyecto);

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                transaction = conexion.BeginTransaction();

                newProdID = command.ExecuteScalar();
                proyeccion.CodArticulo = Int32.Parse(newProdID.ToString());
                //transaccion
                foreach (DetalleProyeccionVenta detTemp in proyeccion.DetallesProyeccionVenta)
                {
                    command2.CommandText = insert2;
                    command2.Parameters.AddWithValue("cod_proyeccion_venta_articulo", proyeccion.CodArticulo);
                    command2.Parameters.AddWithValue("mes_proyeccion", detTemp.Mes);
                    command2.Parameters.AddWithValue("cantidad_proyeccion", detTemp.Cantidad);
                    command2.Parameters.AddWithValue("precio_proyeccion", detTemp.Precio);
                    newProdID2 = command2.ExecuteScalar();
                    detTemp.CodDetalle = Int32.Parse(newProdID2.ToString());
                }

                transaction.Commit();
                conexion.Close();
                return proyeccion;
            }//try
            catch (SQLiteException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);

                try
                {
                    transaction.Rollback();
                } catch (SQLiteException ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    transaction.Dispose();
                }
            } finally
            {
                if (command != null || command2 !=null)
            {
                    command.Dispose();
                    command2.Dispose();
            }

            if (transaction != null) 
            {
                transaction.Dispose();
            }

            if (conexion != null)
            {
                try 
                {
                    conexion.Close();                    

                } catch (SQLiteException ex)
                {

                    Console.WriteLine("Closing connection failed.");
                    Console.WriteLine("Error: {0}",  ex.ToString());

                } finally
                {
                    conexion.Dispose();
                }
            }
            }//catch
            return proyeccion;
        }//InsertarProyeccionVenta

        public bool EditarProyeccionVenta(ProyeccionVentaArticulo proyeccion, int codProyecto)
        {
            SQLiteCommand command = conexion.CreateCommand();
            SQLiteTransaction transaction;
            transaction = conexion.BeginTransaction();
            try
            {
                String insert1 = "UPDATE PROYECCION_VENTA_POR_ARTICULO SET (cod_unidad_medida = ?, nombre_articulo = ?, " +
                "cod_proyecto = ? WHERE cod_articulo = ?; " +
            "SELECT last_insert_rowid();";

                String insert2 = "UPDATE DETALLE_PROYECCION_VENTA SET (cod_proyeccion_venta_articulo = ?, mes_proyeccion = ?, " +
                "cantidad_proyeccion = ?, precio_proyeccion = ? WHERE cod_detalle = ?;" +
            "SELECT last_insert_rowid();";
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                command.CommandText = insert1;
                command.Parameters.AddWithValue("cod_unidad_medida", proyeccion.UnidadMedida.CodUnidad);
                command.Parameters.AddWithValue("nombre_articulo", proyeccion.NombreArticulo);
                command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                command.Parameters.AddWithValue("cod_articulo", proyeccion.CodArticulo);

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                // Ejecutamos la sentencia UPDATE y cerramos la conexión
                if (command.ExecuteNonQuery() != -1)
                {
                    //transaccion
                    foreach (DetalleProyeccionVenta detTemp in proyeccion.DetallesProyeccionVenta)
                    {
                        command.CommandText = insert2;
                        command.Parameters.AddWithValue("cod_proyeccion_venta_articulo", proyeccion.CodArticulo);
                        command.Parameters.AddWithValue("mes_proyeccion", detTemp.Mes);
                        command.Parameters.AddWithValue("cantidad_proyeccion", detTemp.Cantidad);
                        command.Parameters.AddWithValue("precio_proyeccion", detTemp.Precio);
                        command.Parameters.AddWithValue("cod_detalle", detTemp.CodDetalle);
                    }
                    transaction.Commit();
                    conexion.Close();
                    return true;
                }//if
                else
                {
                    conexion.Close();
                    return false;
                }//else
                
            }//try
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);

                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                conexion.Close();
                return false;
            }//catch
        }//EditarProyeccionVenta

        public bool EliminarProyeccionVenta(int codProyeccion)
        {
            try
            {
                string sqlQuery = "DELETE FROM PROYECCION_VENTA_POR_ARTICULO WHERE cod_articulo =" + codProyeccion + ";";
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
    }
}
