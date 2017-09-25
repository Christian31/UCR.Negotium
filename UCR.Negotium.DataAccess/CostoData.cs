using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class CostoData:BaseData
    {
        private UnidadMedidaData unidadMedidaData;
        private CostoMensualData costoMensualData;

        public CostoData()
        {
            unidadMedidaData = new UnidadMedidaData();
            costoMensualData = new CostoMensualData();
        }

        public List<Costo> GetCostos(int codProyecto)
        {
            List<Costo> listaCostos = new List<Costo>();
            string sqlQuery = "SELECT * FROM COSTO WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Costo costo = new Costo();
                            costo.CodCosto = reader.GetInt32(0);
                            costo.NombreCosto = reader.GetString(1);
                            costo.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(2));
                            costo.CostosMensuales = costoMensualData.GetCostosMensuales(reader.GetInt32(0));
                            costo.CategoriaCosto = reader.GetString(4);
                            costo.AnoCosto = reader.GetInt32(5);

                            listaCostos.Add(costo);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    listaCostos = new List<Costo>();
                }
            }

            return listaCostos;
        } 

        public Costo GetCosto(int codCosto)
        {
            Costo costo = new Costo();
            string sqlQuery = "SELECT * FROM COSTO WHERE cod_costo=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("cod_costo", codCosto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            costo.CodCosto = reader.GetInt32(0);
                            costo.NombreCosto = reader.GetString(1);
                            costo.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(2));
                            costo.CostosMensuales = costoMensualData.GetCostosMensuales(reader.GetInt32(0));
                            costo.CategoriaCosto = reader.GetString(4);
                            costo.AnoCosto = reader.GetInt32(5);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    costo = new Costo();
                }
            }

            return costo;
        }

        public Costo InsertarCosto(Costo costoNuevo, int codProyecto)
        {
            object newProdID, newProdID2;

            string insert1 = "INSERT INTO COSTO(nombre_costo, unidad_medida, cod_proyecto, " + 
                "categoria_costo, ano_inicial) VALUES(?,?,?,?,?); " +
                "SELECT last_insert_rowid();";

            string insert2 = "INSERT INTO COSTO_MENSUAL(mes, costo_unitario, cantidad, cod_costo) " + 
                "VALUES(?,?,?,?); SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(insert1, conn);
                    SQLiteCommand command2 = null;

                    command.Parameters.AddWithValue("nombre_costo", costoNuevo.NombreCosto);
                    command.Parameters.AddWithValue("unidad_medida", costoNuevo.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    command.Parameters.AddWithValue("categoria_costo", costoNuevo.CategoriaCosto);
                    command.Parameters.AddWithValue("ano_inicial", costoNuevo.AnoCosto);

                    transaction = conn.BeginTransaction();

                    newProdID = command.ExecuteScalar();
                    costoNuevo.CodCosto = int.Parse(newProdID.ToString());
                    foreach (CostoMensual detTemp in costoNuevo.CostosMensuales)
                    {
                        command2 = new SQLiteCommand(insert2, conn); ;
                        command2.Parameters.AddWithValue("mes", detTemp.Mes);
                        command2.Parameters.AddWithValue("costo_unitario", detTemp.CostoUnitario);
                        command2.Parameters.AddWithValue("cantidad", detTemp.Cantidad);
                        command2.Parameters.AddWithValue("cod_costo", costoNuevo.CodCosto);
                        newProdID2 = command2.ExecuteScalar();
                        detTemp.CodCostoMensual = int.Parse(newProdID2.ToString());
                    }

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    costoNuevo = new Costo();
                    transaction.Rollback();

                }
            }
            
            return costoNuevo;
        }

        public bool EditarCosto(Costo costoEditar)
        {
            int result = -1;

            string update1 = "UPDATE COSTO SET nombre_costo=?, unidad_medida=?, " +
                "categoria_costo=?, ano_inicial=? WHERE cod_costo=?";

            string update2 = "UPDATE COSTO_MENSUAL SET costo_unitario = ?, cantidad = ? " +
                "WHERE cod_costo_mensual = ?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update1, conn);
                    SQLiteCommand command2 = null;

                    command.Parameters.AddWithValue("nombre_costo", costoEditar.NombreCosto);
                    command.Parameters.AddWithValue("unidad_medida", costoEditar.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("cod_costo", costoEditar.CodCosto);
                    command.Parameters.AddWithValue("categoria_costo", costoEditar.CategoriaCosto);
                    command.Parameters.AddWithValue("ano_inicial", costoEditar.AnoCosto);

                    transaction = conn.BeginTransaction();

                    result = command.ExecuteNonQuery();
                    if (result != -1)
                    {
                        foreach (CostoMensual detTemp in costoEditar.CostosMensuales)
                        {
                            command2 = new SQLiteCommand(update2, conn);
                            command2.Parameters.AddWithValue("costo_unitario", detTemp.CostoUnitario);
                            command2.Parameters.AddWithValue("cantidad", detTemp.Cantidad);
                            command2.Parameters.AddWithValue("cod_costo_mensual", detTemp.CodCostoMensual);
                            result = command2.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                    transaction.Rollback();
                }
            }

            return result != -1;
        }

        public bool EliminarCosto(int codCosto)
        {
            int result = -1;

            string sqlQuery1 = "DELETE FROM COSTO_MENSUAL WHERE cod_costo=?";
            string sqlQuery2 = "DELETE FROM COSTO WHERE cod_costo=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(sqlQuery1, conn);
                    SQLiteCommand command2 = new SQLiteCommand(sqlQuery2, conn);
                    command.Parameters.AddWithValue("cod_costo", codCosto);
                    command2.Parameters.AddWithValue("cod_costo", codCosto);

                    transaction = conn.BeginTransaction();

                    result = command.ExecuteNonQuery();
                    if(result != -1)
                    {
                        result = command2.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                    transaction.Rollback();
                }
            }

            return result != -1;
        }
    }
}
