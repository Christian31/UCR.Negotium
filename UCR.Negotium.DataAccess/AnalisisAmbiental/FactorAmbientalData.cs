using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using UCR.Negotium.DataAccess.Enums;
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

        public List<Tuple<int, string>> GetSignos()
        {
            return EnumsHelper<Signo>.ToTupleList();
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

        //public List<FactorAmbiental> GetFactores(int codProyecto)
        //{
        //    List<FactorAmbiental> factores = new List<FactorAmbiental>();
        //    try
        //    {
        //        using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
        //        {
        //            conn.Open();
        //            string sql = "SELECT * FROM FACTOR_AMBIENTAL WHERE cod_proyecto = " + codProyecto;
        //            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        //            {
        //                using (SQLiteDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        FactorAmbiental factor = new FactorAmbiental();
        //                        factor.CodFactorAmbiental = int.Parse(reader["cod_factor"].ToString());
        //                        factor.NombreFactor = reader["nombre_factor"].ToString();
        //                        factor.CondicionAfectada = (CondicionAfectada)reader["condicion_afectada"];
        //                        factor.ElementoAmbiental = Convert.ToInt32(reader["elemento_ambiental"].ToString());
        //                        factor.Clasificacion = (Clasificacion)reader["clasificacion"];
        //                        factores.Add(factor);
        //                    }
        //                }
        //            }
        //            conn.Close();
        //        }
        //    }
        //    catch (SQLiteException e)
        //    {
        //        return factores;
        //    }
        //    return factores;
        //}

        public FactorAmbiental GetFactor(int codFactor)
        {
            FactorAmbiental factor = new FactorAmbiental();
            //factor.ElementoAmbiental = (Clasificacion)reader["clasificacion"];
            //factor.Signo = (Signo)reader["clasificacion"];
            //factor.Intensidad = (Intensidad)reader["clasificacion"];
            //factor.Extension = (Extension)reader["clasificacion"];
            //factor.Momento = (Momento)reader["clasificacion"];
            //factor.Persistencia = (Persistencia)reader["clasificacion"];
            //factor.Reversibilidad = (Reversibilidad)reader["clasificacion"];
            //factor.Sinergia = (Sinergia)reader["clasificacion"];
            //factor.Acumulacion = (Acumulacion)reader["clasificacion"];
            //factor.Efecto = (Efecto)reader["clasificacion"];
            //factor.Periodicidad = (Periodicidad)reader["clasificacion"];
            //factor.Recuperabilidad = (Recuperabilidad)reader["clasificacion"];
            return factor;
        }

        //public bool InsertarFactorAmbiental(FactorAmbiental factor)
        //{
        //    int result = -1;
        //    using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
        //    {
        //        conn.Open();
        //        using (SQLiteCommand cmd = new SQLiteCommand(conn))
        //        {
        //            cmd.CommandText = "INSERT INTO FACTOR_AMBIENTAL(cod_proyecto, nombre_factor, condicion_afectada"+
        //                "elemento_ambiental)" +
        //                "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
        //            cmd.Prepare();
        //            cmd.Parameters.AddWithValue("cod_proyecto", factor.CodProyecto);
        //            cmd.Parameters.AddWithValue("nombre_factor", factor.NombreFactor);
        //            cmd.Parameters.AddWithValue("condicion_afectada", factor.CondicionAfectada);
        //            cmd.Parameters.AddWithValue("elemento_ambiental", factor.ElementoAmbiental);

        //            try
        //            {
        //                result = cmd.ExecuteNonQuery();
        //            }
        //            catch (SQLiteException e)
        //            {
        //                return false;
        //            }
        //        }
        //        conn.Close();
        //    }
        //    return result != -1;
        //}

        ////TODDO terminar editar
        //public bool EditarFactorAmbiental(FactorAmbiental factor)
        //{
        //    int result = -1;
        //    using (SQLiteConnection conn = new SQLiteConnection(cadenaConexion))
        //    {
        //        conn.Open();
        //        using (SQLiteCommand cmd = new SQLiteCommand(conn))
        //        {
        //            cmd.CommandText = "UPDATE FACTOR_AMBIENTAL "
        //                + "SET nombre_factor=?,  "
        //                + "WHERE cod_factor=?";
        //            cmd.Prepare();
        //            cmd.Parameters.AddWithValue("nombre_factor", factor.NombreFactor);
        //            cmd.Parameters.AddWithValue("cod_factor", factor.CodFactorAmbiental);
        //            try
        //            {
        //                result = cmd.ExecuteNonQuery();
        //            }
        //            catch (SQLiteException)
        //            {
        //                return false;
        //            }
        //        }
        //        conn.Close();
        //    }

        //    return result != -1;
        //}
    }
}
