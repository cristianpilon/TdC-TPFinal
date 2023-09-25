using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using GestorCV.API.Infraestructura;
using Newtonsoft.Json;
using System.Net;

public class ErrorHandlerMiddleware//ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

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
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                    bodyResponse = "Ha ocurrido un error. Contacte al equipo técnico para más detalles";
                    break;
            }

            await response.WriteAsync(bodyResponse);
        }
    }
}