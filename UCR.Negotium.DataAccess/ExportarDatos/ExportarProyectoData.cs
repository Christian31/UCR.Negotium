using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using UCR.Negotium.Base.Trace;

namespace UCR.Negotium.DataAccess
{
    public class ExportarProyectoData: BaseData
    {
        protected string genericCadenaConexion;

        public ExportarProyectoData()
        {
            genericCadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["genericdb"].
                ConnectionString;
        }

        protected DataSet GetDsSampleQuery(string cadenaConexion, string query)
        {
            DataSet dsResult = new DataSet();
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter();
                    adapter.SelectCommand = new SQLiteCommand(query, conn);
                    adapter.Fill(dsResult);
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    throw;
                }
            }

            return dsResult;
        }

        protected Dictionary<Guid, int> GetIndicesProyecto(string dbName)
        {
            Dictionary<Guid, int> listaIndices = new Dictionary<Guid, int>();
            string cadenaConexion = genericCadenaConexion.Replace("{AppDir}",
                AppDomain.CurrentDomain.BaseDirectory).Replace("{DatabasePath}", dbName);

            string select = "SELECT * FROM PROYECTO_INDICE";
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaIndices.Add(
                                new Guid(reader[1].ToString()),
                                int.Parse(reader[0].ToString())
                            );
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    throw;
                }
            }

            return listaIndices;
        }

        protected DataRowCollection ExeSampleQuery(string cadenaConexion, string query)
        {
            DataTable dtResult = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        dtResult.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    throw;
                }
            }

            return dtResult.Rows;
        }

        protected int ExeScalarQuery(string cadenaConexion, string insertFormat, List<Tuple<string, bool>> structure, DataRow row, bool returnValue = false)
        {
            int codInsert = -1;
            if (returnValue)
                insertFormat += " SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insertFormat, conn);

                    foreach (var cell in structure.Where(col => !col.Item2).Select(col => col.Item1))
                    {
                        cmd.Parameters.AddWithValue(cell, row[cell]);
                    }

                    if (returnValue)
                    {
                        codInsert = int.Parse(cmd.ExecuteScalar().ToString());
                    }  
                    else
                    {
                        cmd.ExecuteScalar();
                        codInsert = 0;
                    }
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    throw;
                }
            }

            if (returnValue && codInsert.Equals(-1))
                throw new Exception();

            return codInsert;
        }

        protected List<Tuple<string, bool>> GetTableColumns(string cadenaConexion, string tableName)
        {
            string select = string.Format("PRAGMA table_info({0})", tableName);
            List<Tuple<string, bool>> columns = new List<Tuple<string, bool>>();
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    var reader = cmd.ExecuteReader();
                    int nameIndex = reader.GetOrdinal("Name");
                    int pkIndex = reader.GetOrdinal("pk");

                    while (reader.Read())
                    {
                        columns.Add(Tuple.Create(reader.GetString(nameIndex),
                            reader.GetBoolean(pkIndex)));
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    throw;
                }
            }

            return columns;
        }

        protected bool ExeNonQuery(string cadenaConexion, string query)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);

                    result = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }
    }
}
