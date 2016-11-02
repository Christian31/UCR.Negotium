//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.DataAccess
{
    public class TipoOrganizacionData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;

        public TipoOrganizacionData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public DataTable GetTiposDeOrganizacion()
        {
            String select = "SELECT * FROM TIPO_ORGANIZACION";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteDataAdapter daTipoOrganizacion = new SQLiteDataAdapter();
            daTipoOrganizacion.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsTipoOrganizacion = new DataSet();
            daTipoOrganizacion.Fill(dsTipoOrganizacion, "TipoOrganizacion");
            DataTable dtTipoOrganizacion = dsTipoOrganizacion.Tables["TipoOrganizacion"];
            conexion.Close();
            return dtTipoOrganizacion;
        }
    }
}
