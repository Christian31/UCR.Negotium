using NLog;
using System;
using System.Threading.Tasks;

namespace UCR.Negotium.Base.Trace
{
    public static class TraceException
    {
        public static void TraceExceptionAsync(this Exception exception)
        {
            Task.Run(() => TraceLog(exception));
        }

        private static async void TraceLog(Exception ex)
        {
            Logger logger = LogManager.GetLogger("fileLogger");
            logger.Error(ex, "Error Inesperado");
        }
    }
}
