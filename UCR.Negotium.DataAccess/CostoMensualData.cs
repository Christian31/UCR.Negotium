using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class CostoMensualData :BaseData
    {
        private SQLiteCommand command;

        public CostoMensualData() { }

        public List<CostoMensual> GetCostosMensuales(int codCosto)
        {
            List<CostoMensual> listaCostos = new List<CostoMensual>();
            try
            {
                string select = "SELECT cod_costo_mensual, mes, costo_unitario, cantidad " +
                "FROM COSTO_MENSUAL WHERE cod_costo=" + codCosto + ";";

                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CostoMensual detallecosto = new CostoMensual();
                    detallecosto.CodCostoMensual = reader.GetInt32(0);
                    detallecosto.Mes = reader.GetString(1);
                    detallecosto.CostoUnitario = reader.GetInt32(2);
                    detallecosto.Cantidad = reader.GetDouble(3);
                    listaCostos.Add(detallecosto);
                }//while
                conexion.Close();
                return listaCostos;
            }//try
            catch
            {
                conexion.Close();
                return listaCostos;
            }//catch
        }//GetDetallesProyeccionVenta 
    }
}
