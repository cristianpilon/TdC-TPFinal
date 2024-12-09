using GestorCV.API.Infraestructura;
using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static GestorCV.API.Infraestructura.ValidacionException;
using static GestorCV.API.Repositorios.RepositorioPostulaciones;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioPostulaciones : IRepositorio
    {
        public List<Postulacion> ObtenerTodos(int? idUsuario);

        public RespuestaAgregarPostulacion Agregar(Postulacion postulacion);

        public Postulacion Obtener(int id);

        public RespuestaModificarPostulacion Modificar(int id, string estado);
    }

    /// <summary>
    /// Repositorio de postulaciones.
    /// </summary>
    public sealed class RepositorioPostulaciones : RepositorioBase, IRepositorioPostulaciones
    {
        /// <summary>
        /// Obtiene todas las postulaciones registradas en el sistema
        /// </summary>
        /// <param name="idUsuario">Filtro por ID de usuario.</param>
        /// <returns>Postulaciones guardadas en la base de datos</returns>
        public List<Postulacion> ObtenerTodos(int? idUsuario)
        {
            var postulaciones = _contexto.Postulaciones
                .Include(x => x.IdEmpleoNavigation)
                .ThenInclude(x => x.IdEmpresaNavigation)
                .Include(x => x.IdUsuarioNavigation)
                .Where(x => !idUsuario.HasValue || x.IdUsuario == idUsuario.Value)
                .Select(x => new Postulacion(x.Id, x.IdEmpleo, x.IdUsuario, x.Estado, x.Fecha, x.IdEmpleoNavigation, x.IdUsuarioNavigation))
                .ToList();

            return postulaciones;
        }

        /// <summary>
        /// Crea una nueva postulación.
        /// </summary>
        /// <param name="postulacion">Postulación a crear.</param>
        /// <returns>ID de la postulación creada.</returns>
        public RespuestaAgregarPostulacion Agregar(Models.Dtos.Postulacion postulacion)
        {
            ValidarPostulacionExistente(postulacion);

            var nuevaPostulacion = new Models.Postulacion
            {
                IdUsuario = postulacion.IdUsuario,
                IdEmpleo = postulacion.IdEmpleo,
                Estado = postulacion.Estado,
                Fecha = DateTime.Now,
            };

            _contexto.Postulaciones
                .Add(nuevaPostulacion);

            _contexto.SaveChanges();

            var empleo = _contexto.Empleos.First(e => e.Id == postulacion.IdEmpleo);
            var usuario = _contexto.Usuarios.First(u => u.Id == postulacion.IdUsuario);

            var usuariosNotificacionIds = _contexto.Usuarios
                .Where(x => x.IdEmpresa == empleo.IdEmpresa)
                .Select(x => x.Id);

            return new RespuestaAgregarPostulacion(nuevaPostulacion.Id, empleo.Titulo, $"{usuario.Nombre} {usuario.Apellido}", usuariosNotificacionIds);
        }

        public Postulacion Obtener(int id)
        {
            var postulacion = _contexto.Postulaciones
                .Include(x => x.IdUsuarioNavigation)
                .ThenInclude(x => x.IdCurriculumNavigation)
                .Include(x => x.IdUsuarioNavigation)
                .ThenInclude(x => x.EtiquetasUsuarios)
                .ThenInclude(x => x.IdEtiquetaNavigation)
                .Include(x => x.IdUsuarioNavigation)
                .ThenInclude(x => x.PerfilesUsuarios)
                .ThenInclude(x => x.IdPerfilNavigation)
                .FirstOrDefault(x => x.Id == id);

            var empleo = _contexto.Postulaciones
                .Include(x => x.IdEmpleoNavigation)
                .ThenInclude(x => x.EtiquetasEmpleos)
                .ThenInclude(x => x.IdEtiquetaNavigation)
                .Include(x => x.IdEmpleoNavigation)
                .ThenInclude(x => x.PerfilesEmpleos)
                .ThenInclude(x => x.IdPerfilNavigation)
                .First(x => x.Id == id)
                .IdEmpleoNavigation;

            return new Postulacion(postulacion.Id, postulacion.Estado, postulacion.Fecha, empleo, postulacion.IdUsuarioNavigation, postulacion.IdUsuarioNavigation.IdCurriculumNavigation);
        }

        public RespuestaModificarPostulacion Modificar(int id, string estado)
        {
            var postulacion = _contexto.Postulaciones
                .Include(x => x.IdUsuarioNavigation)
                .Include(x => x.IdEmpleoNavigation)
                .FirstOrDefault(x => x.Id == id);

            ValidarPostulacion(postulacion);

            postulacion.Estado = estado;

            _contexto.Update(postulacion);

            _contexto.SaveChanges();

            return new RespuestaModificarPostulacion(postulacion.IdUsuarioNavigation.Id,
                $"{postulacion.IdUsuarioNavigation.Nombre} {postulacion.IdUsuarioNavigation.Apellido}",
                postulacion.IdUsuarioNavigation.Correo, postulacion.IdEmpleoNavigation.Titulo);
        }

        public sealed class RespuestaModificarPostulacion
        {
            public RespuestaModificarPostulacion(int usuarioId, string nombreCompleto, string correo, string tituloEmpleo)
            {
                UsuarioId = usuarioId;
                UsuarioNombre = nombreCompleto;
                UsuarioCorreo = correo;
                TituloEmpleo = tituloEmpleo;
            }

            public int UsuarioId { get; private set; }

            public string UsuarioNombre { get; private set; }

            public string UsuarioCorreo { get; private set; }

            public string TituloEmpleo { get; private set; }
        }

        public sealed class RespuestaAgregarPostulacion
        {
            public RespuestaAgregarPostulacion(int id, string empleo, string usuario, IEnumerable<int> usuariosNotificacionIds)
            {
                Id = id;
                Empleo = empleo;
                Usuario = usuario;
                UsuariosNotificacionIds = usuariosNotificacionIds;
            }

            public int Id { get; private set; }

            public string Empleo { get; private set; }

            public string Usuario { get; private set; }

            public IEnumerable<int> UsuariosNotificacionIds { get; set; }
        }

        private void ValidarPostulacion(Models.Postulacion postulacion)
        {
            if (postulacion == null)
            {
                var validaciones = new List<Validacion>
                {
                    new("La postulación especificada no existe.")
                };

                throw new ValidacionException(validaciones);
            }
        }

        private void ValidarEstadoPostulacion(string estado)
        {
            switch (estado)
            {
                case "Revisado":
                case "Descartado":
                case "Entrevista":
                case "Finalizado":
                    break;
                default:
                    var validaciones = new List<Validacion>
                    {
                        new("El estado especificado para la postulación es incorrecto.")
                    };

                    throw new ValidacionException(validaciones);
            }
        }

        private void ValidarPostulacionExistente(Models.Dtos.Postulacion postulacion)
        {
            // Si el usuario ya fue postulado, devuelvo excepción de validacion
            var postulacionExistente = _contexto.Postulaciones
                .FirstOrDefault(x => x.IdEmpleo == postulacion.IdEmpleo && x.IdUsuario == postulacion.IdUsuario);

            if (postulacionExistente != null)
            {
                var validaciones = new List<Validacion>
                {
                    new Validacion("Usted ya se ha postulado para esta vacante de empleo")
                };

                throw new ValidacionException(validaciones);
            }
        }
    }
}
