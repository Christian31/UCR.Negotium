﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Base.Trace;

namespace UCR.Negotium.DataAccess
{
    public class CrecimientoOfertaData:BaseData
    {
        public CrecimientoOfertaData() { }

        public List<CrecimientoOferta> GetCrecimientoOfertaObjetoIntereses(int codProyeccion)
        {
            List<CrecimientoOferta> crecimientosOferta = new List<CrecimientoOferta>();
            string select = "SELECT cod_crecimiento, ano_crecimiento, porcentaje_crecimiento " +
                "FROM CRECIMIENTO_OFERTA_OBJETO_INTERES WHERE cod_proyeccion=?";

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
                            CrecimientoOferta crecimientoOferta = new CrecimientoOferta();
                            crecimientoOferta.CodCrecimiento = reader.GetInt32(0);
                            crecimientoOferta.AnoCrecimiento = reader.GetInt32(1);
                            crecimientoOferta.PorcentajeCrecimiento = reader.GetDouble(2);
                            crecimientosOferta.Add(crecimientoOferta);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    crecimientosOferta = new List<CrecimientoOferta>();
                }
            }

            return crecimientosOferta;
        } 

        public CrecimientoOferta InsertarCrecimientoOfertaObjetoIntereses(CrecimientoOferta crecimiento, int codProyeccion)
        {
            object newProdID;
            string insert = "INSERT INTO CRECIMIENTO_OFERTA_OBJETO_INTERES(ano_crecimiento, " +
                "porcentaje_crecimiento, cod_proyeccion) VALUES(?,?,?); " +
                "SELECT last_insert_rowid();";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("ano_crecimiento", crecimiento.AnoCrecimiento);
                    cmd.Parameters.AddWithValue("porcentaje_crecimiento", crecimiento.PorcentajeCrecimiento);
                    cmd.Parameters.AddWithValue("cod_proyeccion", codProyeccion);

                    newProdID = cmd.ExecuteScalar();
                    crecimiento.CodCrecimiento = int.Parse(newProdID.ToString());
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    crecimiento = new CrecimientoOferta();
                }
            }

            return crecimiento;
        }

        public bool EditarCrecimientoOfertaObjetoIntereses(CrecimientoOferta crecimiento)
        {
            int result = -1;
            string insert = "UPDATE CRECIMIENTO_OFERTA_OBJETO_INTERES SET porcentaje_crecimiento = ? " +
                "WHERE cod_crecimiento=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("porcentaje_crecimiento", crecimiento.PorcentajeCrecimiento);
                    cmd.Parameters.AddWithValue("cod_crecimiento", crecimiento.CodCrecimiento);

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

        public bool EliminarCrecimientoObjetoInteres(int codProyeccion)
        {
            int result = -1;
            string delete = "DELETE FROM CRECIMIENTO_OFERTA_OBJETO_INTERES WHERE cod_proyeccion=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(delete, conn);
                    cmd.Parameters.AddWithValue("cod_proyeccion", codProyeccion);

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
