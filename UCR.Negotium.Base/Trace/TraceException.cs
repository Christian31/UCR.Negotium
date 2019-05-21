using System;
using System.IO;
using System.Threading.Tasks;

namespace UCR.Negotium.Base.Trace
{
    public static class TraceException
    {
        public static void TraceExceptionAsync(this Exception exception)
        {
            Task.Run(() => TraceExcepcion(exception));
        }

        private static void TraceExcepcion(this Exception exception)
        {
            try
            {
                string laFechaActual = DateTime.Now.ToString();
                string laRutaDelLog = ObtengaElLogDelDia();
                EscribaMensaje(laRutaDelLog, laFechaActual, exception.Message, exception.ToString());
            }
            catch(Exception ex)
            {
                //se omite lanzar la excepción
            }
        }

        private static void EscribaMensaje(string path, string fechaDelMensaje, string mensaje, string detalle)
        {
            string nuevoMensaje = string.Format(templateMessages, fechaDelMensaje, mensaje, detalle) + separadorDeMensajes;
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write(nuevoMensaje);
            }
        }

        private static string ObtengaElLogDelDia()
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            string nombreDelLog = $"mensajes-errores.log";
            string fullLogPath = Path.Combine(logPath, nombreDelLog);

            return fullLogPath;
        }

        private const string templateMessages = "Fecha de error: {0}, \n Mensaje: {1}, \n Detalle: {2}. \n";
        private const string separadorDeMensajes = "/*****************************************************/ \n";
    }
}
