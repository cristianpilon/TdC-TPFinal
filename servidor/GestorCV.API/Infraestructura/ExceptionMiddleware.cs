using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Net;

namespace GestorCV.API.Infraestructura
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                string bodyResponse;

                switch (error)
                {
                    case ValidacionException e:
                        // Errores de validacion
                        response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                        response.ContentType = "application/json";
                        bodyResponse = JsonConvert.SerializeObject(new { e.Validaciones });

                        break;
                    case UnauthorizedAccessException:
                        // Errores de validacion
                        response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401
                        response.ContentType = "application/json";
                        bodyResponse = JsonConvert.SerializeObject(new { Mensaje = "Operación o usuario no autorizado. Contacte al equipo técnico para más detalles." });

                        break;
                    default:
                        // Errores no controlados
                        response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                        response.ContentType = "application/json";
                        bodyResponse = JsonConvert.SerializeObject(new { Mensaje = "Ha ocurrido un error. Contacte al equipo técnico para más detalles" });

                        // Escribo el error en los logs
                        Logger.LogException(error);
                        break;
                }

                await response.WriteAsync(bodyResponse);
            }
        }
    }
}