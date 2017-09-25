using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;

namespace UCR.Negotium.DataAccess
{
    public class ExportarData : ExportarProyectoData
    {
        private OrderedDictionary tablesDictionary;
        private List<string> tablesToDeleteTemp;

        public ExportarData()
        {
            tablesDictionary = new OrderedDictionary();

            tablesToDeleteTemp = new List<string>() { "COSTO_MENSUAL", "DETALLE_PROYECCION_VENTA",
                "CRECIMIENTO_OFERTA_OBJETO_INTERES", "REQUERIMIENTO_INVERSION",
                "REQUERIMIENTO_REINVERSION", "COSTO", "VARIACION_ANUAL_COSTO", "PROYECCION_VENTA",
                "FINANCIAMIENTO", "INTERES_FINANCIAMIENTO", "FACTOR_AMBIENTAL"};

            //tablas principales para obtener todos los datos del proyecto
            foreach (string table in new List<string>() { "PROYECTO", "ORGANIZACION_PROPONENTE",
                "REQUERIMIENTO_INVERSION", "REQUERIMIENTO_REINVERSION", "PROYECCION_VENTA", "COSTO",
                "VARIACION_ANUAL_COSTO", "FINANCIAMIENTO", "INTERES_FINANCIAMIENTO", "FACTOR_AMBIENTAL"})
            {
                tablesDictionary.Add(table, null);
            }
        }

        #region PublicMethods
        public new Dictionary<Guid, int> GetIndicesProyecto(string dbName)
        {
            return base.GetIndicesProyecto(dbName);
        }

        public OrderedDictionary GetProyecto(string dbName, int codProyecto)
        {
            OrderedDictionary mainTables = tablesDictionary;
            DataSet ds = new DataSet();
            string cadenaConexion = genericCadenaConexion.Replace("{AppDir}",
                AppDomain.CurrentDomain.BaseDirectory).Replace("{DatabasePath}", dbName);

            ds = GetDsSampleQuery(cadenaConexion, GetMainQuery(codProyecto));

            for (int i = 0; i < mainTables.Count; i++)
            {
                if (!ds.Tables[i].Rows.Count.Equals(0))
                {
                    mainTables[i] = ds.Tables[i];
                }
            }

            return GetCompleteTablesData(mainTables, cadenaConexion);
        }

        public void EditarProyecto(string dbName, int codProyecto, OrderedDictionary proyecto)
        {
            string cadenaConexion = genericCadenaConexion.Replace("{AppDir}",
                AppDomain.CurrentDomain.BaseDirectory).Replace("{DatabasePath}", dbName);

            Dictionary<string, List<Tuple<string, bool>>> tablesStructure = new Dictionary<string, List<Tuple<string, bool>>>();
            Dictionary<string, string> queriesFormat = new Dictionary<string, string>();

            foreach (DictionaryEntry table in proyecto)
            {
                string tableName = table.Key as string;
                var tableColumns = GetTableColumns(cadenaConexion, tableName);
                tablesStructure.Add(tableName, tableColumns);

                if (tablesToDeleteTemp.Contains(tableName))
                {
                    queriesFormat.Add(tableName, CreateInsertQuery(tableName, table.Value, tableColumns));
                }
            }

            string deleteOldData = CreateDeleteQuery(codProyecto, proyecto);

            if (ExeNonQuery(cadenaConexion, deleteOldData))
            { 
                IEnumerable tablesToUpdate = proyecto.Cast<DictionaryEntry>().Where(
                    table => !tablesToDeleteTemp.Contains(table.Key));

                IEnumerable tablesToInsert = proyecto.Cast<DictionaryEntry>().Where(
                    table => tablesToDeleteTemp.Contains(table.Key));

                foreach (DictionaryEntry entry in tablesToUpdate)
                {
                    string tableName = entry.Key as string;

                    var rowEntity = entry.Value as DataRow;
                    string updateQuery = CreateEditQuery(tableName, rowEntity, tablesStructure[tableName]);
                    ExeScalarQuery(cadenaConexion, updateQuery, tablesStructure[tableName], rowEntity);
                }

                InsertarTablesData(cadenaConexion, codProyecto, tablesToInsert, queriesFormat, tablesStructure);
            }
            else
            {
                throw new Exception();
            }
        }

