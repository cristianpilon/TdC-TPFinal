using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace GestorCV.API.Infraestructura.Seguridad
{
    /// <summary>
    /// Clase dedicada al manejo de criptografía en el aplicativo.
    /// </summary>
    public class ManejadorCriptografia
    {
        // Utilizo semilla para encriptar y desencriptar la contraseña
        // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-8.0

        /// <summary>
        /// Codifica la contraseña a HASH con criptografía HMACSHA256.
        /// </summary>
        /// <param name="password">Password a codificar</param>
        /// <returns>Password codificado</returns>
        public static string Codificar(string password)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(AppConfiguration.SemillaPassword),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }
    }
}
