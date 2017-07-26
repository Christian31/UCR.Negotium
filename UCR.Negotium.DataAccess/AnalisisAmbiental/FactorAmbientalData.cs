using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using UCR.Negotium.Base.Enums;
using UCR.Negotium.Domain;

namespace UCR.Negotium.DataAccess.AnalisisAmbiental
{
    public class FactorAmbientalData
    {
        private string cadenaConexion;

        public FactorAmbientalData()
        {
            cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["db"].
                ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
        }

        #region GetEnumValues
        public List<Tuple<int, string, List<Tuple<int, string>>>> GetCondicionesAfectadas()
        {
            List<Tuple<int, string, List<Tuple<int, string>>>> condicionesAfectadas = new List<Tuple<int, string, List<Tuple<int, string>>>>();
            var values = Enum.GetValues(typeof(CondicionAfectada)).Cast<CondicionAfectada>();
            int i = 1;
            foreach(var value in values)
            {
                string displayValue = EnumsHelper<CondicionAfectada>.GetDisplayValue(value);
                condicionesAfectadas.Add(Tuple.Create((int)value, displayValue, GetElementosAmbiental(i)));
                i++;
            }
            return condicionesAfectadas;
        }

        private List<Tuple<int, string>> GetElementosAmbiental(int condicion)
        {
            switch (condicion)
            {
                case 1:
                    return EnumsHelper<ElementoAmbientalCond1>.ToTupleList();
                case 2:
                    return EnumsHelper<ElementoAmbientalCond2>.ToTupleList();
                case 3:
                    return EnumsHelper<ElementoAmbientalCond3>.ToTupleList();
                case 4:
                    return EnumsHelper<ElementoAmbientalCond4>.ToTupleList();
                case 5:
                    return EnumsHelper<ElementoAmbientalCond5>.ToTupleList();
                default:
                    return EnumsHelper<ElementoAmbientalCond1>.ToTupleList();
            }
        }

        public List<Tuple<int, string>> GetClasificaciones()
        {
            return EnumsHelper<Clasificacion>.ToTupleList();
        }

        public List<Tuple<int, string>> GetIntensidades()
        {
            return EnumsHelper<Intensidad>.ToTupleList();
        }

        public List<Tuple<int, string>> GetExtensiones()
        {
            return EnumsHelper<Extension>.ToTupleList();
        }

        public List<Tuple<int, string>> GetMomentos()
        {
            return EnumsHelper<Momento>.ToTupleList();
        }

        public List<Tuple<int, string>> GetPersistencias()
        {
            return EnumsHelper<Persistencia>.ToTupleList();
        }

        public List<Tuple<int, string>> GetReversibilidades()
        {
            return EnumsHelper<Reversibilidad>.ToTupleList();
        }

        public List<Tuple<int, string>> GetSinergias()
        {
            return EnumsHelper<Sinergia>.ToTupleList();
        }

        public List<Tuple<int, string>> GetAcumulaciones()
        {
            return EnumsHelper<Acumulacion>.ToTupleList();
        }

        public List<Tuple<int, string>> GetEfectos()
        {
            return EnumsHelper<Efecto>.ToTupleList();
        }

        public List<Tuple<int, string>> GetPeriodicidades()
        {
            return EnumsHelper<Periodicidad>.ToTupleList();
        }

        public List<Tuple<int, string>> GetRecuperabilidades()
        {
            return EnumsHelper<Recuperabilidad>.ToTupleList();
        }
        #endregion

