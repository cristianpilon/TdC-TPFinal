using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioPerfiles : IRepositorio
    {
        public List<Models.Dtos.Perfil> ObtenerTodos();
    }

    /// <summary>
    /// Repositorio de perfiles.
    /// </summary>
    public sealed class RepositorioPerfiles : RepositorioBase, IRepositorioPerfiles
    {
        /// <summary>
        /// Obtiene los perfiles.
        /// </summary>
        /// <returns>Perfiles guardados en base de datos.</returns>
        public List<Models.Dtos.Perfil> ObtenerTodos()
        {
            var perfiles = _contexto.Perfiles
                .Select(x => new Models.Dtos.Perfil(x.Id, x.Nombre))
                .ToList();

            return perfiles;
        }
    }
}
