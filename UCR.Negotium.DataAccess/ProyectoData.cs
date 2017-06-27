using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess
{
    public class ProyectoData
    {
        private string cadenaConexion;
        private SQLiteConnection conexion;

        public ProyectoData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

            conexion = new SQLiteConnection(cadenaConexion);
        }

        public int InsertarProyecto(Proyecto proyecto)
        {
            int idProyecto = -1;
            object newProdID;
            string insert = "INSERT INTO PROYECTO(nombre_proyecto, resumen_ejecutivo, con_ingresos, "+
                "ano_inicial_proyecto, horizonte_evaluacion_en_anos, paga_impuesto, porcentaje_impuesto, "+
                "direccion_exacta, cod_provincia, cod_canton, cod_distrito, cod_evaluador, con_financiamiento, "+
                "objeto_interes, archivado)" +
                " VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); "
                + "SELECT last_insert_rowid();";
            try
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();
                SQLiteCommand command = conexion.CreateCommand();
                command.CommandText = insert;
                command.Parameters.AddWithValue("nombre_proyecto", proyecto.NombreProyecto);
                command.Parameters.AddWithValue("resumen_ejecutivo", proyecto.ResumenEjecutivo);
                command.Parameters.AddWithValue("con_ingresos", proyecto.ConIngresos?1:0);
                command.Parameters.AddWithValue("ano_inicial_proyecto", proyecto.AnoInicial);
                command.Parameters.AddWithValue("horizonte_evaluacion_en_anos", proyecto.HorizonteEvaluacionEnAnos);
                command.Parameters.AddWithValue("paga_impuesto", proyecto.PagaImpuesto);
                command.Parameters.AddWithValue("porcentaje_impuesto", proyecto.PorcentajeImpuesto);
                command.Parameters.AddWithValue("direccion_exacta", proyecto.DireccionExacta);
                command.Parameters.AddWithValue("cod_provincia", proyecto.Provincia.CodProvincia);
                command.Parameters.AddWithValue("cod_canton", proyecto.Canton.CodCanton);
                command.Parameters.AddWithValue("cod_distrito", proyecto.Distrito.CodDistrito);
                command.Parameters.AddWithValue("cod_evaluador", proyecto.Encargado.IdEncargado);
                command.Parameters.AddWithValue("con_financiamiento", proyecto.ConFinanciamiento);
                command.Parameters.AddWithValue("objeto_interes", proyecto.ObjetoInteres);
                command.Parameters.AddWithValue("archivado", proyecto.Archivado);

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
        }

        public bool ActualizarProyecto(Proyecto proyecto)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(conn))
                {
                    command.CommandText = "UPDATE PROYECTO SET nombre_proyecto=?, resumen_ejecutivo=?, con_ingresos=?, " +
                "ano_inicial_proyecto=?, horizonte_evaluacion_en_anos=?, paga_impuesto=?, " +
                "porcentaje_impuesto=?, direccion_exacta=?, cod_provincia=?, cod_canton=?, cod_distrito=?, " +
                "con_financiamiento=?, objeto_interes=? WHERE cod_proyecto=?;";
                    command.Prepare();
                    command.Parameters.AddWithValue("nombre_proyecto", proyecto.NombreProyecto);
                    command.Parameters.AddWithValue("resumen_ejecutivo", proyecto.ResumenEjecutivo);
                    command.Parameters.AddWithValue("con_ingresos", proyecto.ConIngresos?1:0);
                    command.Parameters.AddWithValue("ano_inicial_proyecto", proyecto.AnoInicial);
                    command.Parameters.AddWithValue("horizonte_evaluacion_en_anos", proyecto.HorizonteEvaluacionEnAnos);
                    command.Parameters.AddWithValue("paga_impuesto", proyecto.PagaImpuesto?1:0);
                    command.Parameters.AddWithValue("porcentaje_impuesto", proyecto.PorcentajeImpuesto);
                    command.Parameters.AddWithValue("direccion_exacta", proyecto.DireccionExacta);
                    command.Parameters.AddWithValue("cod_provincia", proyecto.Provincia.CodProvincia);
                    command.Parameters.AddWithValue("cod_canton", proyecto.Canton.CodCanton);
                    command.Parameters.AddWithValue("cod_distrito", proyecto.Distrito.CodDistrito);
                    command.Parameters.AddWithValue("con_financiamiento", proyecto.ConFinanciamiento?1:0);
                    command.Parameters.AddWithValue("objeto_interes", proyecto.ObjetoInteres);
                    command.Parameters.AddWithValue("cod_proyecto", proyecto.CodProyecto);
                    try
                    {
                        result = command.ExecuteNonQuery();
                    }
                    catch (SQLiteException)
                    {
                        return false;
                    }
                }
                conn.Close();
            }

            return result != -1;
        }

        public bool ActualizarProyectoCaracterizacion(Proyecto proyecto)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(conn))
                {
                    command.CommandText = "UPDATE PROYECTO SET descripcion_poblacion_beneficiaria=?, categorizacion_bien_servicio=?, " +
                "descripcion_sostenibilidad_proyecto=?, justificacion_de_mercado=? WHERE cod_proyecto=?";
                    command.Prepare();
                    command.Parameters.AddWithValue("descripcion_poblacion_beneficiaria", proyecto.DescripcionPoblacionBeneficiaria);
                    command.Parameters.AddWithValue("categorizacion_bien_servicio", proyecto.CaraterizacionDelBienServicio);
                    command.Parameters.AddWithValue("descripcion_sostenibilidad_proyecto", proyecto.DescripcionSostenibilidadDelProyecto);
                    command.Parameters.AddWithValue("justificacion_de_mercado", proyecto.JustificacionDeMercado);
                    command.Parameters.AddWithValue("cod_proyecto", proyecto.CodProyecto);
                    try
                    {
                        result = command.ExecuteNonQuery();
                    }
                    catch (SQLiteException)
                    {
                        return false;
                    }
                }
                conn.Close();
            }

            return result != -1;
        }

        public bool ActualizarProyectoFlujoCaja(Proyecto proyecto)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(conn))
                {
                    command.CommandText = "UPDATE PROYECTO SET tasa_costo_capital=?, " +
                "personas_participantes=?, familias_involucradas=?, " +
                "beneficiarios_indirectos=? WHERE cod_proyecto=?";
                    command.Prepare();
                    command.Parameters.AddWithValue("tasa_costo_capital", proyecto.TasaCostoCapital);
                    command.Parameters.AddWithValue("personas_participantes", proyecto.PersonasParticipantes);
                    command.Parameters.AddWithValue("familias_involucradas", proyecto.FamiliasInvolucradas);
                    command.Parameters.AddWithValue("beneficiarios_indirectos", proyecto.PersonasBeneficiadas);
                    command.Parameters.AddWithValue("cod_proyecto", proyecto.CodProyecto);
                    try
                    {
                        result = command.ExecuteNonQuery();
                    }
                    catch (SQLiteException)
                    {
                        return false;
                    }
                }
                conn.Close();
            }

            return result != -1;
        }

        public bool ArchivarProyecto(int codProyecto, bool archivar)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(conn))
                {
                    command.CommandText = "UPDATE PROYECTO SET archivado=? WHERE cod_proyecto=?";
                    command.Prepare();
                    command.Parameters.AddWithValue("archivado", archivar?1:0);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    try
                    {
                        result = command.ExecuteNonQuery();
                    }
                    catch (SQLiteException)
                    {
                        return false;
                    }
                }
                conn.Close();
            }
            return result != -1;
        }

        public List<Proyecto> GetProyectos()
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
                proyecto.HorizonteEvaluacionEnAnos = int.Parse(reader["horizonte_evaluacion_en_anos"].ToString());
                proyecto.Archivado = (bool)reader["archivado"];
                proyecto.NombreProyecto = reader["nombre_proyecto"].ToString();

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
                proyecto.HorizonteEvaluacionEnAnos = Int32.Parse(reader["horizonte_evaluacion_en_anos"].ToString());
                proyecto.JustificacionDeMercado = reader["justificacion_de_mercado"].ToString();
                proyecto.NombreProyecto = reader["nombre_proyecto"].ToString();
                proyecto.PagaImpuesto = Boolean.Parse(reader["paga_impuesto"].ToString());
                proyecto.PorcentajeImpuesto = float.Parse(reader["porcentaje_impuesto"].ToString());
                proyecto.ResumenEjecutivo = reader["resumen_ejecutivo"].ToString();
                proyecto.ConFinanciamiento = Boolean.Parse(reader["con_financiamiento"].ToString());
                proyecto.FamiliasInvolucradas = Int32.Parse(reader["familias_involucradas"].ToString());
                proyecto.TasaCostoCapital = float.Parse(reader["tasa_costo_capital"].ToString());
                proyecto.PersonasBeneficiadas = Int32.Parse(reader["beneficiarios_indirectos"].ToString());
                proyecto.PersonasParticipantes = Int32.Parse(reader["personas_participantes"].ToString());
                proyecto.ObjetoInteres = reader["objeto_interes"].ToString();
                proyecto.Archivado = (reader["archivado"] as int?).Equals(1);

                return proyecto;
            }//if
            conexion.Close();
            return proyecto;
        }
    }//ProyectoData
}
