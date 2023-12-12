using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioGrupos : IRepositorio
    {
        public List<Permiso> ObtenerPermisos(int id);
    }

    /// <summary>
    /// Repositorio de grupos.
    /// </summary>
    public sealed class RepositorioGrupos : RepositorioBase, IRepositorioGrupos
    {
        /// <summary>
        /// Obtiene los permisos asociados al grupo.
        /// </summary>
        /// <param name="id">ID del grupo.</param>
        /// <returns>Permisos asociados al grupo.</returns>
        public List<Permiso> ObtenerPermisos(int id)
        {
            var repositorioFormularios = new RepositorioFormularios();
            var grupos = _contexto.Grupos
                .Where(g => g.Id == id || (g.IdGrupoPadre.HasValue && g.IdGrupoPadre == id))
                .ToList();

            var permisos = _contexto.GruposPermisos
                .Include(gp => gp.IdPermisoNavigation)
                .Include("IdPermisoNavigation.IdFormularioNavigation")
                .Where(gp => grupos.Any(g => g.Id == gp.IdGrupo))
                .ToList() // Busca los permisos en la base de datos
                .Select(pg =>
                new Permiso(
                    pg.IdPermiso,
                    pg.IdPermisoNavigation.Nombre,
                    pg.IdPermisoNavigation.Accion,
                    new Formulario(
                        pg.IdPermisoNavigation.IdFormulario,
                        pg.IdPermisoNavigation.IdFormularioNavigation.Nombre,
                        repositorioFormularios.ObtenerRutas(pg.IdPermisoNavigation.IdFormulario)
                    )
                )
            ).ToList();

            // Obtengo los grupos hijos para extraer los permisos asociados a estos
            var gruposHijosIds = grupos
                .Select(g => g.Id)
                .Where(gId => gId != id)
                .ToList();

            foreach (var grupoId in gruposHijosIds)
            {
                // Recursividad para obtener los permisos de los grupos hijos
                permisos.AddRange(ObtenerPermisos(grupoId));
            }

            return permisos;
        }
    }
}
