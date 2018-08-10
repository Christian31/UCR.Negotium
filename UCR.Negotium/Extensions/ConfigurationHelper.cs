namespace UCR.Negotium.Extensions
{
    public static class ConfigurationHelper
    {
        private const string USOACADEMICO = "ACADEMICO";

        public static string GetCurrentVersion()
        {
            return System.Configuration.ConfigurationManager.AppSettings["versionDb"];
        }

        public static bool EsDeUsoAcademico()
        {
            bool esAcademico;
            string valorSetting = System.Configuration.ConfigurationManager.AppSettings["tipoUso"];
            esAcademico = valorSetting == USOACADEMICO;
            return esAcademico;
        }
    }
}
