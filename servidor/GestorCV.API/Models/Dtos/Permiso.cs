using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Models.Dtos
{
    public sealed class Permiso : Acceso
    {
        public Permiso(int id, string nombre, string accion, Formulario formulario)
            : base(id, nombre)
        {
            Accion = accion;
            Formulario = formulario;
        }

        public string Accion { get; private set; }

        public Formulario Formulario { get; private set; }

        public override bool ValidarRuta(string ruta, string accion)
        {
            // Valido si la acción es la misma
            if (accion != Accion)
            {
                return false;
            }

            // Valido si la ruta existe en el formulario
            return Formulario.RutasFormulario.Any(rf => rf.Ruta == ruta);
        }

        /// <summary>
        /// Obtiene todos los accesos exclusivos para el frontend registrados en el formulario del permiso.
        /// </summary>
        /// <returns>Lista de rutas de frontend con el tipo de permiso admitido.</returns>
        public override List<Acceso.PermisoFrontend> ObtenerPermisosFrontend()
        {
            // Valido si la ruta existe en el formulario
            return Formulario
                .RutasFormulario
                .Where(rf => !rf.Backend)
                .Select(rf => new PermisoFrontend(rf.Ruta, Accion.Equals(EntidadConAcceso.PermisoDeEscritura)))
                .ToList();
        }
    }
}
