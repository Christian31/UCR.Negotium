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
    public class ProyectoData
    {
        private String cadenaConexion;
        private SQLiteConnection conexion;
        

        public ProyectoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            conexion = new SQLiteConnection(cadenaConexion);
        }

        //El siguiente metodo va a insertar un proyecto en la base de datos
        public int InsertarProyecto(Proyecto proyecto)
        {
            int idProyecto = -1;
            Object newProdID;
            String insert = "INSERT INTO PROYECTO(nombre_proyecto, resumen_ejecutivo, con_ingresos, "+
                "ano_inicial_proyecto, horizonte_evaluacion_en_anos, "+
                "demanda_anual, oferta_anual, paga_impuesto, porcentaje_impuesto, "+
                "direccion_exacta, cod_provincia, cod_canton, cod_distrito, id_evaluador)"+
                " VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?);"
                + "SELECT last_insert_rowid();";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = insert;
            command.Parameters.AddWithValue("nombre_proyecto", proyecto.NombreProyecto);
            command.Parameters.AddWithValue("resumen_ejecutivo", proyecto.ResumenEjecutivo);
            command.Parameters.AddWithValue("con_ingresos", proyecto.ConIngresos);
            command.Parameters.AddWithValue("ano_inicial_proyecto", proyecto.AnoInicial);
            command.Parameters.AddWithValue("horizonte_evaluacion_en_anos", proyecto.HorizonteEvaluacionEnAnos);
            command.Parameters.AddWithValue("demanda_anual", proyecto.DemandaAnual);
            command.Parameters.AddWithValue("oferta_anual", proyecto.OfertaAnual);
            command.Parameters.AddWithValue("paga_impuesto", proyecto.PagaImpuesto);
            command.Parameters.AddWithValue("porcentaje_impuesto", proyecto.PorcentajeImpuesto);
            command.Parameters.AddWithValue("direccion_exacta", proyecto.DireccionExacta);
            command.Parameters.AddWithValue("cod_provincia", proyecto.Provincia.CodProvincia);
            command.Parameters.AddWithValue("cod_canton", proyecto.Canton.CodCanton);
            command.Parameters.AddWithValue("cod_distrito", proyecto.Distrito.CodDistrito);
            command.Parameters.AddWithValue("id_evaluador", proyecto.Evaluador.IdEvaluador);
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                newProdID = command.ExecuteScalar();
                idProyecto = Int32.Parse(newProdID.ToString());
                conexion.Close();
                return idProyecto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                conexion.Close();
                return idProyecto;
            }
        }//InsertarProyecto

        //El siguiente metodo va a actualizar un proyecto en la base de datos
        public bool ActualizarProyecto(Proyecto proyecto)
        {
            String update = "UPDATE PROYECTO SET nombre_proyecto=?, "+
                "resumen_ejecutivo=?, con_ingresos=?, "+
                "descripcion_poblacion_beneficiaria=?, "+
                "categorizacion_bien_servicio=?, "+
                "descripcion_sostenibilidad_proyecto=?, "+
                "justificacion_de_mercado=?, ano_inicial_proyecto=?, "+
                "horizonte_evaluacion_en_anos=?,demanda_anual=?, oferta_anual=?, " +
                "paga_impuesto=?, porcentaje_impuesto=?, direccion_exacta=?, "+
                "cod_provincia=?, cod_canton=?, cod_distrito=?, id_evaluador=? WHERE cod_proyecto=?; ";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = update;
            command.Parameters.AddWithValue("nombre_proyecto", proyecto.NombreProyecto);
            command.Parameters.AddWithValue("resumen_ejecutivo", proyecto.ResumenEjecutivo);
            command.Parameters.AddWithValue("con_ingresos", proyecto.ConIngresos);
            command.Parameters.AddWithValue("descripcion_poblacion_beneficiaria", proyecto.DescripcionPoblacionBeneficiaria);
            command.Parameters.AddWithValue("categorizacion_bien_servicio", proyecto.CaraterizacionDelBienServicio);
            command.Parameters.AddWithValue("descripcion_sostenibilidad_proyecto", proyecto.DescripcionSostenibilidadDelProyecto);
            command.Parameters.AddWithValue("justificacion_de_mercado", proyecto.JustificacionDeMercado);
            command.Parameters.AddWithValue("ano_inicial_proyecto", proyecto.AnoInicial);
            command.Parameters.AddWithValue("horizonte_evaluacion_en_anos", proyecto.HorizonteEvaluacionEnAnos);
            command.Parameters.AddWithValue("demanda_anual", proyecto.DemandaAnual);
            command.Parameters.AddWithValue("oferta_anual", proyecto.OfertaAnual);
            command.Parameters.AddWithValue("paga_impuesto", proyecto.PagaImpuesto);
            command.Parameters.AddWithValue("porcentaje_impuesto", proyecto.PorcentajeImpuesto);
            command.Parameters.AddWithValue("direccion_exacta", proyecto.DireccionExacta);
            command.Parameters.AddWithValue("cod_provincia", proyecto.Provincia.CodProvincia);
            command.Parameters.AddWithValue("cod_canton", proyecto.Canton.CodCanton);
            command.Parameters.AddWithValue("cod_distrito", proyecto.Distrito.CodDistrito);
            command.Parameters.AddWithValue("id_evaluador", proyecto.Evaluador.IdEvaluador);
            command.Parameters.AddWithValue("cod_proyecto", proyecto.CodProyecto);
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
        }//ActualizarProyecto

        //Extrae todos los proyectos de la base de datos y los retorna en un datatable
        public DataTable GetProyectos()
        {
            String select = "SELECT * FROM PROYECTO";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataAdapter daProyectos = new SQLiteDataAdapter();
            daProyectos.SelectCommand = new SQLiteCommand(select, conexion);
            DataSet dsProyecto = new DataSet();
            daProyectos.Fill(dsProyecto, "Proyecto");
            DataTable dtProyecto = dsProyecto.Tables["Proyecto"];
            conexion.Close();
            return dtProyecto;
        }
    }//ProyectoData
}
