using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.DataAccess
{
    public class ExportarData
    {
        string genericCadenaConexion;

        public ExportarData()
        {
            genericCadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["genericdb"].
                ConnectionString;
        }

        public Dictionary<Guid, int> GetIndicesProyecto(string dbName)
        {
            Dictionary<Guid, int> listaIndices = new Dictionary<Guid, int>();
            string cadenaConexion = genericCadenaConexion.Replace("{AppDir}", 
                AppDomain.CurrentDomain.BaseDirectory).Replace("{DatabasePath}", dbName);

            string select = "SELECT * FROM PROYECTO_INDICE";
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
                            listaIndices.Add(
                                new Guid(reader[1].ToString()),
                                int.Parse(reader[0].ToString())
                            );
                        }
                    }
                }
                catch { throw; }
            }

            return listaIndices;
        }

        public DataTable GetProyecto(string dbName, int codProyecto)
        {
            string cadenaConexion = genericCadenaConexion.Replace("{AppDir}",
                AppDomain.CurrentDomain.BaseDirectory).Replace("{DatabasePath}", dbName);
            string select = string.Format("SELECT * FROM PROYECTO WHERE cod_proyecto={0};" +
                "SELECT * FROM PROPONENTE p, ORGANIZACION_PROPONENTE op " + 
                "WHERE p.cod_proyecto={0} AND p.cod_proponente=op.cod_proponente;" +
                "SELECT * FROM REQUERIMIENTO_INVERSION WHERE cod_proyecto={0};" +
                "SELECT * FROM REQUERIMIENTO_REINVERSION WHERE cod_proyecto={0};" +
                "SELECT * FROM VARIACION_ANUAL_COSTO WHERE cod_proyecto={0};" +
                "SELECT * FROM FINANCIAMIENTO WHERE cod_proyecto={0};" +
                "SELECT * FROM INTERES_FINANCIAMIENTO WHERE cod_proyecto={0};"

                , codProyecto);

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();

                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                }
                catch { throw; }
            }

            return null;
        }
    }
}
