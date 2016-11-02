//@Copyright Yordan Campos Piedra
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
    public class ObjetoInteresData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        private SQLiteCommand command;
        private UnidadMedidaData unidadMedidaData;

        public ObjetoInteresData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
            unidadMedidaData = new UnidadMedidaData();
        }

        public ObjetoInteresProyecto GetObjetoInteres(int codProyecto)
        {
            String select = "SELECT * FROM OBJETO_INTERES_PROYECTO WHERE cod_proyecto="+codProyecto;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            ObjetoInteresProyecto objetoInteres = null;
            if (reader.Read())
            {
                objetoInteres = new ObjetoInteresProyecto();
                objetoInteres.DescripcionObjetoInteres = reader.GetString(0);
                objetoInteres.CodObjetoInteres = reader.GetInt32(3);
                objetoInteres.UnidadMedida = unidadMedidaData.GetUnidadMedida(reader.GetInt32(2));
            }//if
            conexion.Close();
            return objetoInteres;
        }

        public bool InsertarObjetoDeInteres(ObjetoInteresProyecto objetoInteres, int codProyecto)
        {
            String insert = "INSERT INTO OBJETO_INTERES_PROYECTO(descripcion_objeto_interes, cod_proyecto, cod_unidad) VALUES(?,?,?)";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("descripcion_objeto_interes", objetoInteres.DescripcionObjetoInteres);
            command.Parameters.AddWithValue("cod_proyecto", codProyecto);
            command.Parameters.AddWithValue("cod_unidad", objetoInteres.UnidadMedida.CodUnidad);
            // Ejecutamos la sentencia INSERT y cerramos la conexión
            if (command.ExecuteNonQuery() != -1)
            {
                conexion.Close();
                return true;
            }//if
            else
            {
                conexion.Close();
                return false;
            }//else
        }//InsertarObjetoDeInteres

        public bool ActualizarObjetoInteres(ObjetoInteresProyecto objetoInteres, int codProyecto)
        {
            String update = "UPDATE OBJETO_INTERES_PROYECTO SET descripcion_objeto_interes=?, " +
                "cod_unidad=? WHERE cod_proyecto="+codProyecto;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = update;
            command.Parameters.AddWithValue("descripcion_objeto_interes", objetoInteres.DescripcionObjetoInteres);
            command.Parameters.AddWithValue("cod_unidad", objetoInteres.UnidadMedida.CodUnidad);
            // Ejecutamos la sentencia UPDATE y cerramos la conexión
            if (command.ExecuteNonQuery() != -1)
            {
                conexion.Close();
                return true;
            }//if
            else
            {
                conexion.Close();
                return false;
            }//else
        }//ActualizarObjetoInteres
    }//ObjetoInteresData
}
