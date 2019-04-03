using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Base.Trace;
using System.Linq;

namespace UCR.Negotium.DataAccess
{
    public class CostoData:BaseData
    {
        private UnidadMedidaData unidadMedidaData;

        public CostoData()
        {
            unidadMedidaData = new UnidadMedidaData();
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
                            costo.CategoriaCosto = reader.GetString(4);
                            costo.AnoCosto = reader.GetInt32(5);
                            costo.CostosMensuales = GetCostosMensuales(costo.CodCosto);
                            costo.VariacionCostos = GetVariacionesAnual(costo.CodCosto);
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
                            costo.CategoriaCosto = reader.GetString(4);
                            costo.AnoCosto = reader.GetInt32(5);
                            costo.CostosMensuales = GetCostosMensuales(costo.CodCosto);
                            costo.VariacionCostos = GetVariacionesAnual(costo.CodCosto);
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

        public Costo InsertarCosto(Costo costo, int codProyecto)
        {
            object newProdID, newProdID2;

            string insert1 = "INSERT INTO COSTO(nombre_costo, unidad_medida, cod_proyecto, " + 
                "categoria_costo, ano_inicial) VALUES(?,?,?,?,?); " +
                "SELECT last_insert_rowid();";

            string insert2 = "INSERT INTO COSTO_MENSUAL(mes, costo_unitario, cantidad, cod_costo) " + 
                "VALUES(?,?,?,?); SELECT last_insert_rowid();";

            string insert3 = "INSERT INTO VARIACION_ANUAL_COSTO(cod_costo, ano_variacion, " +
                "porcentaje_variacion, cod_tipo_variacion) VALUES(?,?,?,?); " +
                "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(insert1, conn);
                    SQLiteCommand command2 = null;

                    command.Parameters.AddWithValue("nombre_costo", costo.NombreCosto);
                    command.Parameters.AddWithValue("unidad_medida", costo.UnidadMedida.CodUnidad);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    command.Parameters.AddWithValue("categoria_costo", costo.CategoriaCosto);
                    command.Parameters.AddWithValue("ano_inicial", costo.AnoCosto);

                    transaction = conn.BeginTransaction();

                    newProdID = command.ExecuteScalar();
                    costo.CodCosto = int.Parse(newProdID.ToString());
                    foreach (CostoMensual detTemp in costo.CostosMensuales)
                    {
                        command2 = new SQLiteCommand(insert2, conn);
                        command2.Parameters.AddWithValue("mes", detTemp.Mes);
                        command2.Parameters.AddWithValue("costo_unitario", detTemp.CostoUnitario);
                        command2.Parameters.AddWithValue("cantidad", detTemp.Cantidad);
                        command2.Parameters.AddWithValue("cod_costo", costo.CodCosto);
                        newProdID2 = command2.ExecuteScalar();
                        detTemp.CodCostoMensual = int.Parse(newProdID2.ToString());
                    }

                    foreach (VariacionAnualCostoPorTipo variacionPorTipo in costo.VariacionCostos)
                    {
                        foreach (VariacionAnualCosto variacionTemp in variacionPorTipo.VariacionAnual)
                        {
                            command2 = new SQLiteCommand(insert3, conn);
                            command2.Parameters.AddWithValue("cod_costo", costo.CodCosto);
                            command2.Parameters.AddWithValue("ano_variacion", variacionTemp.Ano);
                            command2.Parameters.AddWithValue("porcentaje_variacion", variacionTemp.PorcentajeIncremento);
                            command2.Parameters.AddWithValue("cod_tipo_variacion", (int)variacionPorTipo.TipoVariacion);

                            newProdID = command2.ExecuteScalar();
                            variacionTemp.CodVariacionCosto = int.Parse(newProdID.ToString());
                        }
                    }

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    costo = new Costo();
                    transaction.Rollback();
                }
            }
            
            return costo;
        }

        public bool EditarCosto(Costo costoEditar)
        {
            int result = -1;
            object newProdID;

            string update1 = "UPDATE COSTO SET nombre_costo=?, unidad_medida=?, " +
                "categoria_costo=?, ano_inicial=? WHERE cod_costo=?";

            string update2 = "UPDATE COSTO_MENSUAL SET costo_unitario = ?, cantidad = ? " +
                "WHERE cod_costo_mensual = ?";

            string update3 = "DELETE FROM VARIACION_ANUAL_COSTO WHERE cod_costo = ?;";

            string update4 = "INSERT INTO VARIACION_ANUAL_COSTO(cod_costo, ano_variacion, " +
                "porcentaje_variacion, cod_tipo_variacion) VALUES(?,?,?,?); " +
                "SELECT last_insert_rowid();";

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
                            if (result == -1)
                                break;
                        }

