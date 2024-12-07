using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioEmpleos : IRepositorio
    {
        public List<Models.Dtos.Empleo> ObtenerTodos(string criterio);

        public Models.Dtos.Empleo Obtener(int id);

        public List<Models.Dtos.Empleo> ObtenerTodosSugeridos(int idUsuario);
    }

    /// <summary>
    /// Repositorio de empleos.
    /// </summary>
    public sealed class RepositorioEmpleos : RepositorioBase, IRepositorioEmpleos
    {
        /// <summary>
        /// Obtiene los empleos.
        /// </summary>
        /// <param name="criterio">Filtro para título/empresa/ubicación.</param>
        /// <returns>Empleos guardados en la base de datos.</returns>
        public List<Models.Dtos.Empleo> ObtenerTodos(string criterio)
        {
            var empleos = _contexto.Empleos
                .Include(e => e.EtiquetasEmpleos)
                .ThenInclude(e => e.IdEtiquetaNavigation)
                .Include(e => e.PerfilesEmpleos)
                .ThenInclude(e => e.IdPerfilNavigation)
                .Where(e =>
                    string.IsNullOrEmpty(criterio)
                    || e.Titulo.Contains(criterio)
                    || e.Empresa.Contains(criterio)
                    || e.Ubicacion.Contains(criterio))
                .ToList();

            return empleos
                .Select(e =>
                {
                    var perfiles = e.PerfilesEmpleos
                        .Select(pe => new Models.Dtos.Perfil(pe.IdPerfil, pe.IdPerfilNavigation.Nombre))
                        .ToList();

                    var etiquetas = e.EtiquetasEmpleos
                        .Select(ee => new Models.Dtos.Etiqueta(ee.IdEtiqueta, ee.IdEtiquetaNavigation.Nombre))
                        .ToList();

                    return new Models.Dtos.Empleo(e.Id, e.Titulo, e.Descripcion, e.Ubicacion, e.Remuneracion, e.ModalidadTrabajo,
                        e.FechaPublicacion, e.HorariosLaborales, e.TipoTrabajo, e.Empresa, e.EmpresaLogo, e.Destacado, etiquetas, perfiles);
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
                empleo.FechaPublicacion, empleo.HorariosLaborales, empleo.TipoTrabajo, empleo.Empresa, empleo.EmpresaLogo, empleo.Destacado, etiquetas, perfiles);
        }

        /// <summary>
        /// Obtiene los empleos sugeridos para el usuario seleccionado basandose en la coincidencia de etiquetas y perfiles.
        /// </summary>
        /// <param name="idUsuario">Usuario para el cual se desea coincidir los empleos.</param>
        /// <returns>Empleos guardados en la base de datos que coincidan con las etiquetas y perfiles del usuario especificado.</returns>
        public List<Models.Dtos.Empleo> ObtenerTodosSugeridos(int idUsuario)
        {
            // Obtengo las etiquetas y perfiles del usuario especificado
            var usuario = _contexto.Usuarios.Find(idUsuario);

            _contexto.Entry(usuario)
            .Collection(u => u.EtiquetasUsuarios)
            .Load();

            _contexto.Entry(usuario)
                .Collection(u => u.PerfilesUsuarios)
                .Load();
            var etiquetasUsuario = usuario.EtiquetasUsuarios.Select(eu => eu.IdEtiqueta).ToList();
            var perfilesUsuario = usuario.PerfilesUsuarios.Select(pu => pu.IdPerfil).ToList();

            // Filtro los empleos por el criterio y las coincidencias con el usuario especificado
            var empleos = _contexto.Empleos
                .Include(e => e.EtiquetasEmpleos)
                    .ThenInclude(ee => ee.IdEtiquetaNavigation)
                .Include(e => e.PerfilesEmpleos)
                    .ThenInclude(pe => pe.IdPerfilNavigation)
                .Where(e => e.EtiquetasEmpleos.Any(ee => etiquetasUsuario.Contains(ee.IdEtiqueta)) ||
                            e.PerfilesEmpleos.Any(pe => perfilesUsuario.Contains(pe.IdPerfil)))
                .ToList();

            // Realiza la ordenación después de cargar los datos
            var empleosOrdenados = empleos
                .OrderByDescending(e => 
                    e.EtiquetasEmpleos.Count(ee => etiquetasUsuario.Contains(ee.IdEtiqueta))
                    + e.PerfilesEmpleos.Count(pe => perfilesUsuario.Contains(pe.IdPerfil))
                    // Si el empleo es destacado, brinda mejor posición
                    + (e.Destacado ? 2 : 0)
                )
                .ToList();

            return empleosOrdenados
                .Select(e =>
                {
                    var perfiles = e.PerfilesEmpleos
                        .Select(pe => new Models.Dtos.Perfil(pe.IdPerfil, pe.IdPerfilNavigation.Nombre))
                        .ToList();

                    var etiquetas = e.EtiquetasEmpleos
                        .Select(ee => new Models.Dtos.Etiqueta(ee.IdEtiqueta, ee.IdEtiquetaNavigation.Nombre))
                        .ToList();

                    return new Models.Dtos.Empleo(e.Id, e.Titulo, e.Descripcion, e.Ubicacion, e.Remuneracion, e.ModalidadTrabajo,
                        e.FechaPublicacion, e.HorariosLaborales, e.TipoTrabajo, e.Empresa, e.EmpresaLogo, e.Destacado, etiquetas, perfiles);
                })
                .ToList();
        }
    }
}
