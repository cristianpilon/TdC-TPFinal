using GestorCV.API.Infraestructura;
using GestorCV.API.Models;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static GestorCV.API.Infraestructura.ValidacionException;
using static GestorCV.API.Repositorios.RepositorioCursos;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioCursos : IRepositorio
    {
        public List<Models.Dtos.Curso> ObtenerTodos();

        public Models.Dtos.Curso Obtener(int id);

        public void Modificar(int id, Models.Dtos.Curso curso);

        public RespuestaAgregarCurso Agregar(int idUsuario, Models.Dtos.Curso cursoDto);
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
                .Include(c => c.IdEmpresaNavigation)
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

                    return new Models.Dtos.Curso(c.Id, c.Titulo, c.Mensaje, c.Fecha, new Models.Dtos.Empresa(c.IdEmpresaNavigation.Id, c.IdEmpresaNavigation.Nombre), etiquetas, perfiles);
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
                .Include(c => c.IdEmpresaNavigation)
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

            return new Models.Dtos.Curso(curso.Id, curso.Titulo, curso.Mensaje, curso.Fecha, new Models.Dtos.Empresa(curso.IdEmpresaNavigation.Id, curso.IdEmpresaNavigation.Nombre), etiquetas, perfiles);
        }

        public void Modificar(int id, Models.Dtos.Curso cursoDto)
        {
            // Busca la entidad en la base de datos
            var curso = _contexto.Cursos
                .Include(c => c.EtiquetasCursos)
                .Include(c => c.PerfilesCursos)
                .FirstOrDefault(c => c.Id == id);

            ValidarCurso(curso);

            curso.Titulo = cursoDto.Titulo;
            curso.Mensaje = cursoDto.Mensaje;
            curso.Fecha = cursoDto.Fecha;

            // Actualiza etiquetas
            _contexto.EtiquetasCursos.RemoveRange(curso.EtiquetasCursos);
            curso.EtiquetasCursos.Clear();
            if (cursoDto.Etiquetas != null && cursoDto.Etiquetas.Any())
            {
                foreach (var etiquetaDto in cursoDto.Etiquetas)
                {
                    curso.EtiquetasCursos.Add(new EtiquetasCurso
                    {
                        IdEtiqueta = etiquetaDto.Id,
                        IdCurso = curso.Id
                    });
                }
            }

            _contexto.PerfilesCursos.RemoveRange(curso.PerfilesCursos);
            curso.PerfilesCursos.Clear();
            if (cursoDto.Perfiles != null && cursoDto.Perfiles.Any())
            {
                foreach (var perfilDto in cursoDto.Perfiles)
                {
                    curso.PerfilesCursos.Add(new PerfilesCurso
                    {
                        IdPerfil = perfilDto.Id,
                        IdCurso = curso.Id
                    });
                }
            }

            _contexto.Update(curso);

            _contexto.SaveChanges();

        }

        public RespuestaAgregarCurso Agregar(int idUsuario, Models.Dtos.Curso cursoDto)
        {
            ValidarEmpresa(cursoDto);

            var nuevoCurso = new Models.Curso
            {
                Titulo = cursoDto.Titulo,
                Mensaje = cursoDto.Mensaje,
                Fecha = cursoDto.Fecha,
                IdEmpresa = cursoDto.Empresa.Id,
                IdUsuarioCreador = idUsuario,
                EtiquetasCursos = cursoDto.Etiquetas.Select(e => new EtiquetasCurso
                {
                    IdEtiqueta = e.Id
                }).ToList(),
                PerfilesCursos = cursoDto.Perfiles.Select(p => new PerfilesCurso
                {
                    IdPerfil = p.Id
                }).ToList()
            };

            _contexto.Cursos.Add(nuevoCurso);
            _contexto.SaveChanges();

            return new RespuestaAgregarCurso(nuevoCurso.Id);
        }

        public sealed class RespuestaAgregarCurso
        {
            public RespuestaAgregarCurso(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }

        private void ValidarCurso(Models.Curso curso)
        {
            if (curso == null)
            {
                var validaciones = new List<Validacion>
                {
                    new("El curso especificado no existe.")
                };

                throw new ValidacionException(validaciones);
            }
        }

        private void ValidarEmpresa(Models.Dtos.Curso curso)
        {
            if (curso.Empresa == null || curso.Empresa.Id <= 0)
            {
                var validaciones = new List<Validacion>
                {
                    new("La empresa especificada es incorrecta.")
                };

                throw new ValidacionException(validaciones);
            }

            var empresa = _contexto.Empresas
                .FirstOrDefault(x => x.Id == curso.Empresa.Id);

            if (empresa == null)
            {
                var validaciones = new List<Validacion>
                {
                    new("La empresa especificada es incorrecta.")
                };

                throw new ValidacionException(validaciones);
            }
        }
    }
}
