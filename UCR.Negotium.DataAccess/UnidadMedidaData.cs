using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Base.Trace;

namespace UCR.Negotium.DataAccess
{
    public class UnidadMedidaData:BaseData
    {
        public UnidadMedidaData() { }

        public List<UnidadMedida> GetUnidadesMedidas()
        {
            List<UnidadMedida> unidadesMedida = new List<UnidadMedida>();
            string select = "SELECT * FROM UNIDAD_MEDIDA WHERE alcance_unidad='*'";

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
                            UnidadMedida unidadMedida = new UnidadMedida();
                            unidadMedida.CodUnidad = reader.GetInt32(0);
                            unidadMedida.NombreUnidad = reader.GetString(1);
                            unidadesMedida.Add(unidadMedida);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    unidadesMedida = new List<UnidadMedida>();
                }
            }

            return unidadesMedida;
        }

        public List<UnidadMedida> GetUnidadesMedidasParaCostos()
        {
            List<UnidadMedida> unidadesMedida = new List<UnidadMedida>();
            string select = "SELECT * FROM UNIDAD_MEDIDA WHERE alcance_unidad='*' OR alcance_unidad='CT'";

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
                            UnidadMedida unidadMedida = new UnidadMedida();
                            unidadMedida.CodUnidad = reader.GetInt32(0);
                            unidadMedida.NombreUnidad = reader.GetString(1);
                            unidadesMedida.Add(unidadMedida);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    unidadesMedida = new List<UnidadMedida>();
                }
            }

            return unidadesMedida;
        }
        public UnidadMedida GetUnidadMedida(int codUnidadMedida)
        {
            UnidadMedida unidadMedida = new UnidadMedida();
            string select = "SELECT * FROM UNIDAD_MEDIDA WHERE cod_unidad=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_unidad", codUnidadMedida);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            unidadMedida.CodUnidad = reader.GetInt32(0);
                            unidadMedida.NombreUnidad = reader.GetString(1);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    unidadMedida = new UnidadMedida();
                }
            }

            return unidadMedida;
        }
    }
}