                        if (result != -1)
                        {
                            command2 = new SQLiteCommand(update3, conn);
                            command2.Parameters.AddWithValue("cod_costo", costoEditar.CodCosto);
                            result = command2.ExecuteNonQuery();

                            if (result != -1)
                            {
                                foreach (VariacionAnualCostoPorTipo variacionPorTipo in costoEditar.VariacionCostos)
                                {
                                    foreach (VariacionAnualCosto variacionTemp in variacionPorTipo.VariacionAnual)
                                    {
                                        command2 = new SQLiteCommand(update4, conn);
                                        command2.Parameters.AddWithValue("cod_costo", costoEditar.CodCosto);
                                        command2.Parameters.AddWithValue("ano_variacion", variacionTemp.Ano);
                                        command2.Parameters.AddWithValue("porcentaje_variacion", variacionTemp.PorcentajeIncremento);
                                        command2.Parameters.AddWithValue("cod_tipo_variacion", (int)variacionPorTipo.TipoVariacion);

                                        newProdID = command2.ExecuteScalar();
                                        variacionTemp.CodVariacionCosto = int.Parse(newProdID.ToString());
                                    }
                                }
                            }
                        }
                    }

                    if (result != -1)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
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

            string sqlQuery1 = string.Format("BEGIN TRANSACTION; DELETE FROM VARIACION_ANUAL_COSTO WHERE cod_costo={0};" +
                "DELETE FROM COSTO_MENSUAL WHERE cod_costo={0};" +
                "DELETE FROM COSTO WHERE cod_costo={0}; COMMIT;", codCosto);

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(sqlQuery1, conn);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        private List<CostoMensual> GetCostosMensuales(int codCosto)
        {
            List<CostoMensual> listaCostos = new List<CostoMensual>();
            string select = "SELECT * FROM COSTO_MENSUAL WHERE cod_costo=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_costo", codCosto);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CostoMensual detallecosto = new CostoMensual();
                            detallecosto.CodCostoMensual = reader.GetInt32(0);
                            detallecosto.Mes = reader.GetString(1);
                            detallecosto.CostoUnitario = reader.GetDouble(2);
                            detallecosto.Cantidad = reader.GetDouble(3);
                            listaCostos.Add(detallecosto);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    listaCostos = new List<CostoMensual>();
                }
            }

            return listaCostos;
        }

        private List<VariacionAnualCostoPorTipo> GetVariacionesAnual(int codCosto)
        {
            List<VariacionAnualCostoPorTipo> variacionesPorTipo = new List<VariacionAnualCostoPorTipo>();
            int codTipoVariacion = 0;
            VariacionAnualCosto variacionAnual;

            string select = "SELECT cod_variacion_anual, ano_variacion, porcentaje_variacion, cod_tipo_variacion " +
                "FROM VARIACION_ANUAL_COSTO WHERE cod_costo=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_costo", codCosto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            variacionAnual = new VariacionAnualCosto();
                            variacionAnual.CodVariacionCosto = reader.GetInt32(0);
                            variacionAnual.Ano = reader.GetInt32(1);
                            variacionAnual.PorcentajeIncremento = reader.GetDouble(2);
                            codTipoVariacion = reader.GetInt32(3);
                            AgrupeVariaciones(variacionesPorTipo, variacionAnual, codTipoVariacion);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    variacionesPorTipo = new List<VariacionAnualCostoPorTipo>();
                }
            }

            return variacionesPorTipo;
        }

        private void AgrupeVariaciones(List<VariacionAnualCostoPorTipo> variacionesPorTipo, VariacionAnualCosto variacionAnual, int codTipoVariacion)
        {
            TipoAplicacionPorcentaje tipoVariacion = (TipoAplicacionPorcentaje)codTipoVariacion;
            VariacionAnualCostoPorTipo variacionActual = variacionesPorTipo.FirstOrDefault(variacion => variacion.TipoVariacion == tipoVariacion);
            if (variacionActual == null)
            {
                VariacionAnualCostoPorTipo variacionNueva = new VariacionAnualCostoPorTipo();
                variacionNueva.TipoVariacion = tipoVariacion;
                variacionNueva.VariacionAnual.Add(variacionAnual);
                variacionesPorTipo.Add(variacionNueva);
            }
            else
            {
                variacionActual.VariacionAnual.Add(variacionAnual);
            }
        }
    }
}
