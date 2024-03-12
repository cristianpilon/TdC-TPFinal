using GestorCV.API.Models;
using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioCurriculums : IRepositorio
    {
        public Models.Dtos.Curriculum Obtener(int id);

        public void Modificar(int id, Models.Dtos.Curriculum curriculum);
    }

    /// <summary>
    /// Repositorio de curriculums.
    /// </summary>
    public sealed class RepositorioCurriculums : RepositorioBase, IRepositorioCurriculums
    {
        /// <summary>
        /// Actualiza la información de un curriculum
        /// </summary>
        /// <param name="id">ID del usuario del cual se desea obtener el curriculum.</param>
        /// <param name="curriculum">Curriculum con nueva información</param>
        public void Modificar(int id, Models.Dtos.Curriculum curriculum)
        {
            var curriculumExistente = _contexto.Usuarios
                .Include(x => x.IdCurriculumNavigation)
                .Where(x => x.Id == id)
                .Select(x => x.IdCurriculumNavigation)
                .First();

            curriculumExistente.Titulo = curriculum.Titulo;
            curriculumExistente.Ubicacion = curriculum.Ubicacion;
            curriculumExistente.ResumenProfesional = curriculum.ResumenProfesional;
            curriculumExistente.ExperienciaLaboral = curriculum.ExperienciaLaboral;
            curriculumExistente.Idiomas = curriculum.Idiomas;
            curriculumExistente.Educacion = curriculum.Educacion;
            curriculumExistente.Certificados = curriculum.Certificados;
            curriculumExistente.Intereses = curriculum.Intereses;

            var usuario = _contexto.Usuarios
                .Include(x => x.IdCurriculumNavigation)
                .Include(x => x.PerfilesUsuarios)
                .Include(x => x.EtiquetasUsuarios)
                .Where(x => x.IdCurriculum == curriculumExistente.Id)
                .First();

            // Eliminar perfiles existentes
            var perfilesUsuario = usuario.PerfilesUsuarios.ToList();
            _contexto.PerfilesUsuarios.RemoveRange(perfilesUsuario);

            // Eliminar etiquetas existentes
            var etiquetasUsuario = usuario.EtiquetasUsuarios.ToList();
            _contexto.EtiquetasUsuarios.RemoveRange(etiquetasUsuario);
            
            // Agregar perfiles nuevos
            foreach (var perfil in curriculum.Perfiles)
            {
                usuario.PerfilesUsuarios.Add(new PerfilesUsuario
                {
                    IdUsuario = usuario.Id,
                    IdPerfil = perfil.Id,
                });
            }

            // Agregar etiquetas nuevas
            foreach (var etiqueta in curriculum.Etiquetas)
            {
                usuario.EtiquetasUsuarios.Add(new EtiquetasUsuario
                {
                    IdUsuario = usuario.Id,
                    IdEtiqueta = etiqueta.Id,
                });
            }

            _contexto.SaveChanges();
        }

        /// <summary>
        /// Obtiene el curriculum del usuario con el ID especificado.
        /// </summary>
        /// <param name="id">ID del usuario del cual se desea obtener el curriculum.</param>
        /// <returns>Curriculum guardado en base de datos.</returns>
        public Models.Dtos.Curriculum Obtener(int id)
        {
            var usuario = _contexto.Usuarios
                .Include(x => x.IdCurriculumNavigation)
                .Include(x => x.EtiquetasUsuarios)
                .ThenInclude(x => x.IdEtiquetaNavigation)
                .Include(x => x.PerfilesUsuarios)
                .ThenInclude(x => x.IdPerfilNavigation)
                .FirstOrDefault(x => x.Id == id);

            var perfiles = usuario.PerfilesUsuarios
                        .Select(pe => new Models.Dtos.Perfil(pe.IdPerfil, pe.IdPerfilNavigation.Nombre))
                        .ToList();

            var etiquetas = usuario.EtiquetasUsuarios
                .Select(ee => new Models.Dtos.Etiqueta(ee.IdEtiqueta, ee.IdEtiquetaNavigation.Nombre))
                .ToList();

            var curriculum = usuario.IdCurriculumNavigation;

            return new Models.Dtos.Curriculum(curriculum.Titulo, curriculum.Ubicacion, curriculum.ResumenProfesional, curriculum.ExperienciaLaboral, curriculum.Educacion,
                curriculum.Idiomas, curriculum.Certificados, curriculum.Intereses, etiquetas, perfiles);
        }
    }
}
