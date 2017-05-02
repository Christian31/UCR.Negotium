//@Copyright Yordan Campos Piedra
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class TipoOrganizacionData : BaseData
    {
        private SQLiteCommand command;

        public TipoOrganizacionData() { }

        public DataTable GetTiposDeOrganizacion()
        {
            string select = "SELECT * FROM TIPO_ORGANIZACION";
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

        public List<TipoOrganizacion> GetTiposDeOrganizacionAux()
        {
            List<TipoOrganizacion> tipoOrganizaciones = new List<TipoOrganizacion>();
            string select = "SELECT * FROM TIPO_ORGANIZACION";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                TipoOrganizacion tipoOrganizacion = new TipoOrganizacion();
                tipoOrganizacion.CodTipo = reader.GetInt32(0);
                tipoOrganizacion.Descripcion = reader.GetString(1);
                tipoOrganizaciones.Add(tipoOrganizacion);
            }
            conexion.Close();

            return tipoOrganizaciones;
        }
    }
}
