using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GestorCV.API.Infraestructura.Seguridad
{
    public class InterceptorAuditoriaAcciones
    {
        private readonly RequestDelegate _next;

        public InterceptorAuditoriaAcciones(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext contexto)
        {
            try
            {
                var request = contexto.Request;

                string cuerpoMensage = string.Empty;
                string parametrosQuery = $"(Parámetros query: {request.QueryString})";

                if (request.Body != null)
                {
                    using (var bodyReader = new StreamReader(request.Body))
                    {
                        request.EnableBuffering();
                        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                        await request.Body.ReadAsync(buffer, 0, buffer.Length);

                        // Rebobinar el flujo para que esté disponible para los controladores posteriores
                        request.Body.Position = 0;

                        cuerpoMensage = $"(Cuerpo mensaje: {Encoding.UTF8.GetString(buffer)})";
                    }
                }

                string ruta = $"{request.Path} {parametrosQuery} {cuerpoMensage}";

                string accion = request.Method;

                var usuario = ManejadorTokens.ObtenerUsuarioToken(contexto.Request);

                int? idUsuario = usuario == null ? null : usuario.Id;

                var parametroPeticion = new Controllers.Servicios.AuditoriasAplicacion.PeticionAgregar.Parametros(idUsuario, ruta, accion);
                var peticion = new Controllers.Servicios.AuditoriasAplicacion.PeticionAgregar(parametroPeticion, new RepositorioAuditoriaAplicacion());

                var ejecutorPeticiones = new EjecutorPeticiones();
                ejecutorPeticiones.Ejecutar(peticion);
            }
            catch(Exception error)
            {
                // Error al intentar auditar. Lo escribo en el log.
                Logger.LogException(error);
            }

            await _next(contexto);
        }
    }
}
