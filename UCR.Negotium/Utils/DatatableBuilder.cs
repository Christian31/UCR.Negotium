using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Utils
{
    public static class DatatableBuilder
    {
        public static void GenerarDTCapitalTrabajo(Proyecto proyecto, out DataView dtCapitalTrabajo, out double recCTResult)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("CapitalTrabajo");
            ds.Tables["CapitalTrabajo"].Columns.Add("Rubro", Type.GetType("System.String"));
            DataRow row = ds.Tables["CapitalTrabajo"].NewRow();
            row["Rubro"] = "Costos variables";
            int a = 1;
            ds.Tables["CapitalTrabajo"].Columns.Add(proyecto.AnoInicial.ToString(), Type.GetType("System.String"));
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                ds.Tables["CapitalTrabajo"].Columns.Add((proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(proyecto.AnoInicial + a).ToString()] = "₡ " + costoGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row);

            //llena fila de capital de trabajo
            DataRow row2 = ds.Tables["CapitalTrabajo"].NewRow();
            row2["Rubro"] = "Capital de trabajo";
            int a2 = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                row2[(proyecto.AnoInicial + a2).ToString()] = "₡ " + ((costoGenerado / 12) * 1.5).ToString("#,##0.##");
                a2++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row2);

            //llena incremental

            double recCT = 0;
            double val = 0;
            DataRow row3 = ds.Tables["CapitalTrabajo"].NewRow();
            row3["Rubro"] = "Incremental";

            val = -((proyecto.CostosGenerados[0] / 12) * 1.5);
            row3[proyecto.AnoInicial.ToString()] = "₡ " + (val).ToString("#,##0.##");
            recCT += val;

            int a3 = 1;
            for (int i = 1; i < proyecto.CostosGenerados.Count; i++)
            {
                val = ((proyecto.CostosGenerados[i] / 12) * 1.5) - ((proyecto.CostosGenerados[i - 1] / 12) * 1.5);
                row3[(proyecto.AnoInicial + a3).ToString()] = "₡ " + (val).ToString("#,##0.##");
                recCT += val;
                a3++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row3);

            dtCapitalTrabajo = ds.Tables["CapitalTrabajo"].AsDataView();
            recCTResult = recCT;
        }

        public static DataTable GenerarDTCostos(Proyecto proyecto)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("Costos");
            ds.Tables["Costos"].Columns.Add("Codigo", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Articulo", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Unidad", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Categoria", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("AñoInicial", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Enero", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Febrero", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Marzo", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Abril", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Mayo", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Junio", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Julio", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Agosto", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Setiembre", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Octubre", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Noviembre", Type.GetType("System.String"));
            ds.Tables["Costos"].Columns.Add("Diciembre", Type.GetType("System.String"));

            foreach (Costo costoTemp in proyecto.Costos)
            {
                DataRow row = ds.Tables["Costos"].NewRow();
                row["Codigo"] = costoTemp.CodCosto;
                row["Articulo"] = costoTemp.NombreCosto;
                row["Unidad"] = costoTemp.UnidadMedida.NombreUnidad;
                row["Categoria"] = costoTemp.CategoriaCosto;
                row["AñoInicial"] = costoTemp.AnoCosto;
                row["Enero"] = "₡ " + costoTemp.CostosMensuales[0].Subtotal.ToString("#,##0.##");
                row["Febrero"] = "₡ " + costoTemp.CostosMensuales[1].Subtotal.ToString("#,##0.##");
                row["Marzo"] = "₡ " + costoTemp.CostosMensuales[2].Subtotal.ToString("#,##0.##");
                row["Abril"] = "₡ " + costoTemp.CostosMensuales[3].Subtotal.ToString("#,##0.##");
                row["Mayo"] = "₡ " + costoTemp.CostosMensuales[4].Subtotal.ToString("#,##0.##");
                row["Junio"] = "₡ " + costoTemp.CostosMensuales[5].Subtotal.ToString("#,##0.##");
                row["Julio"] = "₡ " + costoTemp.CostosMensuales[6].Subtotal.ToString("#,##0.##");
                row["Agosto"] = "₡ " + costoTemp.CostosMensuales[7].Subtotal.ToString("#,##0.##");
                row["Setiembre"] = "₡ " + costoTemp.CostosMensuales[8].Subtotal.ToString("#,##0.##");
                row["Octubre"] = "₡ " + costoTemp.CostosMensuales[9].Subtotal.ToString("#,##0.##");
                row["Noviembre"] = "₡ " + costoTemp.CostosMensuales[10].Subtotal.ToString("#,##0.##");
                row["Diciembre"] = "₡ " + costoTemp.CostosMensuales[11].Subtotal.ToString("#,##0.##");

                ds.Tables["Costos"].Rows.Add(row);

            }//foreach

            return ds.Tables["Costos"];
        }

        public static DataTable GeneraDTCostosGenerados(Proyecto proyecto)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("CostosGenerados");
            ds.Tables["CostosGenerados"].Columns.Add("titulo", Type.GetType("System.String"));
            DataRow row = ds.Tables["CostosGenerados"].NewRow();
            row["titulo"] = "Costos";
            int a = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                ds.Tables["CostosGenerados"].Columns.Add((proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(proyecto.AnoInicial + a).ToString()] = "₡ " + costoGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["CostosGenerados"].Rows.Add(row);

            return ds.Tables["CostosGenerados"];
        }

        public static DataTable GenerarDTDepreciaciones(Proyecto proyecto)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("Depreciaciones");

            ds.Tables["Depreciaciones"].Columns.Add("Descripcion", Type.GetType("System.String"));
            for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                ds.Tables["Depreciaciones"].Columns.Add((proyecto.AnoInicial + i).ToString(), Type.GetType("System.String"));
            }

            for (int i = 0; i < proyecto.Depreciaciones.Count; i++)
            {
                DataRow row = ds.Tables["Depreciaciones"].NewRow();
                row["Descripcion"] = proyecto.Depreciaciones[i].NombreDepreciacion;

                for (int a = 1; a <= proyecto.HorizonteEvaluacionEnAnos; a++)
                {
                    row[(proyecto.AnoInicial + a).ToString()] = "₡ " + proyecto.Depreciaciones[i].MontoDepreciacion[a - 1].ToString("#,##0.##");
                }

                ds.Tables["Depreciaciones"].Rows.Add(row);

            }//foreach

            return ds.Tables["Depreciaciones"];
        }

        public static DataTable GenerarDTTotalesDepreciaciones(Proyecto proyecto)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("DepreciacionesTotales");
            ds.Tables["DepreciacionesTotales"].Columns.Add("titulo", Type.GetType("System.String"));
            DataRow row = ds.Tables["DepreciacionesTotales"].NewRow();
            row["titulo"] = "Depreciaciones Totales";

            for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                ds.Tables["DepreciacionesTotales"].Columns.Add((proyecto.AnoInicial + i).ToString(), Type.GetType("System.String"));
                double sumaAnual = 0;
                proyecto.Depreciaciones.ForEach(dep => sumaAnual += dep.MontoDepreciacion[i-1]);
                row[(proyecto.AnoInicial + i).ToString()] = "₡ " + sumaAnual.ToString("#,##0.##");
            }
            ds.Tables["DepreciacionesTotales"].Rows.Add(row);

            return ds.Tables["DepreciacionesTotales"];
        }

        public static void GenerarDTInversiones(Proyecto proyecto, out DataTable dtInversiones, out Double totalInversiones)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("RequerimientInversion");
            ds.Tables["RequerimientInversion"].Columns.Add("codRequerimiento", Type.GetType("System.String"));
            ds.Tables["RequerimientInversion"].Columns.Add("descripcionRequerimiento", Type.GetType("System.String"));
            ds.Tables["RequerimientInversion"].Columns.Add("cantidad", Type.GetType("System.Int32"));
            ds.Tables["RequerimientInversion"].Columns.Add("costoUnitario", Type.GetType("System.Int32"));
            ds.Tables["RequerimientInversion"].Columns.Add("depreciable", Type.GetType("System.Boolean"));
            ds.Tables["RequerimientInversion"].Columns.Add("vidaUtil", Type.GetType("System.Int32"));
            ds.Tables["RequerimientInversion"].Columns.Add("Subtotal", Type.GetType("System.Int32"));
            double totalInversionesTemp = 0;
            foreach (RequerimientoInversion requerimiento in proyecto.RequerimientosInversion)
            {
                DataRow row = ds.Tables["RequerimientInversion"].NewRow();
                row["codRequerimiento"] = requerimiento.CodRequerimientoInversion;
                row["descripcionRequerimiento"] = requerimiento.DescripcionRequerimiento;
                row["cantidad"] = requerimiento.Cantidad;
                row["costoUnitario"] = requerimiento.CostoUnitario;
                row["depreciable"] = requerimiento.Depreciable;
                row["VidaUtil"] = requerimiento.VidaUtil;
                row["Subtotal"] = requerimiento.Subtotal;
                ds.Tables["RequerimientInversion"].Rows.Add(row);
                totalInversionesTemp += requerimiento.Subtotal;
            }//foreach
            totalInversiones = totalInversionesTemp;
            dtInversiones = ds.Tables["RequerimientInversion"];
        }

        public static DataTable GenerarDTProyeccionesVentas(Proyecto proyecto)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("Proyecciones");
            ds.Tables["Proyecciones"].Columns.Add("Codigo", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Articulo", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Unidad", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Enero", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Febrero", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Marzo", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Abril", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Mayo", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Junio", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Julio", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Agosto", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Setiembre", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Octubre", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Noviembre", Type.GetType("System.String"));
            ds.Tables["Proyecciones"].Columns.Add("Diciembre", Type.GetType("System.String"));
            foreach (ProyeccionVentaArticulo proyTemp in proyecto.Proyecciones)
            {
                DataRow row = ds.Tables["Proyecciones"].NewRow();
                row["Codigo"] = proyTemp.CodArticulo;
                row["Articulo"] = proyTemp.NombreArticulo;
                row["Unidad"] = proyTemp.UnidadMedida.NombreUnidad;
                row["Enero"] = ("₡ " + proyTemp.DetallesProyeccionVenta[0].Subtotal.ToString("#,##0.##"));
                row["Febrero"] = ("₡ " + proyTemp.DetallesProyeccionVenta[1].Subtotal.ToString("#,##0.##"));
                row["Marzo"] = ("₡ " + proyTemp.DetallesProyeccionVenta[2].Subtotal.ToString("#,##0.##"));
                row["Abril"] = ("₡ " + proyTemp.DetallesProyeccionVenta[3].Subtotal.ToString("#,##0.##"));
                row["Mayo"] = ("₡ " + proyTemp.DetallesProyeccionVenta[4].Subtotal.ToString("#,##0.##"));
                row["Junio"] = ("₡ " + proyTemp.DetallesProyeccionVenta[5].Subtotal.ToString("#,##0.##"));
                row["Julio"] = ("₡ " + proyTemp.DetallesProyeccionVenta[6].Subtotal.ToString("#,##0.##"));
                row["Agosto"] = ("₡ " + proyTemp.DetallesProyeccionVenta[7].Subtotal.ToString("#,##0.##"));
                row["Setiembre"] = ("₡ " + proyTemp.DetallesProyeccionVenta[8].Subtotal.ToString("#,##0.##"));
                row["Octubre"] = ("₡ " + proyTemp.DetallesProyeccionVenta[9].Subtotal.ToString("#,##0.##"));
                row["Noviembre"] = ("₡ " + proyTemp.DetallesProyeccionVenta[10].Subtotal.ToString("#,##0.##"));
                row["Diciembre"] = ("₡ " + proyTemp.DetallesProyeccionVenta[11].Subtotal.ToString("#,##0.##"));

                ds.Tables["Proyecciones"].Rows.Add(row);
            }//foreach

            return ds.Tables["Proyecciones"];
        }

        public static DataTable GenerarDTIngresosGenerados(Proyecto proyecto)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("IngresosGenerados");
            ds.Tables["IngresosGenerados"].Columns.Add("titulo", Type.GetType("System.String"));
            DataRow row = ds.Tables["IngresosGenerados"].NewRow();
            row["titulo"] = "Ingresos";

            int a = 1;
            foreach (double IngreGenerado in proyecto.IngresosGenerados)
            {
                ds.Tables["IngresosGenerados"].Columns.Add((proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(proyecto.AnoInicial + a).ToString()] = "₡ " + IngreGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["IngresosGenerados"].Rows.Add(row);

            return ds.Tables["IngresosGenerados"];
        }

        public static DataTable GenerarDTTotalesReinversiones(Proyecto proyecto)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("TotalesReinversiones");
            ds.Tables["TotalesReinversiones"].Columns.Add("titulo", Type.GetType("System.String"));

            if (proyecto != null && proyecto.RequerimientosReinversion.Count > 0)
            {
                DataRow row = ds.Tables["TotalesReinversiones"].NewRow();
                row["titulo"] = "Totales";
                List<double> listVals = new List<double>();
                for (int i = proyecto.AnoInicial + 1; i <= proyecto.AnoInicial + proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    double val = 0;
                    foreach (RequerimientoReinversion reqReinv in proyecto.RequerimientosReinversion)
                    {
                        if (reqReinv.AnoReinversion == i)
                        {
                            val += reqReinv.Subtotal;
                        }
                    }
                    listVals.Add(val);
                }

                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    int anoActual = proyecto.AnoInicial + i;
                    ds.Tables["TotalesReinversiones"].Columns.Add(anoActual.ToString(), Type.GetType("System.String"));
                    row[anoActual.ToString()] = "₡ " + listVals[i - 1].ToString("#,##0.##");
                }//for

                ds.Tables["TotalesReinversiones"].Rows.Add(row);
                return ds.Tables["TotalesReinversiones"];
            }
            return ds.Tables["TotalesReinversiones"];
        }

        public static DataTable GenerarDTFinanciamientoIF(int anoInicial, double monto, double tiempo, InteresFinanciamiento interesIF)
        {
            double interesIFtemp = interesIF.PorcentajeInteres / 100;
            double cuota = Math.Round((monto * interesIFtemp) / (1 - (Math.Pow((1 + interesIFtemp), (-tiempo)))), 2);

            DataSet ds = new DataSet();
            ds.Tables.Add("AmortizacionPrestamo");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo1");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo2");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo3");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo4");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo5");

            for (int i = 1; i <= tiempo; i++)
            {
                DataRow row1 = ds.Tables["AmortizacionPrestamo"].NewRow();
                row1["titulo1"] = anoInicial + i;

                row1["titulo2"] = "₡ " + monto.ToString("#,##0.##");

                row1["titulo3"] = "₡ " + cuota.ToString("#,##0.##");

                double interes = Math.Round((monto * interesIFtemp), 2);
                row1["titulo4"] = "₡ " + interes.ToString("#,##0.##");

                double amortizacion = Math.Round(cuota - interes, 2);
                row1["titulo5"] = "₡ " + amortizacion.ToString("#,##0.##");

                ds.Tables["AmortizacionPrestamo"].Rows.Add(row1);

                monto = Math.Round((monto - amortizacion), 2);
                if (monto < 1)
                {
                    monto = 0;
                }
            }
            return ds.Tables["AmortizacionPrestamo"];
        }

        public static DataTable GenerarDTFinanciamientoIV(Proyecto proyecto)
        {
            double monto = proyecto.Financiamiento.MontoFinanciamiento;
            int tiempo = proyecto.Financiamiento.TiempoFinanciamiento;
            List<InteresFinanciamiento> intereses = proyecto.InteresesFinanciamiento;

            DataSet ds = new DataSet();
            ds.Tables.Add("AmortizacionPrestamo");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo1", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo2", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo3", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo4", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo5", Type.GetType("System.String"));
            int tiempoTemp = tiempo;

            for (int i = 1; i <= tiempo; i++)
            {
                Double interesTemp = intereses[i - 1].PorcentajeInteres / 100;

                DataRow row1 = ds.Tables["AmortizacionPrestamo"].NewRow();
                row1["titulo1"] = proyecto.AnoInicial + i;

                row1["titulo2"] = "₡ " + monto.ToString("#,##0.##");

                double cuota = Math.Round((monto * interesTemp) / (1 - (Math.Pow((1 + interesTemp), (-tiempoTemp)))), 2);
                row1["titulo3"] = "₡ " + cuota.ToString("#,##0.##");

                double interes = Math.Round((monto * interesTemp), 2);
                row1["titulo4"] = "₡ " + interes.ToString("#,##0.##");

                double amortizacion = Math.Round(cuota - interes, 2);
                row1["titulo5"] = "₡ " + amortizacion.ToString("#,##0.##");

                ds.Tables["AmortizacionPrestamo"].Rows.Add(row1);

                monto = Math.Round((monto - amortizacion), 2);
                tiempoTemp--;
            }
            return ds.Tables["AmortizacionPrestamo"];
        }

        public static DataTable GenerarDTFlujoCaja(Proyecto proyecto, DataView dgvCapitalTrabajo, DataView dgvFinanciamiento, DataView dgvTotalesReinversiones, string totalInversiones, string recuperacionCT)
        {
            string dsNombre = "FlujoCaja";
            DataSet ds = new DataSet();
            ds.Tables.Add(dsNombre);
            ds.Tables[dsNombre].Columns.Add("Rubro", Type.GetType("System.String"));
            for (int i = 0; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                ds.Tables[dsNombre].Columns.Add((proyecto.AnoInicial + i).ToString(), Type.GetType("System.String"));
            }

            #region ventas

            DataRow row = ds.Tables[dsNombre].NewRow();
            row["Rubro"] = "Ventas";
            int a = 1;
            if (proyecto.ConIngresos)
            {
                foreach (double IngreGenerado in proyecto.IngresosGenerados)
                {
                    row[(proyecto.AnoInicial + a).ToString()] = "₡ " + IngreGenerado.ToString("#,##0.##");
                    a++;
                }
            }
            else
            {
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row[(proyecto.AnoInicial + a).ToString()] = "₡ 0";
                    a++;
                }
            }

            ds.Tables[dsNombre].Rows.Add(row);

            #endregion

            #region costos
            DataRow row2 = ds.Tables[dsNombre].NewRow();
            row2["Rubro"] = "Costos Totales";
            int a2 = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                row2[(proyecto.AnoInicial + a2).ToString()] = "₡ " + (-costoGenerado).ToString("#,##0.##");
                a2++;
            }
            ds.Tables[dsNombre].Rows.Add(row2);
            #endregion

            #region depreciaciones
            DataRow row3 = ds.Tables[dsNombre].NewRow();
            row3["Rubro"] = "Depreciaciones";
            int a3 = 1;
            foreach (double depreciacionAnual in proyecto.TotalDepreciaciones)
            {
                row3[(proyecto.AnoInicial + a3).ToString()] = "₡ -" + depreciacionAnual.ToString("#,##0.##");
                a3++;
            }
            ds.Tables[dsNombre].Rows.Add(row3);
            #endregion

            #region utilidad operativa
            DataRow row4 = ds.Tables[dsNombre].NewRow();
            row4["Rubro"] = "Utilidad Operativa";
            int a4 = 1;
            foreach (double utilidad in proyecto.UtilidadOperativa)
            {
                row4[(proyecto.AnoInicial + a4).ToString()] = "₡ " + utilidad.ToString("#,##0.##");
                a4++;
            }

            ds.Tables[dsNombre].Rows.Add(row4);
            #endregion

            #region intereses
            DataRow row5 = ds.Tables[dsNombre].NewRow();
            row5["Rubro"] = "Intereses";
            int a5 = 1;

            for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                if (i < dgvFinanciamiento.Table.Rows.Count)
                {
                    row5[(proyecto.AnoInicial + a5).ToString()] = "₡ -" + dgvFinanciamiento.Table.Rows[i][2].ToString().Replace("₡ ", string.Empty);
                }
                else
                {
                    row5[(proyecto.AnoInicial + a5).ToString()] = "₡ 0";
                }
                a5++;
            }
            ds.Tables[dsNombre].Rows.Add(row5);
            #endregion

            #region utilidad sin impuesto
            DataRow row6 = ds.Tables[dsNombre].NewRow();
            row6["Rubro"] = "Utilidad Antes de Impuesto";
            int a6 = 1;

            for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                string val = ds.Tables[dsNombre].Rows[4].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty);
                row6[(proyecto.AnoInicial + a6).ToString()] = "₡ " + Math.Round(proyecto.UtilidadOperativa[i] + Convert.ToDouble(ds.Tables[dsNombre].Rows[4].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                a6++;
            }
            ds.Tables[dsNombre].Rows.Add(row6);
            #endregion

            #region impuesto
            DataRow row7 = ds.Tables[dsNombre].NewRow();
            row7["Rubro"] = "Impuesto";
            int a7 = 1;

            for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                double val = Convert.ToDouble(ds.Tables[dsNombre].Rows[5].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty));
                double valImp = 0;
                if (proyecto.PagaImpuesto)
                {
                    valImp = proyecto.PorcentajeImpuesto;
                }

                if (proyecto.PagaImpuesto)
                {
                    row7[(proyecto.AnoInicial + a7).ToString()] = "₡ " + Math.Round(((valImp * val) / 100), 2).ToString("#,##0.##");
                }
                else
                {
                    row7[(proyecto.AnoInicial + a7).ToString()] = "₡ 0";
                }

                a7++;
            }
            ds.Tables[dsNombre].Rows.Add(row7);
            #endregion

            #region utilidad neta
            DataRow row8 = ds.Tables[dsNombre].NewRow();
            row8["Rubro"] = "Utilidad Neta";
            int a8 = 1;

            for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                row8[(proyecto.AnoInicial + a8).ToString()] = "₡ " + Math.Round(Convert.ToDouble(ds.Tables[dsNombre].Rows[5].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)) - Convert.ToDouble(ds.Tables[dsNombre].Rows[6].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                a8++;
            }
            ds.Tables[dsNombre].Rows.Add(row8);
            #endregion

            #region depreciaciones
            DataRow row9 = ds.Tables[dsNombre].NewRow();
            row9["Rubro"] = "Depreciaciones";
            int a9 = 1;
            foreach (double depreciacionAnual in proyecto.TotalDepreciaciones)
            {
                row9[(proyecto.AnoInicial + a9).ToString()] = "₡ " + depreciacionAnual.ToString("#,##0.##");
                a9++;
            }
            ds.Tables[dsNombre].Rows.Add(row9);
            #endregion

            #region flujo operativo
            DataRow row10 = ds.Tables[dsNombre].NewRow();
            row10["Rubro"] = "Flujo Operativo";
            int a10 = 1;

            for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                row10[(proyecto.AnoInicial + a10).ToString()] = "₡ " + Math.Round(Convert.ToDouble(ds.Tables[dsNombre].Rows[7].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)) + Convert.ToDouble(ds.Tables[dsNombre].Rows[8].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                a10++;
            }
            ds.Tables[dsNombre].Rows.Add(row10);
            #endregion

            #region inversiones
            DataRow row11 = ds.Tables[dsNombre].NewRow();
            row11["Rubro"] = "Inversiones";
            int a11 = 0;
            row11[(proyecto.AnoInicial + a11).ToString()] = "₡ -" + totalInversiones.ToString().Replace("₡ ", string.Empty);
            a11++;
            for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                row11[(proyecto.AnoInicial + a11).ToString()] = "₡ -" + dgvTotalesReinversiones.Table.Rows[0][i].ToString().Replace("₡ ", string.Empty);
                a11++;
            }
            ds.Tables[dsNombre].Rows.Add(row11);
            #endregion

            #region prestamo
            DataRow row12 = ds.Tables[dsNombre].NewRow();
            row12["Rubro"] = "Préstamo";
            row12[(proyecto.AnoInicial).ToString()] = "₡ " + proyecto.Financiamiento.MontoFinanciamiento;
            ds.Tables[dsNombre].Rows.Add(row12);
            #endregion

            #region amortizacion prestamo
            DataRow row13 = ds.Tables[dsNombre].NewRow();
            row13["Rubro"] = "Amortizacion del préstamo";
            for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                if (i < dgvFinanciamiento.Table.Rows.Count)
                {
                    row13[(proyecto.AnoInicial + i + 1).ToString()] = "₡ -" + dgvFinanciamiento.Table.Rows[i][3].ToString().Replace("₡ ", string.Empty);
                }
                else
                {
                    row13[(proyecto.AnoInicial + i + 1).ToString()] = "₡ 0";
                }
            }
            ds.Tables[dsNombre].Rows.Add(row13);
            #endregion

            #region inv capital trabajo
            DataRow row14 = ds.Tables[dsNombre].NewRow();
            row14["Rubro"] = "Inv. Capital Trabajo";
            int a14 = 0;
            for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos + 1; i++)
            {
                row14[(proyecto.AnoInicial + a14).ToString()] = dgvCapitalTrabajo.Table.Rows[2][i].ToString();
                a14++;
            }
            row14[(proyecto.AnoInicial + a14).ToString()] = "₡ 0";
            ds.Tables[dsNombre].Rows.Add(row14);
            #endregion

            #region recuperacionCT
            DataRow row15 = ds.Tables[dsNombre].NewRow();
            row15["Rubro"] = "Recuperación CT";
            row15[(proyecto.AnoInicial + proyecto.HorizonteEvaluacionEnAnos).ToString()] = recuperacionCT;
            ds.Tables[dsNombre].Rows.Add(row15);
            #endregion

            #region valorResidual
            DataRow row16 = ds.Tables[dsNombre].NewRow();
            row16["Rubro"] = "Valor Residual";
            row16[(proyecto.AnoInicial + proyecto.HorizonteEvaluacionEnAnos).ToString()] = "₡ " + proyecto.ValorResidual.ToString("#,##0.##");
            ds.Tables[dsNombre].Rows.Add(row16);
            #endregion

            #region flujo efectivo
            DataRow row17 = ds.Tables[dsNombre].NewRow();
            row17["Rubro"] = "Flujo Efectivo";

            for (int i = 0; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                row17[(proyecto.AnoInicial + i).ToString()] = "₡ " + Math.Round(
                    Convert.ToDouble(ds.Tables[dsNombre].Rows[9].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables[dsNombre].Rows[9].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                    Convert.ToDouble(ds.Tables[dsNombre].Rows[10].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables[dsNombre].Rows[10].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                    Convert.ToDouble(ds.Tables[dsNombre].Rows[11].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables[dsNombre].Rows[11].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                    Convert.ToDouble(ds.Tables[dsNombre].Rows[12].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables[dsNombre].Rows[12].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                    Convert.ToDouble(ds.Tables[dsNombre].Rows[13].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables[dsNombre].Rows[13].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                    Convert.ToDouble(ds.Tables[dsNombre].Rows[14].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables[dsNombre].Rows[14].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                    Convert.ToDouble(ds.Tables[dsNombre].Rows[15].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables[dsNombre].Rows[15].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString()))
                    , 2).ToString("#,##0.##");
            }
            ds.Tables[dsNombre].Rows.Add(row17);
            #endregion

            return ds.Tables[dsNombre];
        }
    }
}