        public void InsertarProyecto(string dbName, OrderedDictionary proyecto, Guid uniqueCodProyecto)
        {
            string cadenaConexion = genericCadenaConexion.Replace("{AppDir}",
                AppDomain.CurrentDomain.BaseDirectory).Replace("{DatabasePath}", dbName);
            Dictionary<string, List<Tuple<string, bool>>> tablesStructure = new Dictionary<string, List<Tuple<string,bool>>>();
            Dictionary<string, string> queriesFormat = new Dictionary<string, string>();

            foreach (DictionaryEntry table in proyecto)
            {
                string tableName = table.Key as string;
                var tableColumns = GetTableColumns(cadenaConexion, tableName);
                tablesStructure.Add(tableName, tableColumns);
                queriesFormat.Add(tableName, CreateInsertQuery(tableName, table.Value, tableColumns));
            }

            IEnumerable tablesToInsertFirst = proyecto.Cast<DictionaryEntry>().Where(
                    table => !tablesToDeleteTemp.Contains(table.Key));

            IEnumerable tablesToInsert = proyecto.Cast<DictionaryEntry>().Where(
                table => tablesToDeleteTemp.Contains(table.Key));

            int codEncargado = 0;
            int codProyecto = 0;
            int codOrganizacion = 0;
            foreach (DictionaryEntry entry in tablesToInsertFirst)
            {
                string tableName = entry.Key as string;
                switch (tableName)
                {
                    case "ENCARGADO":
                        var rowEncargado = entry.Value as DataRow;
                        codEncargado = ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], rowEncargado, true);

                        break;
                    case "PROYECTO":
                        var rowProyecto = entry.Value as DataRow;
                        rowProyecto["cod_evaluador"] = codEncargado;
                        codProyecto = ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], rowProyecto, true);
                        string indiceQuery = string.Format("INSERT INTO PROYECTO_INDICE(cod_proyecto, guid_proyecto) VALUES({0},'{1}')"
                            , codProyecto, uniqueCodProyecto.ToString());
                        ExeScalarQuery(cadenaConexion, indiceQuery, new List<Tuple<string, bool>>(), null);

