using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class TipoMonedaData:BaseData
    {
        public TipoMonedaData() { }

        public List<TipoMoneda> GetTiposMonedas()
        {
            List<TipoMoneda> tiposMonedas = new List<TipoMoneda>();
            string select = "SELECT * FROM TIPO_MONEDA";

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
                            TipoMoneda tipoMoneda = new TipoMoneda();
                            tipoMoneda.CodMoneda = reader.GetInt32(0);
                            tipoMoneda.NombreMoneda = reader.GetString(1);
                            tipoMoneda.SignoMoneda = reader.GetString(2);
                            tiposMonedas.Add(tipoMoneda);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    tiposMonedas = new List<TipoMoneda>();
                }
            }

            return tiposMonedas;
        }

        public TipoMoneda GetTipoMoneda(int codTipoMoneda)
        {
            TipoMoneda tipoMoneda = new TipoMoneda();
            string select = "SELECT * FROM TIPO_MONEDA WHERE cod_tipo_moneda=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_tipo_moneda", codTipoMoneda);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tipoMoneda.CodMoneda = reader.GetInt32(0);
                            tipoMoneda.NombreMoneda = reader.GetString(1);
                            tipoMoneda.SignoMoneda = reader.GetString(2);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    tipoMoneda = new TipoMoneda();
                }
            }

            return tipoMoneda;
        }
    }
}
