using System;
using System.Collections.Generic;
using System.Data;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Extensions;

namespace UCR.Negotium.Extensions
{
    public static class DatatableBuilder
    {
        public static void GenerarCapitalTrabajo(Proyecto proyecto, out DataView dtCapitalTrabajo, out double recCTResult)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);

            DataSet ds = new DataSet();
            ds.Tables.Add("CapitalTrabajo");
            ds.Tables["CapitalTrabajo"].Columns.Add("Rubro", Type.GetType("System.String"));

            //llena costos variables
            DataRow row = ds.Tables["CapitalTrabajo"].NewRow();
            row["Rubro"] = "Costos variables";
            int a = 1;
            ds.Tables["CapitalTrabajo"].Columns.Add(proyecto.AnoInicial.ToString(), Type.GetType("System.String"));
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                ds.Tables["CapitalTrabajo"].Columns.Add((proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(proyecto.AnoInicial + a).ToString()] = costoGenerado.FormatoMoneda(signoMoneda);
                a++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row);

            //llena fila de capital de trabajo
            DataRow row2 = ds.Tables["CapitalTrabajo"].NewRow();
            row2["Rubro"] = "Capital de trabajo";
            int a2 = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                double capitalTrabajo = ((costoGenerado / 12) * 1.5);
                row2[(proyecto.AnoInicial + a2).ToString()] = capitalTrabajo.FormatoMoneda(signoMoneda);
                a2++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row2);

            //llena incremental
            double recCT = 0;
            double val = 0;
            DataRow row3 = ds.Tables["CapitalTrabajo"].NewRow();
            row3["Rubro"] = "Incremental";

            val = -((proyecto.CostosGenerados[0] / 12) * 1.5);
            row3[proyecto.AnoInicial.ToString()] = val.FormatoMoneda(signoMoneda);
            recCT = val;

            int a3 = 1;
            for (int i = 1; i < proyecto.CostosGenerados.Count; i++)
            {
                val = -(((proyecto.CostosGenerados[i] / 12) * 1.5) - ((proyecto.CostosGenerados[i - 1] / 12) * 1.5));
                row3[(proyecto.AnoInicial + a3).ToString()] = val.FormatoMoneda(signoMoneda);
                recCT += val;
                a3++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row3);

            dtCapitalTrabajo = ds.Tables["CapitalTrabajo"].AsDataView();
            recCTResult = +-recCT;
        }

        public static DataTable GenerarCostosTotales(Proyecto proyecto)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);

            DataSet ds = new DataSet();
            ds.Tables.Add("CostosGenerados");
            ds.Tables["CostosGenerados"].Columns.Add("titulo", Type.GetType("System.String"));
            DataRow row = ds.Tables["CostosGenerados"].NewRow();
            row["titulo"] = "Costos";
            int a = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                ds.Tables["CostosGenerados"].Columns.Add((proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(proyecto.AnoInicial + a).ToString()] = costoGenerado.FormatoMoneda(signoMoneda);
                a++;
            }
            ds.Tables["CostosGenerados"].Rows.Add(row);

            return ds.Tables["CostosGenerados"];
        }

        public static DataTable GenerarDepreciaciones(Proyecto proyecto)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);
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
                    row[(proyecto.AnoInicial + a).ToString()] = 
                        proyecto.Depreciaciones[i].MontoDepreciacion[a - 1].FormatoMoneda(signoMoneda);
                }

                ds.Tables["Depreciaciones"].Rows.Add(row);

            }

            return ds.Tables["Depreciaciones"];
        }

        public static List<DataView> GenerarDepreciacionesPaging(Proyecto proyecto, int limit)
        {
            List<DataView> viewPaging = new List<DataView>();
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);

            int count = 1;
            while (count <= proyecto.HorizonteEvaluacionEnAnos)
            {
                int newCount = count;
                DataSet ds = new DataSet();
                ds.Tables.Add("Depreciaciones");

                ds.Tables["Depreciaciones"].Columns.Add("Descripcion", Type.GetType("System.String"));
                for (int i = count; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    ds.Tables["Depreciaciones"].Columns.Add((proyecto.AnoInicial + i).ToString(), Type.GetType("System.String"));
                    newCount = i;

                    if (i%limit == 0)
                        break;
                }

                for (int i = 0; i < proyecto.Depreciaciones.Count; i++)
                {
                    DataRow row = ds.Tables["Depreciaciones"].NewRow();
                    row["Descripcion"] = proyecto.Depreciaciones[i].NombreDepreciacion;

                    for (int a = count; a <= proyecto.HorizonteEvaluacionEnAnos; a++)
                    {
                        row[(proyecto.AnoInicial + a).ToString()] = 
                            proyecto.Depreciaciones[i].MontoDepreciacion[a - 1].FormatoMoneda(signoMoneda);

                        if (a%limit == 0)
                            break;
                    }

                    ds.Tables["Depreciaciones"].Rows.Add(row);

                }

                count = (newCount + 1);
                viewPaging.Add(ds.Tables["Depreciaciones"].AsDataView());
            }

            return viewPaging;
        }

        public static DataTable GenerarDepreciacionesTotales(Proyecto proyecto)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);
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
                row[(proyecto.AnoInicial + i).ToString()] = sumaAnual.FormatoMoneda(signoMoneda);
            }
            ds.Tables["DepreciacionesTotales"].Rows.Add(row);

            return ds.Tables["DepreciacionesTotales"];
        }

        public static DataTable GenerarIngresosTotales(Proyecto proyecto)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);

            DataSet ds = new DataSet();
            ds.Tables.Add("IngresosGenerados");
            ds.Tables["IngresosGenerados"].Columns.Add("titulo", Type.GetType("System.String"));
            DataRow row = ds.Tables["IngresosGenerados"].NewRow();
            row["titulo"] = "Ingresos";

            int a = 1;
            foreach (double ingreGenerado in proyecto.IngresosGenerados)
            {
                ds.Tables["IngresosGenerados"].Columns.Add((proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(proyecto.AnoInicial + a).ToString()] = ingreGenerado.FormatoMoneda(signoMoneda);
                a++;
            }
            ds.Tables["IngresosGenerados"].Rows.Add(row);

            return ds.Tables["IngresosGenerados"];
        }

        public static DataTable GenerarReinversionesTotales(ProyectoLite proyecto, List<Reinversion> reinversiones)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);

            DataSet ds = new DataSet();
            ds.Tables.Add("TotalesReinversiones");
            ds.Tables["TotalesReinversiones"].Columns.Add("titulo", Type.GetType("System.String"));

            if (proyecto != null && reinversiones.Count > 0)
            {
                DataRow row = ds.Tables["TotalesReinversiones"].NewRow();
                row["titulo"] = "Totales";
                List<double> listVals = new List<double>();
                for (int i = proyecto.AnoInicial + 1; i <= proyecto.AnoInicial + proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    double val = 0;
                    foreach (Reinversion reqReinv in reinversiones)
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
                    row[anoActual.ToString()] = listVals[i - 1].FormatoMoneda(signoMoneda);
                }//for

                ds.Tables["TotalesReinversiones"].Rows.Add(row);
                return ds.Tables["TotalesReinversiones"];
            }
            return ds.Tables["TotalesReinversiones"];
        }

        public static DataTable GenerarFinanciamientoIF(Financiamiento financiamiento, int codProyecto)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(codProyecto);

            int tiempo = financiamiento.TiempoFinanciamiento;
            double monto = financiamiento.MontoFinanciamiento;
            int anoInicial = financiamiento.AnoInicialPago;
            double interesIFtemp = financiamiento.TasaIntereses[0].PorcentajeInteres / 100;
            double cuota = (monto * interesIFtemp) / (1 - (Math.Pow((1 + interesIFtemp), (-tiempo))));
            double cuotaPonderada = cuota.PonderarNumero();

            DataSet ds = new DataSet();
            ds.Tables.Add("AmortizacionPrestamo");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo1");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo2");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo3");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo4");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo5");

            for (int i = 0; i < tiempo; i++)
            {
                DataRow row1 = ds.Tables["AmortizacionPrestamo"].NewRow();
                row1["titulo1"] = anoInicial + i;
                row1["titulo2"] = monto.FormatoMoneda(signoMoneda);

                row1["titulo3"] = cuotaPonderada.FormatoMoneda(signoMoneda);

                double interes = (monto * interesIFtemp).PonderarNumero();
                double interesPonderado = interes.PonderarNumero();
                row1["titulo4"] = interesPonderado.FormatoMoneda(signoMoneda);

                double amortizacion = cuotaPonderada - interesPonderado;
                double amortizacionPonderada = amortizacion.PonderarNumero();
                row1["titulo5"] = amortizacionPonderada.FormatoMoneda(signoMoneda);

                ds.Tables["AmortizacionPrestamo"].Rows.Add(row1);

                monto = (monto - amortizacion).PonderarNumero();
            }
            return ds.Tables["AmortizacionPrestamo"];
        }

        public static DataTable GenerarFinanciamientoIV(Financiamiento financiamiento, int codProyecto)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(codProyecto);

            double monto = financiamiento.MontoFinanciamiento;
            int tiempo = financiamiento.TiempoFinanciamiento;
            int anoInicial = financiamiento.AnoInicialPago;
            List<InteresFinanciamiento> intereses = financiamiento.TasaIntereses;

            DataSet ds = new DataSet();
            ds.Tables.Add("AmortizacionPrestamo");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo1", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo2", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo3", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo4", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo5", Type.GetType("System.String"));
            int tiempoTemp = tiempo;

            for (int i = 0; i < tiempo; i++)
            {
                double interesTemp = intereses[i].PorcentajeInteres / 100;

                DataRow row1 = ds.Tables["AmortizacionPrestamo"].NewRow();
                row1["titulo1"] = anoInicial + i;
                row1["titulo2"] = monto.FormatoMoneda(signoMoneda);

                double cuota = ((monto * interesTemp) / (1 - (Math.Pow((1 + interesTemp), (-tiempoTemp)))));
                double coutaPonderada = cuota.PonderarNumero();
                row1["titulo3"] = coutaPonderada.FormatoMoneda(signoMoneda);

                double interes = monto * interesTemp;
                double interesPonderado = interes.PonderarNumero();
                row1["titulo4"] = interesPonderado.FormatoMoneda(signoMoneda);

                double amortizacion = coutaPonderada - interesPonderado;
                double amortizacionPonderada = amortizacion.PonderarNumero();
                row1["titulo5"] = amortizacionPonderada.FormatoMoneda(signoMoneda);

                ds.Tables["AmortizacionPrestamo"].Rows.Add(row1);

                monto = (monto - amortizacionPonderada).PonderarNumero();
                tiempoTemp--;
            }
            return ds.Tables["AmortizacionPrestamo"];
        }

        public static DataTable GenerarFlujoCaja(Proyecto proyecto, DataView dgvCapitalTrabajo, DataView dgvFinanciamiento, DataView dgvTotalesReinversiones, string totalInversiones, string recuperacionCT)
        {
            double valorDefecto = 0;
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);

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
            foreach (double ingreGenerado in proyecto.IngresosGenerados)
            {
                row[(proyecto.AnoInicial + a).ToString()] = ingreGenerado.FormatoMoneda(signoMoneda);
                a++;
            }

            ds.Tables[dsNombre].Rows.Add(row);

            #endregion

            #region costos
            DataRow row2 = ds.Tables[dsNombre].NewRow();
            row2["Rubro"] = "Costos Totales";
            int a2 = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                row2[(proyecto.AnoInicial + a2).ToString()] = (-costoGenerado).FormatoMoneda(signoMoneda);
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
                row3[(proyecto.AnoInicial + a3).ToString()] = (-depreciacionAnual).FormatoMoneda(signoMoneda);
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
                row4[(proyecto.AnoInicial + a4).ToString()] = utilidad.FormatoMoneda(signoMoneda);
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
                if (dgvFinanciamiento.Table != null && i < dgvFinanciamiento.Table.Rows.Count)
                {
                    row5[(proyecto.AnoInicial + a5).ToString()] = dgvFinanciamiento.Table.Rows[i][3].ToString();
                }
                else
                {
                    row5[(proyecto.AnoInicial + a5).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
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
                string val = ds.Tables[dsNombre].Rows[4].ItemArray[i + 2].ToString().Replace(signoMoneda + " ", string.Empty);
                double utilidad = proyecto.UtilidadOperativa[i] + Convert.ToDouble(val);
                double utilidadPonderada = utilidad.PonderarNumero();
                row6[(proyecto.AnoInicial + a6).ToString()] = utilidadPonderada.FormatoMoneda(signoMoneda);
                a6++;
            }
            ds.Tables[dsNombre].Rows.Add(row6);
            #endregion

            #region impuesto
            DataRow row7 = ds.Tables[dsNombre].NewRow();
            row7["Rubro"] = "Impuesto";
            int a7 = 1;
            if (proyecto.PagaImpuesto)
            {
                
                for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    double val = Convert.ToDouble(ds.Tables[dsNombre].Rows[5].ItemArray[i + 2].ToString().Replace(signoMoneda + " ", string.Empty));
                    double valImp = proyecto.PorcentajeImpuesto;

                    if(val > 0)
                    {
                        double impuesto = ((valImp * val) / 100);
                        double impuestoPonderado = impuesto.PonderarNumero();
                        row7[(proyecto.AnoInicial + a7).ToString()] = impuestoPonderado.FormatoMoneda(signoMoneda);
                    }
                    else
                    {
                        row7[(proyecto.AnoInicial + a7).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
                    }
                    a7++;
                }
            }
            else
            {
                for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row7[(proyecto.AnoInicial + a7).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
                    a7++;
                }
            }

            ds.Tables[dsNombre].Rows.Add(row7);
            #endregion

            #region utilidad neta
            DataRow row8 = ds.Tables[dsNombre].NewRow();
            row8["Rubro"] = "Utilidad Neta";
            int a8 = 1;

            for (int i = 0; i < proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                double utilidadNeta = (ds.Tables[dsNombre].Rows[5].ItemArray[i + 2].ToString().FormatoNumero(signoMoneda) - 
                    ds.Tables[dsNombre].Rows[6].ItemArray[i + 2].ToString().FormatoNumero(signoMoneda));
                double utilidadNetaPonderada = utilidadNeta.PonderarNumero();
                row8[(proyecto.AnoInicial + a8).ToString()] = utilidadNetaPonderada.FormatoMoneda(signoMoneda);
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
                row9[(proyecto.AnoInicial + a9).ToString()] = depreciacionAnual.FormatoMoneda(signoMoneda);
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
                double flujoOperativo = (ds.Tables[dsNombre].Rows[7].ItemArray[i + 2].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[8].ItemArray[i + 2].ToString().FormatoNumero(signoMoneda)).PonderarNumero();
                double flujoOperativoPonderado = flujoOperativo.PonderarNumero();
                row10[(proyecto.AnoInicial + a10).ToString()] = flujoOperativoPonderado.FormatoMoneda(signoMoneda);
                a10++;
            }
            ds.Tables[dsNombre].Rows.Add(row10);
            #endregion

            #region inversiones
            DataRow row11 = ds.Tables[dsNombre].NewRow();
            row11["Rubro"] = "Inversiones";
            int a11 = 0;
            row11[(proyecto.AnoInicial + a11).ToString()] = (-(totalInversiones.ToString().FormatoNumero(signoMoneda))).FormatoMoneda(signoMoneda);
            a11++;

            if (dgvTotalesReinversiones.Table != null)
            {
                for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row11[(proyecto.AnoInicial + a11).ToString()] = (-(dgvTotalesReinversiones.Table.Rows[0][i].ToString().FormatoNumero(signoMoneda))).FormatoMoneda(signoMoneda);
                    a11++;
                }
            }
            else
            {
                for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row11[(proyecto.AnoInicial + a11).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
                    a11++;
                }
            }
            ds.Tables[dsNombre].Rows.Add(row11);
            #endregion

            #region prestamo
            DataRow row12 = ds.Tables[dsNombre].NewRow();
            row12["Rubro"] = "Préstamo";
            row12[(proyecto.AnoInicial).ToString()] = proyecto.Financiamiento.MontoFinanciamiento.FormatoMoneda(signoMoneda);
            ds.Tables[dsNombre].Rows.Add(row12);
            #endregion

            #region amortizacion prestamo
            DataRow row13 = ds.Tables[dsNombre].NewRow();
            row13["Rubro"] = "Amortizacion del préstamo";

            for (int i = 0; i < proyecto.Financiamiento.TiempoFinanciamiento; i++)
            {
                if (dgvFinanciamiento.Table != null && i < dgvFinanciamiento.Table.Rows.Count)
                {
                    row13[(proyecto.Financiamiento.AnoInicialPago + i).ToString()] = (-dgvFinanciamiento.Table.Rows[i][4].ToString().FormatoNumero(signoMoneda)).FormatoMoneda(signoMoneda);
                }
                else
                {
                    row13[(proyecto.Financiamiento.AnoInicialPago + i).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
                }
            }
            ds.Tables[dsNombre].Rows.Add(row13);
            #endregion

            #region inv capital trabajo
            DataRow row14 = ds.Tables[dsNombre].NewRow();
            row14["Rubro"] = "Inv. Capital Trabajo";
            int a14 = 0;

            if (dgvCapitalTrabajo.Table != null)
            {
                for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos + 1; i++)
                {
                    row14[(proyecto.AnoInicial + a14).ToString()] = dgvCapitalTrabajo.Table.Rows[2][i].ToString();
                    a14++;
                }
            }
            else
            {
                for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos + 1; i++)
                {
                    row14[(proyecto.AnoInicial + a14).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
                    a14++;
                }
            }

            row14[(proyecto.AnoInicial + a14).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
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
            row16[(proyecto.AnoInicial + proyecto.HorizonteEvaluacionEnAnos).ToString()] = proyecto.ValorResidual.FormatoMoneda(signoMoneda);
            ds.Tables[dsNombre].Rows.Add(row16);
            #endregion

            #region flujo efectivo
            DataRow row17 = ds.Tables[dsNombre].NewRow();
            row17["Rubro"] = "Flujo Efectivo";

            for (int i = 0; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                double flujoEfectivo = (
                    ds.Tables[dsNombre].Rows[9].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[10].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[11].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[12].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[13].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[14].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[15].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda)
                    );
                double flujoEfectivoPonderado = flujoEfectivo.PonderarNumero();
                row17[(proyecto.AnoInicial + i).ToString()] = flujoEfectivoPonderado.FormatoMoneda(signoMoneda);
            }
            ds.Tables[dsNombre].Rows.Add(row17);
            #endregion

            return ds.Tables[dsNombre];
        }

        public static List<DataView> FlujoCajaToPaging(DataView flujoCaja, int limit)
        {
            List<DataView> flujoCajaPaging = new List<DataView>();
            limit = limit + 1;
            int count = 1;
            while (count < flujoCaja.Table.Columns.Count)
            {
                int newCount = count;
                DataSet ds = new DataSet();
                ds.Tables.Add("FlujoCajaPag");
                ds.Tables["FlujoCajaPag"].Columns.Add("Rubro", Type.GetType("System.String"));

                for (int i = count; i < flujoCaja.Table.Columns.Count; i++)
                {
                    ds.Tables["FlujoCajaPag"].Columns.Add(flujoCaja.Table.Columns[i].ColumnName, Type.GetType("System.String"));
                    newCount = i;

                    if (i % limit == 0)
                        break;
                }

                for (int i = 0; i < flujoCaja.Table.Rows.Count; i++)
                {
                    DataRow row = ds.Tables["FlujoCajaPag"].NewRow();
                    row["Rubro"] = flujoCaja.Table.Rows[i][0].ToString();

                    for (int a = count; a < flujoCaja.Table.Columns.Count; a++)
                    {
                        row[flujoCaja.Table.Columns[a].ColumnName] = flujoCaja.Table.Rows[i][a].ToString();

                        if (a % limit == 0)
                            break;
                    }

                    ds.Tables["FlujoCajaPag"].Rows.Add(row);

                }

                count = (newCount + 1);
                flujoCajaPaging.Add(ds.Tables["FlujoCajaPag"].AsDataView());
            }

            return flujoCajaPaging;
        }

        public static DataTable GenerarFlujoCajaSocial(Proyecto proyecto, DataView dgvCapitalTrabajo, DataView dgvFinanciamiento, DataView dgvTotalesReinversiones, string totalInversiones, string recuperacionCT)
        {
            string signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);
            double valorDefecto = 0;
            string dsNombre = "FlujoCaja";
            DataSet ds = new DataSet();
            ds.Tables.Add(dsNombre);
            ds.Tables[dsNombre].Columns.Add("Rubro", Type.GetType("System.String"));
            for (int i = 0; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                ds.Tables[dsNombre].Columns.Add((proyecto.AnoInicial + i).ToString(), Type.GetType("System.String"));
            }

            #region costos
            DataRow row1 = ds.Tables[dsNombre].NewRow();
            row1["Rubro"] = "Costos Totales";
            int a1 = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                row1[(proyecto.AnoInicial + a1).ToString()] = costoGenerado.FormatoMoneda(signoMoneda);
                a1++;
            }
            ds.Tables[dsNombre].Rows.Add(row1);
            #endregion

            #region depreciaciones
            DataRow row2 = ds.Tables[dsNombre].NewRow();
            row2["Rubro"] = "Depreciaciones";
            int a2 = 1;
            foreach (double depreciacionAnual in proyecto.TotalDepreciaciones)
            {
                row2[(proyecto.AnoInicial + a2).ToString()] = depreciacionAnual.FormatoMoneda(signoMoneda);
                a2++;
            }
            ds.Tables[dsNombre].Rows.Add(row2);
            #endregion

            #region inversiones
            DataRow row3 = ds.Tables[dsNombre].NewRow();
            row3["Rubro"] = "Inversiones";
            int a3 = 0;
            row3[(proyecto.AnoInicial + a3).ToString()] = totalInversiones;
            a3++;

            if (dgvTotalesReinversiones.Table != null)
            {
                for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row3[(proyecto.AnoInicial + a3).ToString()] = dgvTotalesReinversiones.Table.Rows[0][i].ToString();
                    a3++;
                }
            }
            else
            {
                for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row3[(proyecto.AnoInicial + a3).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
                    a3++;
                }
            }
            ds.Tables[dsNombre].Rows.Add(row3);
            #endregion

            #region inv capital trabajo
            DataRow row4 = ds.Tables[dsNombre].NewRow();
            row4["Rubro"] = "Inv. Capital Trabajo";
            int a4 = 0;
            if (dgvCapitalTrabajo.Table != null)
            {
                for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos + 1; i++)
                {
                    row4[(proyecto.AnoInicial + a4).ToString()] = dgvCapitalTrabajo.Table.Rows[2][i].ToString();
                    a4++;
                }
            }
            else
            {
                for (int i = 1; i < proyecto.HorizonteEvaluacionEnAnos + 1; i++)
                {
                    row4[(proyecto.AnoInicial + a4).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
                    a4++;
                }
            }

            row4[(proyecto.AnoInicial + a4).ToString()] = valorDefecto.FormatoMoneda(signoMoneda);
            ds.Tables[dsNombre].Rows.Add(row4);
            #endregion

            #region recuperacionCT
            DataRow row5 = ds.Tables[dsNombre].NewRow();
            row5["Rubro"] = "Recuperación CT";
            row5[(proyecto.AnoInicial + proyecto.HorizonteEvaluacionEnAnos).ToString()] = recuperacionCT;
            ds.Tables[dsNombre].Rows.Add(row5);
            #endregion

            #region valorResidual
            DataRow row6 = ds.Tables[dsNombre].NewRow();
            row6["Rubro"] = "Valor Residual";
            row6[(proyecto.AnoInicial + proyecto.HorizonteEvaluacionEnAnos).ToString()] = proyecto.ValorResidual.FormatoMoneda(signoMoneda);
            ds.Tables[dsNombre].Rows.Add(row6);
            #endregion

            #region flujo efectivo
            DataRow row7 = ds.Tables[dsNombre].NewRow();
            row7["Rubro"] = "Flujo Efectivo";

            for (int i = 0; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                double flujoEfectivo = (
                    ds.Tables[dsNombre].Rows[0].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[1].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[2].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[3].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[4].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda) +
                    ds.Tables[dsNombre].Rows[5].ItemArray[i + 1].ToString().FormatoNumero(signoMoneda));
                double flujoEfectivoPonderado = flujoEfectivo.PonderarNumero();
                row7[(proyecto.AnoInicial + i).ToString()] = flujoEfectivoPonderado.FormatoMoneda(signoMoneda);
            }
            ds.Tables[dsNombre].Rows.Add(row7);
            #endregion

            return ds.Tables[dsNombre];
        }
    }
}
