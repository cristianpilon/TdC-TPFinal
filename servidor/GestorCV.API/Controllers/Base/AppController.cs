using GestorCV.API.Controllers.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using GestorCV.API.Infraestructura;
using GestorCV.API.Repositorios;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GestorCV.API.Infraestructura.Seguridad;

namespace GestorCV.API.Controllers.Base
{
    public class AppController : ControllerBase
    {
        protected Models.Dtos.Usuario ObtenerUsuarioToken()
        {
            //var tokenNoSanitizado = HttpContext.Request.Headers["Authorization"].ToString();
            string token = FactoriaTokens.fixedToken;
            //var token = AuthenticationHeaderValue.Parse(tokenNoSanitizado).Parameter.Trim('"');
            

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
                
                var repositorioUsuarios = new RepositorioUsuarios();

                return repositorioUsuarios.ObtenerConAccesos(idUsuario);
            }
            catch (SecurityTokenException)
            {
                // Si hubo un error de seguridad en el token, no se autoriza
                return null;
            }
        }

        protected EjecutorPeticiones EjecutorPeticiones { get; set; }

        protected int UsuarioId { get { return ObtenerUsuarioToken().Id; } }

        protected string UsuarioCorreo { get { return ObtenerUsuarioToken().Correo; } }

        protected string UsuarioNombre { 
            get {
                var usuario = ObtenerUsuarioToken();
                return $"{usuario.Nombre} {usuario.Apellido} "; 
            } 
        }

        public AppController()
        {
            EjecutorPeticiones = new EjecutorPeticiones();
        }
    }
}
