using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Models.Dtos
{
    /// <summary>
    /// Clase para abstraer los permisos de las clases de usuario y rol.
    /// </summary>
    public abstract class EntidadConAcceso
    {
        public const string PermisoDeEscritura = "Escritura";
        public const string PermisoDeLectura = "Lectura";

        public EntidadConAcceso(List<Acceso> accesos)
        {
            Accesos = accesos;
        }

        public List<Acceso> Accesos { get; private set; }

        /// <summary>
        /// Indica si el usuario posee los permisos necesarios para ejecutar la operación para la ruta especificada.
        /// </summary>
        /// <param name="ruta">Ruta de la petición a validar.</param>
        /// <param name="verboOperacion">Verbo HTTP de la operación que se desea validar.</param>
        /// <returns>Bandera que indica si el usuario tiene permitida la operación en la ruta especificada.</returns>
        public bool ValidarAcceso(string ruta, string verboOperacion)
        {
            var accion = ConvertirVerboEnAccion(verboOperacion);
            return Accesos.Any(acceso => acceso.ValidarRuta(ruta, accion));
        }

        /// <summary>
        /// Convierte el verbo de la petición en escritura o lectura. Si el verbo no es válido, devuelve una excepción no controlada
        /// </summary>
        /// <param name="verboOperacion">Verbo de la petición HTTP.</param>
        /// <returns>La acción que realiza el verbo de la operación: Escritura/Lectura.</returns>
        private static string ConvertirVerboEnAccion(string verboOperacion)
        {
            return verboOperacion.ToUpperInvariant() switch
            {
                "GET" or "OPTION" => PermisoDeLectura,
                "POST" or "DELETE" or "PATCH" or "PUT" => PermisoDeEscritura,
                _ => throw new System.ArgumentException("El verbo de la petición no es válido"),
            };
        }
    }
}
