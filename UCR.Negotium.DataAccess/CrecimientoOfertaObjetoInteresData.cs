using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class CrecimientoOfertaObjetoInteresData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public CrecimientoOfertaObjetoInteresData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            conexion = new SQLiteConnection(cadenaConexion);
        }

        public DataTable GetCrecimientoOfertaObjetoIntereses(String codProyecto)
        {
            String select = "SELECT cod_crecimiento,ano_crecimiento,porcentaje_crecimiento" +
                "FROM CRECIMIENTO_OFERTA_OBJETO_INTERES WHERE cod_proyecto=" + codProyecto + ";";

            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataAdapter daProvincias = new SQLiteDataAdapter();
            daProvincias.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsProvincia = new DataSet();
            daProvincias.Fill(dsProvincia, "CrecimientoOfertaObjeto");
            DataTable dtCrecimientos = dsProvincia.Tables["CrecimientoOfertaObjeto"];
            conexion.Close();
            return dtCrecimientos;
        }//GetCrecimientoOfertaObjetoIntereses 

        public CrecimientoOfertaObjetoInteres InsertarCrecimientoOfertaObjetoIntereses(CrecimientoOfertaObjetoInteres crecimiento, int codProyecto)
        {
            Object newProdID;
            String insert = "INSERT INTO CRECIMIENTO_OFERTA_OBJETO_INTERES(ano_crecimiento," +
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

        public CrecimientoOfertaObjetoInteres EditarCrecimientoOfertaObjetoIntereses(CrecimientoOfertaObjetoInteres crecimiento, int codProyecto)
        {
            Object newProdID;
            String insert = "UPDATE CRECIMIENTO_OFERTA_OBJETO_INTERES SET (ano_crecimiento = ?, porcentaje_crecimiento = ?, " +
                "cod_proyecto = ? WHERE cod_crecimiento = ?; " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("ano_crecimiento", crecimiento.AnoCrecimiento);
            command.Parameters.AddWithValue("porcentaje_crecimiento", crecimiento.PorcentajeCrecimiento);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            command.Parameters.AddWithValue("cod_crecimiento", crecimiento.CodCrecimiento);
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
        }//EditarCrecimientoOfertaObjetoIntereses
    }
}
