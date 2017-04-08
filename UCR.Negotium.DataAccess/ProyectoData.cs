//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
            object newProdID;
            string insert = "INSERT INTO PROYECTO(nombre_proyecto, resumen_ejecutivo, con_ingresos, "+
                "ano_inicial_proyecto, horizonte_evaluacion_en_anos, paga_impuesto, porcentaje_impuesto, "+
                "direccion_exacta, cod_provincia, cod_canton, cod_distrito, id_evaluador)"+
                " VALUES(?,?,?,?,?,?,?,?,?,?,?,?); "
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
            String update = "UPDATE PROYECTO SET nombre_proyecto=?, resumen_ejecutivo=?, con_ingresos=?, "+
                "descripcion_poblacion_beneficiaria=?, categorizacion_bien_servicio=?, "+
                "descripcion_sostenibilidad_proyecto=?, justificacion_de_mercado=?, ano_inicial_proyecto=?, "+
                "horizonte_evaluacion_en_anos=?, paga_impuesto=?, porcentaje_impuesto=?, direccion_exacta=?, "+
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

        public bool ActualizarProyectoFlujoCaja(Proyecto proyecto)
        {
            String update = "UPDATE PROYECTO SET tasa_costo_capital=?, " +
                "personas_participantes=?, familias_involucradas=?, " +
                "beneficiarios_indirectos=? WHERE cod_proyecto=?; ";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command.CommandText = update;
            command.Parameters.AddWithValue("tasa_costo_capital", proyecto.TasaCostoCapital);
            command.Parameters.AddWithValue("personas_participantes", proyecto.PersonasParticipantes);
            command.Parameters.AddWithValue("familias_involucradas", proyecto.FamiliasInvolucradas);
            command.Parameters.AddWithValue("beneficiarios_indirectos", proyecto.PersonasBeneficiadas);
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

        public List<Proyecto> GetProyectosAux()
        {
            ProponenteData proponenteData = new ProponenteData();
            List<Proyecto> proyectos = new List<Proyecto>();
            string select = "SELECT * FROM PROYECTO";
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Proyecto proyecto = new Proyecto();
                proyecto.CodProyecto = int.Parse(reader["cod_proyecto"].ToString()); ;
                proyecto.AnoInicial = int.Parse(reader["ano_inicial_proyecto"].ToString());
                //proyecto.Evaluador = this.evaluador;
                proyecto.HorizonteEvaluacionEnAnos = Int32.Parse(reader["horizonte_evaluacion_en_anos"].ToString());
                proyecto.NombreProyecto = reader["nombre_proyecto"].ToString();
                //proyecto.ObjetoInteres = GetObjetoInteres(proyecto.CodProyecto);
                //proyecto.CrecimientosAnuales = GetCrecimientosAnuales(proyecto.CodProyecto);
                //proyecto.VariacionCostos = GetVariacionAnualCostos(proyecto.CodProyecto);
                //proyecto.Costos = GetCostos(proyecto.CodProyecto);
                proyecto.Proponente = proponenteData.GetProponente(proyecto.CodProyecto);
                
                proyectos.Add(proyecto);
            }//if
            conexion.Close();
            return proyectos;
        }

        //Extrae todos los proyectos de la base de datos y los retorna en un datatable
        public Proyecto GetProyecto(int codProyecto)
        {
            Proyecto proyecto = new Proyecto();
            String select = "SELECT * FROM PROYECTO WHERE cod_proyecto=" + codProyecto;
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
            SQLiteCommand command = conexion.CreateCommand();
            command = conexion.CreateCommand();
            command.CommandText = select;
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                proyecto.CodProyecto = codProyecto;
                proyecto.AnoInicial = Int32.Parse(reader["ano_inicial_proyecto"].ToString());
                proyecto.Canton.CodCanton = Int32.Parse(reader["cod_canton"].ToString());
                proyecto.Provincia.CodProvincia = Int32.Parse(reader["cod_provincia"].ToString());
                proyecto.Distrito.CodDistrito = Int32.Parse(reader["cod_distrito"].ToString());
                proyecto.CaraterizacionDelBienServicio = reader["categorizacion_bien_servicio"].ToString();
                proyecto.CodProyecto = Int32.Parse(reader["cod_proyecto"].ToString());
                proyecto.ConIngresos = Boolean.Parse(reader["con_ingresos"].ToString());
                proyecto.DescripcionPoblacionBeneficiaria = reader["descripcion_poblacion_beneficiaria"].ToString();
                proyecto.DescripcionSostenibilidadDelProyecto = reader["descripcion_sostenibilidad_proyecto"].ToString();
                proyecto.DireccionExacta = reader["direccion_exacta"].ToString();
                proyecto.Distrito.CodDistrito = Int32.Parse(reader["cod_distrito"].ToString());
                //proyecto.Evaluador = this.evaluador;
                proyecto.HorizonteEvaluacionEnAnos = Int32.Parse(reader["horizonte_evaluacion_en_anos"].ToString());
                proyecto.JustificacionDeMercado = reader["justificacion_de_mercado"].ToString();
                proyecto.NombreProyecto = reader["nombre_proyecto"].ToString();
                //proyecto.ObjetoInteres = GetObjetoInteres(proyecto.CodProyecto);
                //proyecto.CrecimientosAnuales = GetCrecimientosAnuales(proyecto.CodProyecto);
                //proyecto.VariacionCostos = GetVariacionAnualCostos(proyecto.CodProyecto);
                //proyecto.Costos = GetCostos(proyecto.CodProyecto);
                proyecto.PagaImpuesto = Boolean.Parse(reader["paga_impuesto"].ToString());
                proyecto.PorcentajeImpuesto = float.Parse(reader["porcentaje_impuesto"].ToString());
                //proyecto.Proponente = proponenteData.GetProponente(proyecto.CodProyecto);
                proyecto.ResumenEjecutivo = reader["resumen_ejecutivo"].ToString();
                proyecto.FamiliasInvolucradas = Int32.Parse(reader["familias_involucradas"].ToString());
                proyecto.TasaCostoCapital = Double.Parse(reader["tasa_costo_capital"].ToString());
                proyecto.PersonasBeneficiadas = Int32.Parse(reader["beneficiarios_indirectos"].ToString());
                proyecto.PersonasParticipantes = Int32.Parse(reader["personas_participantes"].ToString());

                return proyecto;
            }//if
            conexion.Close();
            return proyecto;
        }
    }//ProyectoData
}
