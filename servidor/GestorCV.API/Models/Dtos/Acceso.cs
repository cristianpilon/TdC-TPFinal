using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos
{
    /// <summary>
    /// Clase de abstracción para validar los accesos y establecer un patrón composite sobre las clases Grupos y Permisos.
    /// </summary>
    public abstract class Acceso
    {
        public Acceso(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public int Id { get; private set; }

        public string Nombre { get; private set; }

        public abstract bool ValidarRuta(string ruta, string accion);

        public abstract List<PermisoFrontend> ObtenerPermisosFrontend();

        /// <summary>
        /// Clase para abstraer los permisos exclusivos del frontend.
        /// </summary>
        public class PermisoFrontend
        {
            public PermisoFrontend(string ruta, bool escritura)
            {
                Ruta = ruta;
                Escritura = escritura;
            }

            public string Ruta { get; private set; }

            public bool Escritura { get; private set; }
        }
    }
}
