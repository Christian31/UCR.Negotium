using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class CrecimientoOfertaObjetoInteresData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;

        public CrecimientoOfertaObjetoInteresData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public List<CrecimientoOfertaObjetoInteres> GetCrecimientoOfertaObjetoIntereses(int codProyeccion)
        {
            List<CrecimientoOfertaObjetoInteres> crecimientosOferta = new List<CrecimientoOfertaObjetoInteres>();
            string select = "SELECT cod_crecimiento,ano_crecimiento,porcentaje_crecimiento " +
                "FROM CRECIMIENTO_OFERTA_OBJETO_INTERES WHERE cod_proyeccion_venta=" + codProyeccion + ";";
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = select;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CrecimientoOfertaObjetoInteres crecimientoOferta = new CrecimientoOfertaObjetoInteres();
                    crecimientoOferta.CodCrecimiento = reader.GetInt32(0);
                    crecimientoOferta.AnoCrecimiento = reader.GetInt32(1);
                    crecimientoOferta.PorcentajeCrecimiento = reader.GetDouble(2);
                    crecimientosOferta.Add(crecimientoOferta);
                }

                conexion.Close();
                return crecimientosOferta;
            }
            catch
            {
                conexion.Close();
                return crecimientosOferta;
            }
        }//GetCrecimientoOfertaObjetoIntereses 

        public CrecimientoOfertaObjetoInteres InsertarCrecimientoOfertaObjetoIntereses(CrecimientoOfertaObjetoInteres crecimiento, int codProyeccion)
        {
            object newProdID;
            string insert = "INSERT INTO CRECIMIENTO_OFERTA_OBJETO_INTERES(ano_crecimiento," +
                " porcentaje_crecimiento, cod_proyeccion_venta) VALUES(?,?,?); " +
            "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("ano_crecimiento", crecimiento.AnoCrecimiento);
            command.Parameters.AddWithValue("porcentaje_crecimiento", crecimiento.PorcentajeCrecimiento);
            command.Parameters.AddWithValue("cod_proyeccion_venta", codProyeccion);
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

        public bool EditarCrecimientoOfertaObjetoIntereses(CrecimientoOfertaObjetoInteres crecimiento)
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
                return true;
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return false;
            }//catch
        }//EditarCrecimientoOfertaObjetoIntereses

        public bool EliminarCrecimientoObjetoInteres(int codProyecto) {
            string select = "DELETE FROM CRECIMIENTO_OFERTA_OBJETO_INTERES WHERE cod_proyeccion_venta=" + codProyecto + ";";

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
