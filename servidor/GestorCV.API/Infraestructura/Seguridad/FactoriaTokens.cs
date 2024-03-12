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
        public const string fixedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiUGF1bGEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zdXJuYW1lIjoiRmVybmFuZGV6IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2F1dGhvcml6YXRpb25kZWNpc2lvbiI6IltdIiwiZXhwIjoxODAzNTgzOTQxLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjQwMDAvIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0MDAwLyJ9.CyxMS39dL_wKv4nZrCptF03y6MnQrf2PlMDj1ojhVL8";
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

            var Sectoken = new JwtSecurityToken(
              AppConfiguration.FirmaToken,
              AppConfiguration.FirmaToken,
              claims,
              //expires: DateTime.Now.AddMonths(1),
              expires: DateTime.Now.AddYears(3),
              signingCredentials: credenciales);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}
