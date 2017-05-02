using System;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class CrecimientoOfertaObjetoInteresData : BaseData
    {
        private SQLiteCommand command;

        public CrecimientoOfertaObjetoInteresData() { }

        public DataTable GetCrecimientoOfertaObjetoIntereses(int codProyeccion)
        {
            string select = "SELECT cod_crecimiento,ano_crecimiento,porcentaje_crecimiento " +
                "FROM CRECIMIENTO_OFERTA_OBJETO_INTERES WHERE cod_proyecto=" + codProyeccion + ";";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            //command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataAdapter daCrecimientos = new SQLiteDataAdapter();
            daCrecimientos.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsCrecimientos = new DataSet();
            daCrecimientos.Fill(dsCrecimientos, "CrecimientoOfertaObjeto");
            DataTable dtCrecimientos = dsCrecimientos.Tables["CrecimientoOfertaObjeto"];
            conexion.Close();
            return dtCrecimientos;
        }//GetCrecimientoOfertaObjetoIntereses 

        public CrecimientoOfertaObjetoInteres InsertarCrecimientoOfertaObjetoIntereses(CrecimientoOfertaObjetoInteres crecimiento, int codProyecto)
        {
            object newProdID;
            string insert = "INSERT INTO CRECIMIENTO_OFERTA_OBJETO_INTERES(ano_crecimiento," +
                " porcentaje_crecimiento, cod_proyecto) VALUES(?,?,?); " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("ano_crecimiento", crecimiento.AnoCrecimiento);
            command.Parameters.AddWithValue("porcentaje_crecimiento", crecimiento.PorcentajeCrecimiento);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                crecimiento.CodCrecimiento = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return crecimiento;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return crecimiento;
            }//catch
        }//InsertarCrecimientoOfertaObjetoIntereses

        public CrecimientoOfertaObjetoInteres EditarCrecimientoOfertaObjetoIntereses(CrecimientoOfertaObjetoInteres crecimiento)
        {
            string insert = "UPDATE CRECIMIENTO_OFERTA_OBJETO_INTERES SET porcentaje_crecimiento = ? WHERE cod_crecimiento = ?;";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("porcentaje_crecimiento", crecimiento.PorcentajeCrecimiento);
            command.Parameters.AddWithValue("cod_crecimiento", crecimiento.CodCrecimiento);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                command.ExecuteNonQuery();

                conexion.Close();
                return crecimiento;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return crecimiento;
            }//catch
        }//EditarCrecimientoOfertaObjetoIntereses

        public bool eliminarCrecimientoObjetoInteres(int codProyecto) {
            string select = "DELETE FROM CRECIMIENTO_OFERTA_OBJETO_INTERES WHERE cod_proyecto=" + codProyecto + ";";

            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                //command = conexion.CreateCommand();
                command.CommandText = select;
                command.ExecuteNonQuery();

                conexion.Close();
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return false;
            }
        }
    }
}
