using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UCR.Negotium.Domain;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.DataAccess
{
    public class ProyectoData:BaseData
    {
        private TipoProyectoData tipoProyectoData;

        public ProyectoData()
        {
            tipoProyectoData = new TipoProyectoData();
        }

        public int InsertarProyecto(Proyecto proyecto)
        {
            int idProyecto = -1;
            object newProdID;
            string insert1 = "INSERT INTO PROYECTO(nombre_proyecto, resumen_ejecutivo, con_ingresos, "+
                "ano_inicial_proyecto, horizonte_evaluacion_en_anos, paga_impuesto, porcentaje_impuesto, "+
                "direccion_exacta, cod_provincia, cod_canton, cod_distrito, cod_evaluador, con_financiamiento, "+
                "objeto_interes, archivado, cod_tipo_proyecto, cod_tipo_moneda) " +
                "VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); SELECT last_insert_rowid();";

            string insert2 = "INSERT INTO PROYECTO_INDICE(cod_proyecto, guid_proyecto) " +
                "VALUES(?,?)";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                SQLiteTransaction transaction = null;

                try
                {
                    conn.Open();
                    SQLiteCommand command1 = new SQLiteCommand(insert1, conn);
                    SQLiteCommand command2 = new SQLiteCommand(insert2, conn);
                    command1.Parameters.AddWithValue("nombre_proyecto", proyecto.NombreProyecto);
                    command1.Parameters.AddWithValue("resumen_ejecutivo", proyecto.ResumenEjecutivo);
                    command1.Parameters.AddWithValue("con_ingresos", proyecto.ConIngresos ? 1 : 0);
                    command1.Parameters.AddWithValue("ano_inicial_proyecto", proyecto.AnoInicial);
                    command1.Parameters.AddWithValue("horizonte_evaluacion_en_anos", proyecto.HorizonteEvaluacionEnAnos);
                    command1.Parameters.AddWithValue("paga_impuesto", proyecto.PagaImpuesto);
                    command1.Parameters.AddWithValue("porcentaje_impuesto", proyecto.PorcentajeImpuesto);
                    command1.Parameters.AddWithValue("direccion_exacta", proyecto.DireccionExacta);
                    command1.Parameters.AddWithValue("cod_provincia", proyecto.Provincia.CodProvincia);
                    command1.Parameters.AddWithValue("cod_canton", proyecto.Canton.CodCanton);
                    command1.Parameters.AddWithValue("cod_distrito", proyecto.Distrito.CodDistrito);
                    command1.Parameters.AddWithValue("cod_evaluador", proyecto.Encargado.IdEncargado);
                    command1.Parameters.AddWithValue("con_financiamiento", proyecto.ConFinanciamiento);
                    command1.Parameters.AddWithValue("objeto_interes", proyecto.ObjetoInteres);
                    command1.Parameters.AddWithValue("archivado", proyecto.Archivado);
                    command1.Parameters.AddWithValue("cod_tipo_proyecto", proyecto.TipoProyecto.CodTipo);
                    command1.Parameters.AddWithValue("cod_tipo_moneda", proyecto.TipoMoneda.CodMoneda);

                    transaction = conn.BeginTransaction();

                    newProdID = command1.ExecuteScalar();
                    idProyecto = int.Parse(newProdID.ToString());

                    command2.Parameters.AddWithValue("cod_proyecto", idProyecto);
                    command2.Parameters.AddWithValue("guid_proyecto", Guid.NewGuid().ToString());
                    command2.ExecuteScalar();
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    idProyecto = -1;
                    transaction.Rollback();
                }
            }

            return idProyecto;
        }

        public bool EditarProyecto(Proyecto proyecto)
        {
            int result = -1;
            string update = "UPDATE PROYECTO SET nombre_proyecto=?, ano_inicial_proyecto=?, " +
                "horizonte_evaluacion_en_anos=?, resumen_ejecutivo=?, con_ingresos=?, " +
                "paga_impuesto=?, porcentaje_impuesto=?, direccion_exacta=?, cod_provincia=?, cod_canton=?, " +
                "cod_distrito=?, con_financiamiento=?, objeto_interes=?, cod_tipo_proyecto=? WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update, conn);
                    command.Parameters.AddWithValue("nombre_proyecto", proyecto.NombreProyecto);
                    command.Parameters.AddWithValue("ano_inicial_proyecto", proyecto.AnoInicial);
                    command.Parameters.AddWithValue("horizonte_evaluacion_en_anos", proyecto.HorizonteEvaluacionEnAnos);
                    command.Parameters.AddWithValue("resumen_ejecutivo", proyecto.ResumenEjecutivo);
                    command.Parameters.AddWithValue("con_ingresos", proyecto.ConIngresos ? 1 : 0);
                    command.Parameters.AddWithValue("paga_impuesto", proyecto.PagaImpuesto ? 1 : 0);
                    command.Parameters.AddWithValue("porcentaje_impuesto", proyecto.PorcentajeImpuesto);
                    command.Parameters.AddWithValue("direccion_exacta", proyecto.DireccionExacta);
                    command.Parameters.AddWithValue("cod_provincia", proyecto.Provincia.CodProvincia);
                    command.Parameters.AddWithValue("cod_canton", proyecto.Canton.CodCanton);
                    command.Parameters.AddWithValue("cod_distrito", proyecto.Distrito.CodDistrito);
                    command.Parameters.AddWithValue("con_financiamiento", proyecto.ConFinanciamiento ? 1 : 0);
                    command.Parameters.AddWithValue("objeto_interes", proyecto.ObjetoInteres);
                    command.Parameters.AddWithValue("cod_tipo_proyecto", proyecto.TipoProyecto.CodTipo);
                    command.Parameters.AddWithValue("cod_proyecto", proyecto.CodProyecto);

                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        public bool EditarProyectoCaracterizacion(Proyecto proyecto)
        {
            int result = -1;
            string update = "UPDATE PROYECTO SET descripcion_poblacion_beneficiaria=?, categorizacion_bien_servicio=?, " +
                "descripcion_sostenibilidad_proyecto=?, justificacion_de_mercado=? WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update, conn);
                    command.Parameters.AddWithValue("descripcion_poblacion_beneficiaria", proyecto.DescripcionPoblacionBeneficiaria);
                    command.Parameters.AddWithValue("categorizacion_bien_servicio", proyecto.CaraterizacionDelBienServicio);
                    command.Parameters.AddWithValue("descripcion_sostenibilidad_proyecto", proyecto.DescripcionSostenibilidadDelProyecto);
                    command.Parameters.AddWithValue("justificacion_de_mercado", proyecto.JustificacionDeMercado);
                    command.Parameters.AddWithValue("cod_proyecto", proyecto.CodProyecto);

                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        public bool EditarProyectoFlujoCaja(Proyecto proyecto)
        {
            int result = -1;
            string update = "UPDATE PROYECTO SET tasa_costo_capital=?, personas_participantes=?, " +
                "familias_involucradas=?, beneficiarios_indirectos=? WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update, conn);
                    command.Parameters.AddWithValue("tasa_costo_capital", proyecto.TasaCostoCapital);
                    command.Parameters.AddWithValue("personas_participantes", proyecto.PersonasParticipantes);
                    command.Parameters.AddWithValue("familias_involucradas", proyecto.FamiliasInvolucradas);
                    command.Parameters.AddWithValue("beneficiarios_indirectos", proyecto.PersonasBeneficiadas);
                    command.Parameters.AddWithValue("cod_proyecto", proyecto.CodProyecto);

                    result = command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }

            return result != -1;
        }

        public bool ArchivarProyecto(int codProyecto, bool archivar)
        {
            int result = -1;
            string update = "UPDATE PROYECTO SET archivado=? WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update, conn);
                    command.Parameters.AddWithValue("archivado", archivar ? 1 : 0);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    result = command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    result = -1;
                }
            }
            return result != -1;
        }

        public bool EditarMoneda(int codProyecto, int codMoneda)
        {
            int result = -1;
            string update = "UPDATE PROYECTO SET cod_tipo_moneda=? WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(update, conn);
                    command.Parameters.AddWithValue("cod_tipo_moneda", codMoneda);
                    command.Parameters.AddWithValue("cod_proyecto", codProyecto);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ex.TraceExceptionAsync();
                    return false;
                }
            }

            return result != -1;
        }

        public List<Proyecto> GetProyectos()
        {
            OrganizacionProponenteData orgProponenteData = new OrganizacionProponenteData();
            List<TipoProyecto> tiposProyectos = tipoProyectoData.GetTipoProyectos();
            List<Proyecto> proyectos = new List<Proyecto>();

            string select = "SELECT p.cod_proyecto, p.ano_inicial_proyecto, p.horizonte_evaluacion_en_anos, " +
                "p.archivado, p.nombre_proyecto, p.cod_tipo_proyecto FROM PROYECTO p";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Proyecto proyecto = new Proyecto();
                            proyecto.CodProyecto = int.Parse(reader[0].ToString());
                            proyecto.AnoInicial = int.Parse(reader[1].ToString());
                            proyecto.HorizonteEvaluacionEnAnos = int.Parse(reader[2].ToString());
                            proyecto.Archivado = (bool)reader[3];
                            proyecto.NombreProyecto = reader[4].ToString();

                            proyecto.TipoProyecto = tiposProyectos.Find(
                                tipoproy => tipoproy.CodTipo.Equals(int.Parse(reader[5].ToString())));

                            proyecto.OrganizacionProponente = orgProponenteData.GetOrganizacionProponente(proyecto.CodProyecto);

                            proyectos.Add(proyecto);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    proyectos = new List<Proyecto>();
                }
            }

            return proyectos;
        }
        
        public Proyecto GetProyecto(int codProyecto)
        {
            Proyecto proyecto = new Proyecto();
            string select = "SELECT * FROM PROYECTO WHERE cod_proyecto=?";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            proyecto.CodProyecto = codProyecto;
                            proyecto.AnoInicial = int.Parse(reader["ano_inicial_proyecto"].ToString());
                            proyecto.Canton.CodCanton = int.Parse(reader["cod_canton"].ToString());
                            proyecto.Provincia.CodProvincia = int.Parse(reader["cod_provincia"].ToString());
                            proyecto.Distrito.CodDistrito = int.Parse(reader["cod_distrito"].ToString());
                            proyecto.Encargado.IdEncargado = int.Parse(reader["cod_evaluador"].ToString());
                            proyecto.CaraterizacionDelBienServicio = reader["categorizacion_bien_servicio"].ToString();
                            proyecto.CodProyecto = int.Parse(reader["cod_proyecto"].ToString());
                            proyecto.ConIngresos = bool.Parse(reader["con_ingresos"].ToString());
                            proyecto.DescripcionPoblacionBeneficiaria = reader["descripcion_poblacion_beneficiaria"].ToString();
                            proyecto.DescripcionSostenibilidadDelProyecto = reader["descripcion_sostenibilidad_proyecto"].ToString();
                            proyecto.DireccionExacta = reader["direccion_exacta"].ToString();
                            proyecto.Distrito.CodDistrito = int.Parse(reader["cod_distrito"].ToString());
                            proyecto.HorizonteEvaluacionEnAnos = int.Parse(reader["horizonte_evaluacion_en_anos"].ToString());
                            proyecto.JustificacionDeMercado = reader["justificacion_de_mercado"].ToString();
                            proyecto.NombreProyecto = reader["nombre_proyecto"].ToString();
                            proyecto.PagaImpuesto = bool.Parse(reader["paga_impuesto"].ToString());
                            proyecto.PorcentajeImpuesto = float.Parse(reader["porcentaje_impuesto"].ToString());
                            proyecto.ResumenEjecutivo = reader["resumen_ejecutivo"].ToString();
                            proyecto.ConFinanciamiento = bool.Parse(reader["con_financiamiento"].ToString());
                            proyecto.FamiliasInvolucradas = int.Parse(reader["familias_involucradas"].ToString());
                            proyecto.TasaCostoCapital = float.Parse(reader["tasa_costo_capital"].ToString());
                            proyecto.PersonasBeneficiadas = int.Parse(reader["beneficiarios_indirectos"].ToString());
                            proyecto.PersonasParticipantes = int.Parse(reader["personas_participantes"].ToString());
                            proyecto.ObjetoInteres = reader["objeto_interes"].ToString();
                            proyecto.Archivado = (reader["archivado"] as int?).Equals(1);
                            proyecto.TipoProyecto = tipoProyectoData.GetTipoProyecto(int.Parse(reader["cod_tipo_proyecto"].ToString()));
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    proyecto = new Proyecto();
                }
            }

            return proyecto;
        }

        public TipoMoneda GetMonedaProyecto(int codProyecto)
        {
            TipoMoneda tipoMoneda = new TipoMoneda();
            string select = "SELECT tp.* FROM TIPO_MONEDA tp, PROYECTO p " +
                "WHERE p.cod_proyecto=? AND tp.cod_tipo_moneda=p.cod_tipo_moneda";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tipoMoneda.CodMoneda = int.Parse(reader["cod_tipo_moneda"].ToString());
                            tipoMoneda.NombreMoneda = reader["nombre_moneda"].ToString();
                            tipoMoneda.SignoMoneda = reader["signo_moneda"].ToString();
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    tipoMoneda = new TipoMoneda();
                }
            }

            return tipoMoneda;
        }

        public string GetSignoMonedaProyecto(int codProyecto)
        {
            string signo = "";
            string select = "SELECT tp.signo_moneda FROM TIPO_MONEDA tp, PROYECTO p "+
                "WHERE p.cod_proyecto=? AND tp.cod_tipo_moneda=p.cod_tipo_moneda";

            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(select, conn);
                    cmd.Parameters.AddWithValue("cod_proyecto", codProyecto);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            signo = reader[0].ToString();
                        }
                    }
                }
                catch(Exception ex)
                {
                    ex.TraceExceptionAsync();
                    signo = "";
                }
            }

            return signo;
        }
    }
}
