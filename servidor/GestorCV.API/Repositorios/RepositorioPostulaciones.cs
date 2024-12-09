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

        public RespuestaAgregarPostulacion Agregar(Models.Dtos.Postulacion postulacion);
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

            return new RespuestaAgregarPostulacion(nuevaPostulacion.Id, empleo.Titulo, $"{usuario.Nombre} {usuario.Apellido}");
        }

        private void ValidarPostulacionExistente(Postulacion postulacion)
        {
            // Si el usuario ya fue postulado, devuelvo excepción de validacion
            var postulacionExistente = _contexto.Postulaciones
                .FirstOrDefault(x => x.IdEmpleo == postulacion.IdEmpleo && x.IdUsuario == postulacion.IdUsuario);

            if (postulacionExistente != null)
            {
                var validaciones = new List<Validacion>();
                validaciones.Add(new Validacion("Usted ya se ha postulado para esta vacante de empleo"));

                throw new ValidacionException(validaciones);
            }
        }

        public sealed class RespuestaAgregarPostulacion
        {
            public RespuestaAgregarPostulacion(int id, string empleo, string usuario)
            {
                Id = id;
                Empleo = empleo;
                Usuario = usuario;
            }

            public int Id { get; private set; }

            public string Empleo { get; private set; }

            public string Usuario { get; private set; }
        }
    }
}
