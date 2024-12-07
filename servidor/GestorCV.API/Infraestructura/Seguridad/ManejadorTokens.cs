using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace GestorCV.API.Infraestructura.Seguridad
{
    public class ManejadorTokens
    {
        public static Models.Dtos.Usuario ObtenerUsuarioToken(HttpRequest peticionHttp)
        {
            var tokenNoSanitizado = peticionHttp.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(tokenNoSanitizado))
            {
                return null;
            }

            var token = AuthenticationHeaderValue.Parse(tokenNoSanitizado).Parameter.Trim('"');

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
    }
}
