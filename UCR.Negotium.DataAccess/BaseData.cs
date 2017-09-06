using System;

namespace UCR.Negotium.DataAccess
{
    public class BaseData
    {
        protected string cadenaConexion;

        public BaseData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
