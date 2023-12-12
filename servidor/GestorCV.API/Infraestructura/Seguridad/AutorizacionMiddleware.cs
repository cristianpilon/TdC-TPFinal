using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using GestorCV.API.Repositorios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Azure;
using Newtonsoft.Json;

namespace GestorCV.API.Infraestructura.Seguridad
{
    public class AutorizacionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RepositorioUsuarios repositorioUsuarios;

        public AutorizacionMiddleware(RequestDelegate next)
        {
            _next = next;
            repositorioUsuarios = new RepositorioUsuarios();
        }

        public async Task Invoke(HttpContext context)
        {
            // Si el token de autonizacion existe, valido los permisos para el usuario
            if (context.Request.Headers["Autorization"].Count > 0)
            {
                var controllerActionDescriptor = context
                    .GetEndpoint()
                    .Metadata
                    .GetMetadata<ControllerActionDescriptor>();

                var controllerName = controllerActionDescriptor.ControllerName;
                var actionName = controllerActionDescriptor.ActionName;

                var ruta = controllerName + "/" + actionName;
                var verboOperacion = context.Request.Method;

                var tokenNoSanitizado = context.Request.Headers["Authorization"].ToString();
                var tokenAutorizacion = AuthenticationHeaderValue.Parse(tokenNoSanitizado).Parameter.Trim('"');
                if (!UsuarioAutorizado(tokenAutorizacion, ruta, verboOperacion))
                {
                    var response = context.Response;
                    response.ContentType = "application/json";

                    response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401
                    response.ContentType = "application/json";
                    var bodyResponse = JsonConvert.SerializeObject(new { Mensaje = "Operación o usuario no autorizado. Contacte al equipo técnico para más detalles." });
                    await response.WriteAsync(bodyResponse);

                    return;
                }
            }

            await _next(context);
        }

        /// <summary>
        /// Valida los permisos del usuario autenticado.
        /// </summary>
        /// <param name="token">Token de la petición</param>
        /// <param name="ruta">Ruta del servicio a la que se quiere acceder</param>
        /// <param name="verboOperacion">Accion que se quiere realizar</param>
        /// <returns>Devuelve una bandera que indica si el usuario tiene acceso</returns>
        public bool UsuarioAutorizado(string token, string ruta, string verboOperacion)
        {
            var usuario = ObtenerUsuarioToken(token);

            if (usuario != null)
            {
                // Valido si alguno de los permisos del rol autoriza la ruta
                var autorizadoRol = usuario.Rol.ValidarAcceso(ruta, verboOperacion);

                // Valido si alguno de los permisos del usuario autoriza la ruta
                var autorizadoPermisos = usuario.ValidarAcceso(ruta, verboOperacion);

                return autorizadoRol || autorizadoPermisos;
            }

            return false;
        }

        /// <summary>
        /// Obtiene el usuario registrado en el token, siempre que el token sea valido.
        /// </summary>
        /// <param name="token">Token de la petición</param>
        /// <returns>Usuario asociado al token</returns>
        public Models.Dtos.Usuario ObtenerUsuarioToken(string token)
        {
            ClaimsPrincipal datosToken;
            var jwtIssuer = AppConfiguration.FirmaToken;
            var jwtKey = AppConfiguration.ClaveToken;

            try
            {
                // Validar datos del token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                datosToken = tokenHandler.ValidateToken(token, validationParameters, out _);

                // Obtener ID de usuario desde el token
                var idUsuario = int.Parse(datosToken.FindFirstValue(ClaimTypes.NameIdentifier));

                return repositorioUsuarios.ObtenerConAccesos(idUsuario);
            }
            catch (SecurityTokenException)
            {
                // Si hubo un error de seguridad en el token, no se autoriza
                return null;
            }
        }

        /// <summary>
        /// Devuelve una respuesta de acceso denegado en la peticion (Error 401)
        /// </summary>
        /// <param name="context">Contexto de la peticion</param>
        public void DenegarAcceso(HttpContext context)
        {
            throw new UnauthorizedAccessException();
        }
    }
}
