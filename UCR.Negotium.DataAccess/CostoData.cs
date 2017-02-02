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
    public class CostoData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private UnidadMedidaData unidadMedidaData;
        private CostoMensualData costoMensualData;

        public CostoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
            unidadMedidaData = new UnidadMedidaData();
            costoMensualData = new CostoMensualData();
        }

        public List<Costo> GetCostos(int codProyecto)
        {
            SQLiteCommand command = conexion.CreateCommand();
            List<Costo> listaCostos = new List<Costo>();
            try
            {
                String select = "SELECT * FROM COSTO WHERE cod_proyecto=" + codProyecto + ";";

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Costo costo = new Costo();
                    costo.CodCosto = reader.GetInt32(0);
                    costo.NombreCosto = reader.GetString(1);
                    costo.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(2));
                    costo.CostosMensuales = costoMensualData.GetCostosMensuales(reader.GetInt32(0));
                    costo.CostoVariable = reader.GetBoolean(4);
                    costo.CategoriaCosto = reader.GetString(5);
                    costo.AnoCosto = reader.GetInt32(6);

                    listaCostos.Add(costo);
                }//while
                conexion.Close();
                return listaCostos;
            }//try
            catch
            {
                conexion.Close();
                return listaCostos;
            }//catch
        }//GetCostos 

        public Costo InsertarCosto(Costo costoNuevo, int codProyecto)
        {
            SQLiteTransaction transaction = null;
            //transaction = conexion.BeginTransaction();
            SQLiteCommand command = conexion.CreateCommand();
            SQLiteCommand command2 = conexion.CreateCommand();
            try
            {
                Object newProdID;
                Object newProdID2;
                String insert1 = "INSERT INTO COSTO(nombre_costo, " +
                "unidad_medida, cod_proyecto, costo_variable, categoria_costo, ano_inicial) VALUES(?,?,?,?,?,?); " +
            "SELECT last_insert_rowid();";

                String insert2 = "INSERT INTO COSTO_MENSUAL(mes, costo_unitario, " +
                " cantidad, cod_costo) VALUES(?,?,?,?); " +
            "SELECT last_insert_rowid();";

                command.CommandText = insert1;
                command.Parameters.AddWithValue("nombre_costo", costoNuevo.NombreCosto);
                command.Parameters.AddWithValue("unidad_medida", costoNuevo.UnidadMedida.CodUnidad);
                command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                command.Parameters.AddWithValue("costo_variable", costoNuevo.CostoVariable);
                command.Parameters.AddWithValue("categoria_costo", costoNuevo.CategoriaCosto);
                command.Parameters.AddWithValue("ano_inicial", costoNuevo.AnoCosto);

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                transaction = conexion.BeginTransaction();

                newProdID = command.ExecuteScalar();
                costoNuevo.CodCosto = Int32.Parse(newProdID.ToString());
                //transaccion
                foreach (CostoMensual detTemp in costoNuevo.CostosMensuales)
                {
                    command2.CommandText = insert2;
                    command2.Parameters.AddWithValue("mes", detTemp.Mes);
                    command2.Parameters.AddWithValue("costo_unitario", detTemp.CostoUnitario);
                    command2.Parameters.AddWithValue("cantidad", detTemp.Cantidad);
                    command2.Parameters.AddWithValue("cod_costo", costoNuevo.CodCosto);
                    newProdID2 = command2.ExecuteScalar();
                    detTemp.CodCostoMensual = Int32.Parse(newProdID2.ToString());
                }

                transaction.Commit();
                conexion.Close();
                return costoNuevo;
            }//try
            catch (SQLiteException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);

                try
                {
                    transaction.Rollback();
                }
                catch (SQLiteException ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    transaction.Dispose();
                }
            }
            finally
            {
                if (command != null || command2 != null)
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

                    }
                    catch (SQLiteException ex)
                    {

                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());

                    }
                    finally
                    {
                        conexion.Dispose();
                    }
                }
            }//catch
            return costoNuevo;
        }//InsertarCosto

        public bool EditarCosto(Costo costoEditar, int codProyecto)
        {
            SQLiteTransaction transaction = null;
            SQLiteCommand command = conexion.CreateCommand();
            SQLiteCommand command2 = conexion.CreateCommand();
            try
            {
                String insert1 = "UPDATE COSTO SET nombre_costo = ?, unidad_medida = ?, " +
                "costo_variable = ?, categoria_costo = ?, ano_inicial = ? WHERE cod_costo = ?;";

                String insert2 = "UPDATE COSTO_MENSUAL SET costo_unitario = ?, " +
                "cantidad = ? WHERE cod_costo_mensual = ?;";

                command.CommandText = insert1;
                command.Parameters.AddWithValue("nombre_costo", costoEditar.NombreCosto);
                command.Parameters.AddWithValue("unidad_medida", costoEditar.UnidadMedida.CodUnidad);
                command.Parameters.AddWithValue("cod_costo", costoEditar.CodCosto);
                command.Parameters.AddWithValue("costo_variable", costoEditar.CostoVariable);
                command.Parameters.AddWithValue("categoria_costo", costoEditar.CategoriaCosto);
                command.Parameters.AddWithValue("ano_inicial", costoEditar.AnoCosto);

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                transaction = conexion.BeginTransaction();
                // Ejecutamos la sentencia UPDATE y cerramos la conexión
                if (command.ExecuteNonQuery() != -1)
                {
                    //transaccion
                    foreach (CostoMensual detTemp in costoEditar.CostosMensuales)
                    {
                        command2.CommandText = insert2;
                        command2.Parameters.AddWithValue("costo_unitario", detTemp.CostoUnitario);
                        command2.Parameters.AddWithValue("cantidad", detTemp.Cantidad);
                        command2.Parameters.AddWithValue("cod_costo_mensual", detTemp.CodCostoMensual);
                        command2.ExecuteNonQuery();
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
        }//EditarCosto

        public bool EliminarCosto(int codCosto)
        {
            string sqlQuery1 = "DELETE FROM COSTO_MENSUAL WHERE cod_costo = " + codCosto + ";";
            string sqlQuery2 = "DELETE FROM COSTO WHERE cod_costo = " + codCosto + ";";

            SQLiteTransaction transaction = null;
            SQLiteCommand command = conexion.CreateCommand();
            SQLiteCommand command2 = conexion.CreateCommand();
            try
            {
                command.CommandText = sqlQuery1;

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                transaction = conexion.BeginTransaction();
                // Ejecutamos la sentencia UPDATE y cerramos la conexión
                if (command.ExecuteNonQuery() != -1)
                {
                    //transaccion
                    command2.CommandText = sqlQuery2;
                    command2.ExecuteNonQuery();

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
        }
    }
}
