using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class ProvinciaData:BaseData
    {
        private CantonData cantonData;

        public ProvinciaData()
        {
            cantonData = new CantonData();
        }

        public List<Provincia> GetProvincias()
        {
            List<Provincia> provincias = new List<Provincia>();
            string select = "SELECT * FROM PROVINCIA";

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
                            Provincia provincia = new Provincia();
                            provincia.CodProvincia = reader.GetInt32(0);
                            provincia.NombreProvincia = reader.GetString(1);
                            provincia.Cantones = cantonData.GetCantonesPorProvincia(provincia.CodProvincia);
                            provincias.Add(provincia);
                        }
                    }
                }
                catch
                {
                    provincias = new List<Provincia>();
                }
            }

            return provincias;
        }
    }
}