                        break;
                    case "ORGANIZACION_PROPONENTE":
                        var rowOrgProponente = entry.Value as DataRow;
                        rowOrgProponente["cod_proyecto"] = codProyecto;
                        codOrganizacion = ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], rowOrgProponente, true);

                        break;
                    case "PROPONENTE":
                        var rowProponente = entry.Value as DataRow;
                        rowProponente["cod_organizacion"] = codOrganizacion;
                        ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], rowProponente);

                        break;
                }   
            }

            InsertarTablesData(cadenaConexion, codProyecto, tablesToInsert, queriesFormat, tablesStructure);
        }
        #endregion

        private string GetMainQuery(int codProyecto)
        {
            string queryFormat = "SELECT * FROM {0} WHERE cod_proyecto={1}; ";
            StringBuilder query = new StringBuilder();

            foreach(DictionaryEntry dicEntry in tablesDictionary)
            {
                query.Append(string.Format(queryFormat, dicEntry.Key, codProyecto));
            }

            return query.ToString();
        }

        private OrderedDictionary GetCompleteTablesData(OrderedDictionary mainTables, string cadenaConexion)
        {
            OrderedDictionary fullTables = new OrderedDictionary();
            foreach(DictionaryEntry entry in mainTables)
            {
                if(entry.Value != null)
                {
                    DataTable dt = entry.Value as DataTable;
                    switch (entry.Key as string)
                    {
                        case "PROYECTO":
                            int codEncargado = GetDTRowValue(dt.Rows[0], "cod_evaluador");
                            if (codEncargado != 0)
                            {
                                fullTables.Add("ENCARGADO", GetEncargado(cadenaConexion, codEncargado));
                            }
                            fullTables.Add(entry.Key, dt.Rows[0]);
                            break;
                        case "ORGANIZACION_PROPONENTE":
                            fullTables.Add(entry.Key, dt.Rows[0]);
                            int codOrganizacion = GetDTRowValue(dt.Rows[0], "cod_organizacion");

                            fullTables.Add("PROPONENTE", GetProponente(cadenaConexion, codOrganizacion));
                            break;
                        case "COSTO":
                            List<int> codCostos = new List<int>();
                            fullTables.Add(entry.Key, dt.Rows);

                            foreach(DataRow row in dt.Rows)
                            {
                                codCostos.Add(GetDTRowValue(row, "cod_costo"));
                            }
                            fullTables.Add("COSTO_MENSUAL", GetCostosMensuales(cadenaConexion, codCostos));

                            break;
                        case "PROYECCION_VENTA":
                            List<int> codProyecciones = new List<int>();
                            fullTables.Add(entry.Key, dt.Rows);
                            foreach(DataRow row in dt.Rows)
                            {
                                codProyecciones.Add(GetDTRowValue(row, "cod_proyeccion"));
                            }
                            fullTables.Add("DETALLE_PROYECCION_VENTA", GetDetalleProyeccion(cadenaConexion, codProyecciones));
                            fullTables.Add("CRECIMIENTO_OFERTA_OBJETO_INTERES", GetCrecimientoOferta(cadenaConexion, codProyecciones));

                            break;
                        case "FINANCIAMIENTO":
                            fullTables.Add(entry.Key, dt.Rows[0]);
                            break;
                        default:
                            fullTables.Add(entry.Key, dt.Rows);
                            break;
                    }
                }
            }

            return fullTables;
        }

        private int GetDTRowValue(DataRow dataRow, string rowName)
        {
            int rowValue = 0;
            for(int i=0; i< dataRow.Table.Columns.Count; i++)
            {
                if (dataRow.Table.Columns[i].ColumnName.Equals(rowName))
                {
                    int.TryParse(dataRow[i].ToString(), out rowValue);
                    break;
                }
            }

            return rowValue;
        }

        private DataRow GetEncargado(string cadenaConexion, int codEncargado)
        {
            string select = string.Format("SELECT * FROM ENCARGADO WHERE cod_encargado={0}"
                , codEncargado);

            return ExeSampleQuery(cadenaConexion, select)[0];
        }

        private DataRow GetProponente(string cadenaConexion, int codOrganizacion)
        {
            string select = string.Format("SELECT * FROM PROPONENTE WHERE cod_organizacion={0}"
                , codOrganizacion);

            return GetDsSampleQuery(cadenaConexion, select).Tables[0].Rows[0];
        }

        private DataRowCollection GetCostosMensuales(string cadenaConexion, List<int> codCostos)
        {
            string select = string.Format("SELECT * FROM COSTO_MENSUAL WHERE cod_costo IN ({0})"
                , string.Join(", ", codCostos.ToArray()));

            return ExeSampleQuery(cadenaConexion, select);
        }

        private DataRowCollection GetCrecimientoOferta(string cadenaConexion, List<int> codCrecimientos)
        {
            string select = string.Format("SELECT * FROM CRECIMIENTO_OFERTA_OBJETO_INTERES WHERE cod_proyeccion IN ({0})"
                , string.Join(", ", codCrecimientos.ToArray()));

            return ExeSampleQuery(cadenaConexion, select);
        }

        private DataRowCollection GetDetalleProyeccion(string cadenaConexion, List<int> codProyecciones)
        {
            string select = string.Format("SELECT * FROM DETALLE_PROYECCION_VENTA " +
                "WHERE cod_proyeccion IN ({0})"
                , string.Join(", ", codProyecciones.ToArray()));
            
            return ExeSampleQuery(cadenaConexion, select);
        }
        
        private string CreateInsertQuery(string tableName, object tableData, List<Tuple<string, bool>> tableStructure)
        {
            DataRow rootRow = tableData is DataRow ? 
                (DataRow)tableData : ((DataRowCollection)tableData)[0];
            
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append("INSERT INTO " + tableName + "(");
            bool isFirst = true;
            int i = 0;
            foreach (string column in tableStructure.Where(col => !col.Item2).Select(col => col.Item1))
            {
                if (rootRow.Table.Columns.Contains(column))
                {
                    if (!isFirst)
                    {
                        insertQuery.Append(", ");
                    }

                    insertQuery.Append(column);
                    isFirst = false;
                    i++;
                }
            }

            insertQuery.Append(") VALUES(");
            isFirst = true;
            int a = 0;
            while(a<i)
            {
                if (!isFirst)
                {
                    insertQuery.Append(", ");
                }

                insertQuery.Append("?");
                isFirst = false;
                a++;
            }
            insertQuery.Append(");");

            return insertQuery.ToString();
        }

        private string CreateEditQuery(string tableName, DataRow rowEntity, List<Tuple<string, bool>> tableStructure)
        {
            StringBuilder editQuery = new StringBuilder();
            editQuery.Append("UPDATE " + tableName + " SET ");
            bool isFirst = true;

            foreach (string column in tableStructure.Where(col => !col.Item2).Select(col => col.Item1))
            {
                if (rowEntity.Table.Columns.Contains(column))
                {
                    if (!isFirst)
                    {
                        editQuery.Append(", ");
                    }

                    editQuery.Append(column + "=?");
                    isFirst = false;
                }
            }
            
            editQuery.Append("WHERE ");
            isFirst = true;
            foreach (string column in tableStructure.Where(col => col.Item2).Select(col => col.Item1))
            {
                if (rowEntity.Table.Columns.Contains(column))
                {
                    if (!isFirst)
                    {
                        editQuery.Append(" AND ");
                    }

                    editQuery.Append(column + "=" + rowEntity[column]);
                    isFirst = false;
                }
            }

            return editQuery.ToString();
        }

        private string CreateDeleteQuery(int codProyecto, OrderedDictionary proyecto)
        {
            StringBuilder queryBuilder = new StringBuilder();
            foreach(string table in tablesToDeleteTemp)
            {
                if (proyecto.Contains(table))
                {
                    switch (table)
                    {
                        case "COSTO_MENSUAL":
                            List<int> codsCostos = new List<int>();
                            foreach(DataRow costo in proyecto["COSTO"] as DataRowCollection)
                            {
                                codsCostos.Add(GetDTRowValue(costo, "cod_costo"));
                            }
                            queryBuilder.Insert(0, string.Format("DELETE FROM {0} WHERE cod_costo IN ({1}); ",
                                table, string.Join(", ", codsCostos.ToArray())));
                            
                            break;
                        case "DETALLE_PROYECCION_VENTA":
                        case "CRECIMIENTO_OFERTA_OBJETO_INTERES":
                            List<int> codsProyecciones = new List<int>();
                            foreach (DataRow proyeccion in proyecto["PROYECCION_VENTA"] as DataRowCollection)
                            {
                                codsProyecciones.Add(GetDTRowValue(proyeccion, "cod_proyeccion"));
                            }
                            queryBuilder.Insert(0, string.Format("DELETE FROM {0} WHERE cod_proyeccion IN ({1}); ",
                                table, string.Join(", ", codsProyecciones.ToArray())));
                            break;
                        default:
                            queryBuilder.Append(string.Format("DELETE FROM {0} WHERE cod_proyecto={1}; ",
                        table, codProyecto));
                            break;
                    }
                }
            }

            return queryBuilder.ToString();
        }

        private void InsertarTablesData(string cadenaConexion, int codProyecto, IEnumerable tablesToInsert, Dictionary<string, string> queriesFormat, Dictionary<string, List<Tuple<string, bool>>> tablesStructure)
        {
            //old -> new
            List<Tuple<int, int>> codCostosConversion = new List<Tuple<int, int>>();
            List<Tuple<int, int>> codProyeccionesConversion = new List<Tuple<int, int>>();
            foreach (DictionaryEntry entry in tablesToInsert)
            {
                string tableName = entry.Key as string;
                switch (tableName)
                {
                    case "COSTO":
                        DataRowCollection rowsCostos = entry.Value as DataRowCollection;
                        foreach (DataRow row in rowsCostos)
                        {
                            row["cod_proyecto"] = codProyecto;
                            int codCosto = ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], row, true);
                            codCostosConversion.Add(Tuple.Create(Convert.ToInt32(row["cod_costo"]), codCosto));
                        }

                        break;
                    case "COSTO_MENSUAL":
                        DataRowCollection rowsCostosMensuales = entry.Value as DataRowCollection;
                        int oldCod = 0;
                        int newCod = 0;
                        foreach (DataRow row in rowsCostosMensuales)
                        {
                            int tempCod = Convert.ToInt32(row["cod_costo"]);
                            if (!oldCod.Equals(tempCod))
                            {
                                oldCod = tempCod;
                                newCod = codCostosConversion.Find(cod => cod.Item1.Equals(oldCod)).Item2;
                            }

                            row["cod_costo"] = newCod;
                            ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], row);
                        }
                        break;
                    case "PROYECCION_VENTA":
                        DataRowCollection rowsProyecciones = entry.Value as DataRowCollection;
                        foreach (DataRow row in rowsProyecciones)
                        {
                            row["cod_proyecto"] = codProyecto;
                            int codProyeccion = ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], row, true);
                            codProyeccionesConversion.Add(Tuple.Create(Convert.ToInt32(row["cod_proyeccion"]), codProyeccion));
                        }
                        break;
                    case "DETALLE_PROYECCION_VENTA":
                    case "CRECIMIENTO_OFERTA_OBJETO_INTERES":
                        DataRowCollection rowsDetalles = entry.Value as DataRowCollection;
                        int oldCodDetalle = 0;
                        int newCodDetalle = 0;
                        foreach (DataRow row in rowsDetalles)
                        {
                            int tempCodDetalle = Convert.ToInt32(row["cod_proyeccion"]);
                            if (!oldCodDetalle.Equals(tempCodDetalle))
                            {
                                oldCodDetalle = tempCodDetalle;
                                newCodDetalle = codProyeccionesConversion.Find(cod => cod.Item1.Equals(oldCodDetalle)).Item2;
                            }

                            row["cod_proyeccion"] = newCodDetalle;
                            ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], row);
                        }

                        break;
                    case "FINANCIAMIENTO":
                        DataRow rowFinanciamiento = entry.Value as DataRow;

                        rowFinanciamiento["cod_proyecto"] = codProyecto;
                        ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], rowFinanciamiento);

                        break;
                    default:
                        DataRowCollection rowsDefault = entry.Value as DataRowCollection;
                        foreach (DataRow row in rowsDefault)
                        {
                            row["cod_proyecto"] = codProyecto;
                            ExeScalarQuery(cadenaConexion, queriesFormat[tableName], tablesStructure[tableName], row);
                        }
                        break;
                }
            }
        }
    }
}
