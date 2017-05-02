using System;
using System.Data.SQLite;

namespace UCR.Negotium.DataAccess
{
    public class BaseData
    {
        private string cadenaConexion;
        internal SQLiteConnection conexion;

        public BaseData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }
    }
}
