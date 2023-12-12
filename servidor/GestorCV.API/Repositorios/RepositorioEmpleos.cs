using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioEmpleos : IRepositorio
    {
        public List<Models.Dtos.Empleo> ObtenerTodos();

        public Models.Dtos.Empleo Obtener(int id);
    }

    /// <summary>
    /// Repositorio de empleos.
    /// </summary>
    public sealed class RepositorioEmpleos : RepositorioBase, IRepositorioEmpleos
    {
        /// <summary>
        /// Obtiene los empleos.
        /// </summary>
        /// <returns>Empleos guardados en la base de datos.</returns>
        public List<Models.Dtos.Empleo> ObtenerTodos()
        {
            var empleos = _contexto.Empleos
                .Include(e => e.EtiquetasEmpleos)
                .ThenInclude(e => e.IdEtiquetaNavigation)
                .Include(e => e.PerfilesEmpleos)
                .ThenInclude(e => e.IdPerfilNavigation)
                .ToList();

            return empleos
                .Select(e => {
                    var perfiles = e.PerfilesEmpleos
                        .Select(pe => new Models.Dtos.Perfil(pe.IdPerfil, pe.IdPerfilNavigation.Nombre))
                        .ToList();

                    var etiquetas = e.EtiquetasEmpleos
                        .Select(ee => new Models.Dtos.Etiqueta(ee.IdEtiqueta, ee.IdEtiquetaNavigation.Nombre))
                        .ToList();

                    return new Models.Dtos.Empleo(e.Id, e.Titulo, e.Descripcion, e.Ubicacion, e.Remuneracion, e.ModalidadTrabajo,
                        e.FechaPublicacion, e.HorariosLaborales, e.TipoTrabajo, etiquetas, perfiles); 
                })
                .ToList();
        }

        /// <summary>
        /// Obtiene el empleo con el ID especificado.
        /// </summary>
        /// <param name="id">ID del empleo a obtener.</param>
        /// <returns>Empleo guardado en base de datos.</returns>
        public Models.Dtos.Empleo Obtener(int id)
        {
            var empleo = _contexto.Empleos
                .Include(e => e.EtiquetasEmpleos)
                .ThenInclude(ee => ee.IdEtiquetaNavigation)
                .Include(e => e.PerfilesEmpleos)
                .ThenInclude(e => e.IdPerfilNavigation)
                .FirstOrDefault(e => e.Id == id);

            var perfiles = empleo.PerfilesEmpleos
                        .Select(pe => new Models.Dtos.Perfil(pe.IdPerfil, pe.IdPerfilNavigation.Nombre))
                        .ToList();

            var etiquetas = empleo.EtiquetasEmpleos
                .Select(ee => new Models.Dtos.Etiqueta(ee.IdEtiqueta, ee.IdEtiquetaNavigation.Nombre))
                .ToList();

            return new Models.Dtos.Empleo(empleo.Id, empleo.Titulo, empleo.Descripcion, empleo.Ubicacion, empleo.Remuneracion, empleo.ModalidadTrabajo,
                empleo.FechaPublicacion, empleo.HorariosLaborales, empleo.TipoTrabajo, etiquetas, perfiles);
        }
    }
}