        public List<FactorAmbiental> GetFactores(int codProyecto)
        {
            List<FactorAmbiental> factores = new List<FactorAmbiental>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
                {
                    conn.Open();
                    string sql = "SELECT cod_factor, nombre_factor, condicion_afectada, elemento_ambiental, "+
                        "clasificacion, signo FROM FACTOR_AMBIENTAL WHERE cod_proyecto = " + codProyecto +";";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FactorAmbiental factor = new FactorAmbiental();
                                factor.CodFactorAmbiental = reader.GetInt32(0);
                                factor.NombreFactor = reader.GetString(1);
                                factor.CodCondicionAfectada = reader.GetInt16(2);
                                factor.CodElementoAmbiental = reader.GetInt16(3);
                                factor.CodClasificacion = reader.GetInt16(4);
                                factor.Signo = reader.GetBoolean(5);

                                factores.Add(factor);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (SQLiteException e)
            {
                return factores;
            }
            return factores;
        }

        public FactorAmbiental GetFactor(int codFactor)
        {
            FactorAmbiental factor = new FactorAmbiental();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
                {
                    conn.Open();
                    string sql = "SELECT * FROM FACTOR_AMBIENTAL WHERE cod_factor = " + codFactor + ";";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                factor.CodFactorAmbiental = codFactor;
                                factor.NombreFactor = reader.GetString(2);
                                factor.CodCondicionAfectada = reader.GetInt16(3);
                                factor.CodElementoAmbiental = reader.GetInt16(4);
                                factor.CodClasificacion = reader.GetInt16(5);
                                factor.Signo = reader.GetBoolean(6);
                                factor.CodIntensidad = reader.GetInt16(7);
                                factor.CodExtension = reader.GetInt16(8);
                                factor.ExtensionCritico = reader.GetBoolean(9);
                                factor.CodMomento = reader.GetInt16(10);
                                factor.MomentoCritico = reader.GetBoolean(11);
                                factor.CodPersistencia = reader.GetInt16(12);
                                factor.CodReversibilidad = reader.GetInt16(13);
                                factor.CodSinergia = reader.GetInt16(14);
                                factor.CodAcumulacion = reader.GetInt16(15);
                                factor.CodEfecto = reader.GetInt16(16);
                                factor.CodPeriodicidad = reader.GetInt16(17);
                                factor.CodRecuperabilidad = reader.GetInt16(18);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (SQLiteException e)
            {
                return factor;
            }
            return factor;
        }

        public FactorAmbiental InsertarFactorAmbiental(FactorAmbiental factor)
        {
            object codFactorObject;
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO FACTOR_AMBIENTAL(cod_proyecto, nombre_factor, condicion_afectada, " +
                        "elemento_ambiental, clasificacion, signo, intensidad, extension, extension_critico, " +
                        "momento, momento_critico, persistencia, reversibilidad, sinergia, acumulacion, efecto, "+
                        "periodicidad, recuperabilidad) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); "+
                        "SELECT last_insert_rowid();";

                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("cod_proyecto", factor.CodProyecto);
                    cmd.Parameters.AddWithValue("nombre_factor", factor.NombreFactor);
                    cmd.Parameters.AddWithValue("condicion_afectada", factor.CodCondicionAfectada);
                    cmd.Parameters.AddWithValue("elemento_ambiental", factor.CodElementoAmbiental);
                    cmd.Parameters.AddWithValue("clasificacion", factor.CodClasificacion);
                    cmd.Parameters.AddWithValue("signo", factor.Signo?1:0);
                    cmd.Parameters.AddWithValue("intensidad", factor.CodIntensidad);
                    cmd.Parameters.AddWithValue("extension", factor.CodProyecto);
                    cmd.Parameters.AddWithValue("extension_critico", factor.ExtensionCritico? 1:0);
                    cmd.Parameters.AddWithValue("momento", factor.CodMomento);
                    cmd.Parameters.AddWithValue("momento_critico", factor.MomentoCritico? 1:0);
                    cmd.Parameters.AddWithValue("persistencia", factor.CodPersistencia);
                    cmd.Parameters.AddWithValue("reversibilidad", factor.CodReversibilidad);
                    cmd.Parameters.AddWithValue("sinergia", factor.CodSinergia);
                    cmd.Parameters.AddWithValue("acumulacion", factor.CodAcumulacion);
                    cmd.Parameters.AddWithValue("efecto", factor.CodEfecto);
                    cmd.Parameters.AddWithValue("periodicidad", factor.CodPeriodicidad);
                    cmd.Parameters.AddWithValue("recuperalidad", factor.CodRecuperabilidad);

                    try
                    {
                        codFactorObject = cmd.ExecuteScalar();
                        factor.CodFactorAmbiental = int.Parse(codFactorObject.ToString());

                        return factor;
                    }//try
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return factor;
                    }//catch

                    finally { conn.Close(); }
                }
            }
        }

        public bool EditarFactorAmbiental(FactorAmbiental factor)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE FACTOR_AMBIENTAL SET nombre_factor=?, condicion_afectada=?, "+
                        "elemento_ambiental=?, clasificacion=?, signo=?, intensidad=?, extension=?, "+
                        "extension_critico=?, momento=?, momento_critico=?, persistencia=?, reversibilidad=?, "+
                        "sinergia=?, acumulacion=?, efecto=?, periodicidad=?, recuperabilidad=? "
                        + "WHERE cod_factor=?";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("nombre_factor", factor.NombreFactor);
                    cmd.Parameters.AddWithValue("condicion_afectada", factor.CodCondicionAfectada);
                    cmd.Parameters.AddWithValue("elemento_ambiental", factor.CodElementoAmbiental);
                    cmd.Parameters.AddWithValue("clasificacion", factor.CodClasificacion);
                    cmd.Parameters.AddWithValue("signo", factor.Signo?1:0);
                    cmd.Parameters.AddWithValue("intensidad", factor.CodIntensidad);
                    cmd.Parameters.AddWithValue("extension", factor.CodExtension);
                    cmd.Parameters.AddWithValue("extension_critico", factor.ExtensionCritico?1:0);
                    cmd.Parameters.AddWithValue("momento", factor.CodMomento);
                    cmd.Parameters.AddWithValue("momento_critico", factor.MomentoCritico?1:0);
                    cmd.Parameters.AddWithValue("persistencia", factor.CodPersistencia);
                    cmd.Parameters.AddWithValue("reversibilidad", factor.CodReversibilidad);
                    cmd.Parameters.AddWithValue("sinergia", factor.CodSinergia);
                    cmd.Parameters.AddWithValue("acumulacion", factor.CodAcumulacion);
                    cmd.Parameters.AddWithValue("efecto", factor.CodEfecto);
                    cmd.Parameters.AddWithValue("periodicidad", factor.CodPeriodicidad);
                    cmd.Parameters.AddWithValue("recuperabilidad", factor.CodRecuperabilidad);
                    cmd.Parameters.AddWithValue("cod_factor", factor.CodFactorAmbiental);

                    try
                    {
                        result = cmd.ExecuteNonQuery();
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

        public bool EliminarFactorAmbiental(int codFactor)
        {
            string sqlQuery = "DELETE FROM FACTOR_AMBIENTAL WHERE cod_factor =" + codFactor + ";";
            using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();
                    command.CommandText = sqlQuery;

                    return command.ExecuteNonQuery() != 0;
                }
                catch
                {
                    return false;
                }
                finally { conn.Close(); }
            }
        }
    }
}
