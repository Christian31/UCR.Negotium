using log4net;
using log4net.Config;
using System;
using System.Text;
using System.Threading.Tasks;

namespace UCR.Negotium.Domain.Tracing
{
    public static class TraceException
    {
        private static ILog log = LogManager.GetLogger(typeof(TraceException));
        private const string LOGFIENAME = @"{0}logs\log_{1}";

        public static void TraceExceptionAsync(this Exception exception)
        {
            string newFileName = string.Format(LOGFIENAME, AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));
            if (Convert.ToString(GlobalContext.Properties["LogFileName"]) != newFileName)
            {
                ConfigureTracing(newFileName);
            }

            Task.Run(() => TraceLog(exception));
        }

        private static async void TraceLog(Exception e)
        {
            StringBuilder s = new StringBuilder();
            while (e != null)
            {
                s.AppendLine();
                s.AppendLine("Exception type: " + e.GetType().FullName);
                s.AppendLine("Message       : " + e.Message);
                s.AppendLine("Stacktrace:");
                s.AppendLine(e.StackTrace);
                s.AppendLine();
                e = e.InnerException;
            }

            log.Error(s.ToString());
        }

        private static void ConfigureTracing(string logFileName)
        {
            GlobalContext.Properties["LogFileName"] = logFileName;
            XmlConfigurator.Configure();
        }
    }
}
