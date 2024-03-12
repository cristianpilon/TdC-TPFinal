using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioEtiquetas : IRepositorio
    {
        public List<Models.Dtos.Etiqueta> ObtenerTodos();
    }

    /// <summary>
    /// Repositorio de etiquetas.
    /// </summary>
    public sealed class RepositorioEtiquetas: RepositorioBase, IRepositorioEtiquetas
    {
        /// <summary>
        /// Obtiene las etiquetas.
        /// </summary>
        /// <returns>Etiquetas guardadas en base de datos.</returns>
        public List<Models.Dtos.Etiqueta> ObtenerTodos()
        {
            var etiquetas = _contexto.Etiquetas
                .Select(x => new Models.Dtos.Etiqueta(x.Id, x.Nombre))
                .ToList();

            return etiquetas;
        }
    }
}
