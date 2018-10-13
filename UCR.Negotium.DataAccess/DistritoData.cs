using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Base.Trace;

namespace UCR.Negotium.DataAccess
{
    public class DistritoData:BaseData
    {
        public DistritoData(){ }

        public List<Distrito> GetDistritosPorCanton(int codCanton)
        {
            List<Distrito> distritos = new List<Distrito>();
            string select = "SELECT * FROM DISTRITO WHERE cod_canton=? ORDER BY nombre_distrito";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_canton", codCanton);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Distrito distrito = new Distrito();
                            distrito.CodDistrito = reader.GetInt32(0);
                            distrito.NombreDistrito = reader.GetString(2);
                            distritos.Add(distrito);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    distritos = new List<Distrito>();
                }
            }

            return distritos;
        }
    }
}
