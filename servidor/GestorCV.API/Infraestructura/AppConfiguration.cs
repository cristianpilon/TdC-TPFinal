using Microsoft.Extensions.Configuration;

namespace GestorCV.API.Infraestructura
{
    public class AppConfiguration
    {
        private static IConfiguration _config;

        public AppConfiguration(IConfiguration config)
        {
            _config = config ;
        }

        public static string ConnString => _config.GetConnectionString("GestorCvContext");
        
        public static string ClaveToken => _config.GetSection("Jwt:Clave").Get<string>();
        
        public static string FirmaToken => _config.GetSection("Jwt:Firma").Get<string>();

        public static string SemillaPassword => _config.GetSection("Password:Semilla").Get<string>();

        public static string SmtpHost => _config.GetSection("CorreoSMTP:Host").Get<string>();

        public static string SmtpPuerto => _config.GetSection("CorreoSMTP:Puerto").Get<string>();
        
        public static string SmtpCorreoOrigen => _config.GetSection("CorreoSMTP:CorreoOrigen").Get<string>();

        public static string SmtpUsuario => _config.GetSection("CorreoSMTP:Usuario").Get<string>();

        public static string SmtpPassword => _config.GetSection("CorreoSMTP:Password").Get<string>();

    }
}
