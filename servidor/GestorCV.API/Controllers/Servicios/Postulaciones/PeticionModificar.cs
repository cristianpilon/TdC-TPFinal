using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Infraestructura;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Web;

namespace GestorCV.API.Controllers.Servicios.Postulaciones
{
    public class PeticionModificar: PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        private readonly IRepositorioNotificaciones repositorioNotificaciones;

        public PeticionModificar(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
            repositorioNotificaciones = new RepositorioNotificaciones();
        }

        public override IResultado Procesar()
        {
            var respuestaModificarPostulacion = ((IRepositorioPostulaciones)Repositorio).Modificar(ParametrosPeticion.IdPostulacion, ParametrosPeticion.Estado);

            EnviarNotificacion(respuestaModificarPostulacion, ParametrosPeticion.Estado);
            EnviarCorreoUsuario(respuestaModificarPostulacion, ParametrosPeticion.Estado);

            return new Resultado();
        }

        private void EnviarCorreoUsuario(RepositorioPostulaciones.RespuestaModificarPostulacion respuestaModificarPostulacion, string nuevoEstado)
        {
            string mensajeNotificacion = string.Empty;

            switch (nuevoEstado)
            {
                case "Revisado":
                    mensajeNotificacion = $"El equipo técnico del empleo '{HttpUtility.HtmlEncode(respuestaModificarPostulacion.TituloEmpleo)}' se encuentra revisando su candidatura. En breve la organización le notificará los resultados. <br /><br />Muchas Gracias";
                    break;
                case "Descartado":
                    mensajeNotificacion = $"Lamentamos informarle que el equipo técnico del empleo '{HttpUtility.HtmlEncode(respuestaModificarPostulacion.TituloEmpleo)}' ha decidido descartar su candidatura. <br />De todas formas, no te aflijas y no dejes de intentarlo. Confía siempre en tí mismo. <br /><br />Muchas Gracias";
                    break;
                case "Entrevista":
                    mensajeNotificacion = $"El equipo técnico del empleo '{HttpUtility.HtmlEncode(respuestaModificarPostulacion.TituloEmpleo)}' ha decidido continuar con las entrevistas. En breve, algún miembro de la organización se pondrá en contacto con usted para proceder. <br /><br />¡Mucha Suerte!";
                    break;
                case "Finalizado":
                    mensajeNotificacion = $" ¡Felicitaciones! El equipo técnico del empleo '{HttpUtility.HtmlEncode(respuestaModificarPostulacion.TituloEmpleo)}' ha aprobado su entrevista. <br /><br />¡Mucha Suerte en esta nueva etapa!";
                    break;
            }

            ServicioCorreo.Instancia.EnviarCorreo(
                AppConfiguration.SmtpCorreoOrigen,
                respuestaModificarPostulacion.UsuarioCorreo,
                $"{respuestaModificarPostulacion.UsuarioNombre} - Nuevo estado del empleo '{HttpUtility.HtmlEncode(respuestaModificarPostulacion.TituloEmpleo)}'",
                $"Estimad@ {HttpUtility.HtmlEncode(respuestaModificarPostulacion.UsuarioNombre)},<br /><br />{mensajeNotificacion}");
        }

        private void EnviarNotificacion(RepositorioPostulaciones.RespuestaModificarPostulacion respuestaModificarPostulacion, string estado)
        {
            var mensaje = $"Su postulación a <strong>{HttpUtility.HtmlEncode(respuestaModificarPostulacion.TituloEmpleo)}</strong> ha pasado a estado <strong>{estado}</strong>.";

            repositorioNotificaciones.Agregar(new Models.Dtos.Notificacion(respuestaModificarPostulacion.UsuarioId, mensaje));
        }

        public class Resultado : IResultado
        {
        }

        public class Parametros
        {
            public int IdPostulacion { get; set; }

            public string Estado { get; set; }
        }
    }
}
