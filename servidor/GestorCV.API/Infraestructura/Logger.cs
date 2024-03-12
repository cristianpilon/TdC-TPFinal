using Serilog;
using System;

namespace GestorCV.API.Infraestructura
{
    public class Logger
    {
        public static void ConfigureLogger()
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }
            catch
            {
                // Si ocurre un error al escribir en el log ya no hay nada para hacer.
            }
        }

        public static void LogException(Exception ex)
        {
            Log.Error(ex, "Exception caught");
        }
    }
}
