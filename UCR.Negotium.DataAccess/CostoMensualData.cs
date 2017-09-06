using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class CostoMensualData:BaseData
    {
        public CostoMensualData() { }

        public List<CostoMensual> GetCostosMensuales(int codCosto)
        {
            List<CostoMensual> listaCostos = new List<CostoMensual>();
            string select = "SELECT * FROM COSTO_MENSUAL WHERE cod_costo=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_costo", codCosto);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CostoMensual detallecosto = new CostoMensual();
                            detallecosto.CodCostoMensual = reader.GetInt32(0);
                            detallecosto.Mes = reader.GetString(1);
                            detallecosto.CostoUnitario = reader.GetDouble(2);
                            detallecosto.Cantidad = reader.GetDouble(3);
                            listaCostos.Add(detallecosto);
                        }
                    }
                }
                catch
                {
                    listaCostos = new List<CostoMensual>();
                }
            }

            return listaCostos;
        }
    }
}