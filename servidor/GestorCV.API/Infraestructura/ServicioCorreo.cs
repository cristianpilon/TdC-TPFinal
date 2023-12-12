using System.Net.Mail;
using System.Net;
using System;

namespace GestorCV.API.Infraestructura
{
    public sealed class ServicioCorreo
    {
        private static readonly ServicioCorreo _instancia = new();

        private ServicioCorreo()
        {
        }

        public static ServicioCorreo Instancia
        {
            get
            {
                return _instancia;
            }
        }

        public void EnviarCorreo(string correoOrigen, string correoDestino, string asunto, string cuerpoMensaje)
        {
            var mailMessage = new MailMessage(correoOrigen, correoDestino, asunto, cuerpoMensaje)
            {
                IsBodyHtml = true
            };

            var smtpClient = new SmtpClient(AppConfiguration.SmtpHost, Convert.ToInt32(AppConfiguration.SmtpPuerto))
            {
                Credentials = new NetworkCredential(AppConfiguration.SmtpUsuario, AppConfiguration.SmtpPassword),
                EnableSsl = true
            };

            smtpClient.Send(mailMessage);
        }
    }
}
