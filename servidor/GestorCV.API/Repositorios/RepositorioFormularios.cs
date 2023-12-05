using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios.Base;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    /// <summary>
    /// Repositorio de formularios.
    /// </summary>
    public sealed class RepositorioFormularios : RepositorioBase
    {
        /// <summary>
        /// Obtienes las rutas asociadas a un formulario.
        /// </summary>
        /// <param name="id">ID del formulario.</param>
        /// <returns>Rutas asociadas al formulario.</returns>
        public List<RutaFormulario> ObtenerRutas(int id)
        {
            var rutasFormuario = _contexto.RutasFormularios
                .Where(rf => rf.IdFormulario == id)
                .ToList();

            return rutasFormuario
                .Select(rf => new RutaFormulario(rf.Id, rf.Ruta, rf.Backend))
                .ToList();
        }
    }
}
