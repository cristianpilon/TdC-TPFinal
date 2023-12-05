using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;

namespace GestorCV.API.Infraestructura.Seguridad
{
    /// <summary>
    /// Clase de factoría para generar tokens de usuario (Algoritmo HmacSha256)
    /// </summary>
    public class FactoriaTokens
    {
        /// <summary>
        /// Crea token de usuario a partir de un modelo de usuario (info de usuario y permisos de cliente).
        /// </summary>
        /// <param name="usuario">Modelo de usuario del cual se desea generar el token</param>
        /// <returns>Cadena de texto que representa el token generado</returns>
        public static string Crear(Models.Dtos.Usuario usuario)
        {
            var claveDeSeguridad = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.ClaveToken));
            var credenciales = new SigningCredentials(claveDeSeguridad, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>() { 
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Surname, usuario.Apellido),
                new Claim(ClaimTypes.Role, usuario.Rol.Id.ToString()),
                new Claim(ClaimTypes.AuthorizationDecision, JsonConvert.SerializeObject(usuario.ObtenerPermisosFrontend())),
            };

            var Sectoken = new JwtSecurityToken(AppConfiguration.ClaveToken,
              AppConfiguration.FirmaToken,
              claims,
              expires: DateTime.Now.AddMonths(1),
              signingCredentials: credenciales);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}
