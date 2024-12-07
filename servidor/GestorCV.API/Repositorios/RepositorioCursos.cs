using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioCursos : IRepositorio
    {
        public List<Models.Dtos.Curso> ObtenerTodos();

        public Models.Dtos.Curso Obtener(int id);
    }

    /// <summary>
    /// Repositorio de cursos.
    /// </summary>
    public sealed class RepositorioCursos : RepositorioBase, IRepositorioCursos
    {
        /// <summary>
        /// Obtiene los cursos.
        /// </summary>
        /// <returns>Cuesos guardados en la base de datos.</returns>
        public List<Models.Dtos.Curso> ObtenerTodos()
        {
            var cursos = _contexto.Cursos
                .Include(c => c.EtiquetasCursos)
                .ThenInclude(c => c.IdEtiquetaNavigation)
                .Include(c => c.PerfilesCursos)
                .ThenInclude(c => c.IdPerfilNavigation)
                .ToList();

            return cursos
                .Select(c =>
                {
                    var perfiles = c.PerfilesCursos
                        .Select(pc => new Models.Dtos.Perfil(pc.IdPerfil, pc.IdPerfilNavigation.Nombre))
                        .ToList();

                    var etiquetas = c.EtiquetasCursos
                        .Select(pc => new Models.Dtos.Etiqueta(pc.IdEtiqueta, pc.IdEtiquetaNavigation.Nombre))
                        .ToList();

                    return new Models.Dtos.Curso(c.Id, c.Titulo, c.Mensaje, c.Fecha, etiquetas, perfiles);
                })
                .ToList();
        }

        /// <summary>
        /// Obtiene el curso con el ID especificado.
        /// </summary>
        /// <param name="id">ID del curso a obtener.</param>
        /// <returns>Curso guardado en base de datos.</returns>
        public Models.Dtos.Curso Obtener(int id)
        {
            if (id == 0)
            {
                return null;
            }

            var curso = _contexto.Cursos
                .Include(e => e.EtiquetasCursos)
                .ThenInclude(ee => ee.IdEtiquetaNavigation)
                .Include(e => e.PerfilesCursos)
                .ThenInclude(e => e.IdPerfilNavigation)
                .FirstOrDefault(e => e.Id == id);

            var perfiles = curso.PerfilesCursos
                        .Select(pe => new Models.Dtos.Perfil(pe.IdPerfil, pe.IdPerfilNavigation.Nombre))
                        .ToList();

            var etiquetas = curso.EtiquetasCursos
                .Select(ee => new Models.Dtos.Etiqueta(ee.IdEtiqueta, ee.IdEtiquetaNavigation.Nombre))
                .ToList();

            return new Models.Dtos.Curso(curso.Id, curso.Titulo, curso.Mensaje, curso.Fecha, etiquetas, perfiles);
        }
    }
}
