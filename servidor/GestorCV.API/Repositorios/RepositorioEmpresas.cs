using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioEmpresas : IRepositorio
    {
        public List<Models.Dtos.Empresa> ObtenerTodos(bool incluirLogo = false);

        public Models.Dtos.Empresa Obtener(int id);
    }

    /// <summary>
    /// Repositorio de empresas.
    /// </summary>
    public sealed class RepositorioEmpresas : RepositorioBase, IRepositorioEmpresas
    {
        /// <summary>
        /// Obtiene las empresas.
        /// </summary>
        /// <param name="incluirLogo">Indica si debe objetenes también el logo de las empresas.</param>
        /// <returns>Empresas guardadas en la base de datos.</returns>
        public List<Models.Dtos.Empresa> ObtenerTodos(bool incluirLogo = false)
        {
            var empleos = _contexto.Empresas.ToList();

            return empleos
                .Select(e =>
                {
                    return incluirLogo 
                        ? new Models.Dtos.Empresa(e.Id, e.Nombre, e.Logo)
                        : new Models.Dtos.Empresa(e.Id, e.Nombre);
                })
                .ToList();
        }

        /// <summary>
        /// Obtiene el empleo con el ID especificado.
        /// </summary>
        /// <param name="id">ID del empleo a obtener.</param>
        /// <returns>Empleo guardado en base de datos.</returns>
        public Models.Dtos.Empresa Obtener(int id)
        {
            if (id == 0)
            {
                return null;
            }

            var empresa = _contexto.Empresas
                .FirstOrDefault(e => e.Id == id);

            return new Models.Dtos.Empresa(empresa.Id, empresa.Nombre, empresa.Logo);
        }
    }
}
