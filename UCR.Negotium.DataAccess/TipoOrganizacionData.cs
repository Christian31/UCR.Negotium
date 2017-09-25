using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class TipoOrganizacionData:BaseData
    {
        public TipoOrganizacionData() { }

        public List<TipoOrganizacion> GetTipoOrganizaciones()
        {
            List<TipoOrganizacion> tipoOrganizaciones = new List<TipoOrganizacion>();
            string select = "SELECT * FROM TIPO_ORGANIZACION";

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
                            TipoOrganizacion tipoOrganizacion = new TipoOrganizacion();
                            tipoOrganizacion.CodTipo = reader.GetInt32(0);
                            tipoOrganizacion.Descripcion = reader.GetString(1);
                            tipoOrganizaciones.Add(tipoOrganizacion);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    tipoOrganizaciones = new List<TipoOrganizacion>();
                }
            }

            return tipoOrganizaciones;
        }
    }
}
