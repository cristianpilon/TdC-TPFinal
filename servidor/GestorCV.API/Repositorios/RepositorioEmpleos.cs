﻿using GestorCV.API.Infraestructura;
using GestorCV.API.Models;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static GestorCV.API.Infraestructura.ValidacionException;
using static GestorCV.API.Repositorios.RepositorioEmpleos;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioEmpleos : IRepositorio
    {
        public List<Models.Dtos.Empleo> ObtenerTodosAvanzado(string empresa, string titulo, string ubicacion, string tipoUbicacion, DateTime? fechaDesde, IEnumerable<int> etiquetas, IEnumerable<int> perfiles);

        public List<Models.Dtos.Empleo> ObtenerTodos(string criterio, int? idUsuario);

        public Models.Dtos.Empleo Obtener(int id);

        public List<Models.Dtos.Empleo> ObtenerTodosSugeridos(int idUsuario);

        public void Modificar(int id, Models.Dtos.Empleo empleo, bool? destacado);

        public RespuestaAgregarEmpleo Agregar(int idUsuario, Models.Dtos.Empleo empleoDto);

        public void Eliminar(int idEmpleo);
    }

    /// <summary>
    /// Repositorio de empleos.
    /// </summary>
    public sealed class RepositorioEmpleos : RepositorioBase, IRepositorioEmpleos
    {
        /// <summary>
        /// Obtiene los empleos buscando por un criterio más especificos.
        /// </summary>
        /// <param name="empresa">Criterio de empresa (contiene).</param>
        /// <param name="titulo">Criterio de título (contiene).</param>
        /// <param name="ubicacion">Ubicación de los empleos</param>
        /// <param name="fechaDesde">Fecha de inicio desde de las publicaciones.</param>
        /// <param name="etiquetas">Etiquetas del empleo.</param>
        /// <param name="perfiles">Perfiles del empleo.</param>
        /// <returns>Empleos guardados en la base de datos.</returns>
        public List<Models.Dtos.Empleo> ObtenerTodosAvanzado(string empresa, string titulo, string ubicacion, string tipoUbicacion, DateTime? fechaDesde, IEnumerable<int> etiquetas, IEnumerable<int> perfiles)
        {
            IQueryable<Empleo> empleos = _contexto.Empleos
                .Include(e => e.IdEmpresaNavigation)
                .Include(e => e.EtiquetasEmpleos)
                .ThenInclude(e => e.IdEtiquetaNavigation)
                .Include(e => e.PerfilesEmpleos)
                .ThenInclude(e => e.IdPerfilNavigation);

            // Agregamos condiciones dinámicas
            if (!string.IsNullOrEmpty(empresa))
            {
                empleos = empleos.Where(e => e.IdEmpresaNavigation.Nombre.Contains(empresa));
            }

            if (!string.IsNullOrEmpty(titulo))
            {
                empleos = empleos.Where(e => e.Titulo.Contains(titulo));
            }

            if (!string.IsNullOrEmpty(ubicacion) && !string.IsNullOrEmpty(tipoUbicacion))
            {
                if (tipoUbicacion == "Localidad")
                {
                    empleos = empleos
                        .AsEnumerable()
                        .Where(e =>
                        {
                            var ubicacionSinProvincia = e.Ubicacion.Substring(0, e.Ubicacion.LastIndexOf(',')).Trim();
                            var penultimoSegmento = ubicacionSinProvincia.Substring(ubicacionSinProvincia.LastIndexOf(',') + 1).Trim();
                            return penultimoSegmento.Contains(ubicacion);
                        })
                        .AsQueryable();
                }
                else if (tipoUbicacion == "Provincia")
                {
                    empleos = empleos
                        .AsEnumerable()
                        .Where(e => e.Ubicacion.Substring(e.Ubicacion.LastIndexOf(',') + 1).Trim().Contains(ubicacion))
                        .AsQueryable();
                }
            }

            if (fechaDesde.HasValue)
            {
                empleos = empleos.Where(e => e.FechaPublicacion >= fechaDesde.Value);
            }

            if (etiquetas != null && etiquetas.Any())
            {
                empleos = empleos.Where(e => e.EtiquetasEmpleos.Any(ee => etiquetas.Contains(ee.IdEtiqueta)));
            }

            if (perfiles != null && perfiles.Any())
            {
                empleos = empleos.Where(e => e.PerfilesEmpleos.Any(pe => perfiles.Contains(pe.IdPerfil)));
            };

            return empleos.ToList()
                .Select(e =>
                {
                    var perfiles = e.PerfilesEmpleos
                        .Select(pe => new Models.Dtos.Perfil(pe.IdPerfil, pe.IdPerfilNavigation.Nombre))
                        .ToList();

                    var etiquetas = e.EtiquetasEmpleos
                        .Select(ee => new Models.Dtos.Etiqueta(ee.IdEtiqueta, ee.IdEtiquetaNavigation.Nombre))
                        .ToList();

                    return new Models.Dtos.Empleo(e.Id, e.Titulo, e.Descripcion, e.Ubicacion, e.Remuneracion, e.ModalidadTrabajo,
                        e.FechaPublicacion, e.HorariosLaborales, e.TipoTrabajo,
                        new Models.Dtos.Empresa(e.IdEmpresaNavigation.Id, e.IdEmpresaNavigation.Nombre, e.IdEmpresaNavigation.Logo),
                        e.Destacado, etiquetas, perfiles);
                })
                .ToList();
        }

        /// <summary>
        /// Obtiene los empleos.
        /// </summary>
        /// <param name="criterio">Filtro para título/empresa/ubicación.</param>
        /// <returns>Empleos guardados en la base de datos.</returns>
        public List<Models.Dtos.Empleo> ObtenerTodos(string criterio, int? idUsuario)
        {
            var empleos = _contexto.Empleos
                .Include(e => e.IdEmpresaNavigation)
                .Include(e => e.EtiquetasEmpleos)
                .ThenInclude(e => e.IdEtiquetaNavigation)
                .Include(e => e.PerfilesEmpleos)
                .ThenInclude(e => e.IdPerfilNavigation)
                .Where(e =>
                    (!idUsuario.HasValue || e.IdUsuarioCreador == idUsuario.Value)
                    && (string.IsNullOrEmpty(criterio)
                    || e.Titulo.Contains(criterio)
                    || e.IdEmpresaNavigation.Nombre.Contains(criterio)
                    || e.Ubicacion.Contains(criterio)))
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
                        e.FechaPublicacion, e.HorariosLaborales, e.TipoTrabajo,
                        new Models.Dtos.Empresa(e.IdEmpresaNavigation.Id, e.IdEmpresaNavigation.Nombre, e.IdEmpresaNavigation.Logo),
                        e.Destacado, etiquetas, perfiles);
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
            if (id == 0)
            {
                return null;
            }

            var empleo = _contexto.Empleos
                .Include(e => e.IdEmpresaNavigation)
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
                empleo.FechaPublicacion, empleo.HorariosLaborales, empleo.TipoTrabajo, new Models.Dtos.Empresa(empleo.IdEmpresaNavigation.Id, empleo.IdEmpresaNavigation.Nombre), empleo.Destacado, etiquetas, perfiles);
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
                .Include(e => e.IdEmpresaNavigation)
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
                        e.FechaPublicacion, e.HorariosLaborales, e.TipoTrabajo,
                        new Models.Dtos.Empresa(e.IdEmpresaNavigation.Id, e.IdEmpresaNavigation.Nombre, e.IdEmpresaNavigation.Logo),
                        e.Destacado, etiquetas, perfiles);
                })
                .ToList();
        }

        public void Modificar(int id, Models.Dtos.Empleo empleoDto, bool? destacado)
        {
            // Busca la entidad en la base de datos
            var empleo = _contexto.Empleos
                .Include(c => c.EtiquetasEmpleos)
                .Include(c => c.PerfilesEmpleos)
                .FirstOrDefault(c => c.Id == id);

            ValidarEmpleo(empleo);

            empleo.Titulo = empleoDto.Titulo;
            empleo.Descripcion = empleoDto.Descripcion;
            empleo.FechaPublicacion = empleoDto.FechaPublicacion;
            empleo.HorariosLaborales = empleoDto.HorariosLaborales;
            empleo.ModalidadTrabajo = empleoDto.ModalidadTrabajo;
            empleo.TipoTrabajo = empleoDto.TipoTrabajo;
            empleo.Ubicacion = empleoDto.Ubicacion;
            empleo.Remuneracion = empleoDto.Remuneracion;

            if (destacado.HasValue)
            {
                empleo.Destacado = destacado.Value;
            }

            // Actualiza etiquetas
            _contexto.EtiquetasEmpleos.RemoveRange(empleo.EtiquetasEmpleos);
            empleo.EtiquetasEmpleos.Clear();
            if (empleoDto.Etiquetas != null && empleoDto.Etiquetas.Any())
            {
                foreach (var etiquetaDto in empleoDto.Etiquetas)
                {
                    empleo.EtiquetasEmpleos.Add(new EtiquetasEmpleo
                    {
                        IdEtiqueta = etiquetaDto.Id,
                        IdEmpleo = empleo.Id
                    });
                }
            }

            _contexto.PerfilesEmpleos.RemoveRange(empleo.PerfilesEmpleos);
            empleo.PerfilesEmpleos.Clear();
            if (empleoDto.Perfiles != null && empleoDto.Perfiles.Any())
            {
                foreach (var perfilDto in empleoDto.Perfiles)
                {
                    empleo.PerfilesEmpleos.Add(new PerfilesEmpleo
                    {
                        IdPerfil = perfilDto.Id,
                        IdEmpleo = empleo.Id
                    });
                }
            }

            _contexto.Update(empleo);

            _contexto.SaveChanges();
        }

        public RespuestaAgregarEmpleo Agregar(int idUsuario, Models.Dtos.Empleo empleoDto)
        {
            ValidarEmpresa(empleoDto);

            var nuevoEmpleo = new Models.Empleo
            {
                Titulo = empleoDto.Titulo,
                Descripcion = empleoDto.Descripcion,
                FechaPublicacion = empleoDto.FechaPublicacion,
                IdEmpresa = empleoDto.Empresa.Id,
                IdUsuarioCreador = idUsuario,
                Destacado = empleoDto.Destacado,
                HorariosLaborales = empleoDto.HorariosLaborales,
                ModalidadTrabajo = empleoDto.ModalidadTrabajo,
                TipoTrabajo = empleoDto.TipoTrabajo,
                Ubicacion = empleoDto.Ubicacion,
                Remuneracion = empleoDto.Remuneracion,
                EtiquetasEmpleos = empleoDto.Etiquetas.Select(e => new EtiquetasEmpleo
                {
                    IdEtiqueta = e.Id
                }).ToList(),
                PerfilesEmpleos = empleoDto.Perfiles.Select(p => new PerfilesEmpleo
                {
                    IdPerfil = p.Id
                }).ToList()
            };

            _contexto.Empleos.Add(nuevoEmpleo);
            _contexto.SaveChanges();

            return new RespuestaAgregarEmpleo(nuevoEmpleo.Id);
        }

        private void ValidarEmpleo(Models.Empleo empleo)
        {
            if (empleo == null)
            {
                var validaciones = new List<Validacion>
                {
                    new("El empleo especificado no existe.")
                };

                throw new ValidacionException(validaciones);
            }
        }

        private void ValidarEmpresa(Models.Dtos.Empleo empleo)
        {
            if (empleo.Empresa == null || empleo.Empresa.Id <= 0)
            {
                var validaciones = new List<Validacion>
                {
                    new("La empresa especificada es incorrecta.")
                };

                throw new ValidacionException(validaciones);
            }

            var empresa = _contexto.Empresas
                .FirstOrDefault(x => x.Id == empleo.Empresa.Id);

            if (empresa == null)
            {
                var validaciones = new List<Validacion>
                {
                    new("La empresa especificada es incorrecta.")
                };

                throw new ValidacionException(validaciones);
            }
        }

        public void Eliminar(int idEmpleo)
        {
            var empleo = _contexto.Empleos.FirstOrDefault(x => x.Id == idEmpleo);

            ValidarEmpleo(empleo);

            _contexto.Remove(empleo);
            _contexto.SaveChanges();
        }

        public sealed class RespuestaAgregarEmpleo
        {
            public RespuestaAgregarEmpleo(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }
    }
}
