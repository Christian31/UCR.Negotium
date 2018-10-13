using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Base.Trace;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class CantonData: BaseData
    {
        public CantonData() { }

        public List<Canton> GetCantonesPorProvincia(int codProvincia)
        {
            DistritoData distritoData = new DistritoData();
            List<Canton> cantones = new List<Canton>();
            string select = "SELECT * FROM CANTON WHERE cod_provincia=? ORDER BY nombre_canton";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_provincia", codProvincia);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Canton canton = new Canton();
                            canton.CodCanton = reader.GetInt32(0);
                            canton.NombreCanton = reader.GetString(2);
                            canton.Distritos = distritoData.GetDistritosPorCanton(canton.CodCanton);
                            cantones.Add(canton);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    cantones = new List<Canton>();
                }
            }

            return cantones;
        }
    }
}
