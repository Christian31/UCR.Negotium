using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class TipoProyectoData:BaseData
    {
        public TipoProyectoData() { }

        public List<TipoProyecto> GetTipoProyectos()
        {
            List<TipoProyecto> tipoProyectos = new List<TipoProyecto>();
            string select = "SELECT * FROM TIPO_PROYECTO";

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
                            TipoProyecto tipoProyecto = new TipoProyecto();
                            tipoProyecto.CodTipo = reader.GetInt32(0);
                            tipoProyecto.Nombre = reader.GetString(1);
                            tipoProyectos.Add(tipoProyecto);
                        }
                    }
                }
                catch
                {
                    tipoProyectos = new List<TipoProyecto>();
                }
            }

            return tipoProyectos;
        }

        public TipoProyecto GetTipoProyecto(int codTipoProyecto)
        {
            TipoProyecto tipoProyecto = new TipoProyecto();
            string select = "SELECT * FROM TIPO_PROYECTO WHERE cod_tipo_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_tipo_proyecto", codTipoProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tipoProyecto = new TipoProyecto();
                            tipoProyecto.CodTipo = reader.GetInt32(0);
                            tipoProyecto.Nombre = reader.GetString(1);
                        }
                    }
                }
                catch
                {
                    tipoProyecto = new TipoProyecto();
                }
            }

            return tipoProyecto;
        }
    }
}
