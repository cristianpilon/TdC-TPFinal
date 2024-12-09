using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioNotificaciones : IRepositorio
    {
        public void Agregar(Notificacion notificacion);

        public List<Notificacion> ObtenerTodos(int idUsuario, DateTime? limit);

        public void Modificar(int idUsuario, DateTime limit);
    }

    /// <summary>
    /// Repositorio de notificaciones.
    /// </summary>
    public sealed class RepositorioNotificaciones : RepositorioBase, IRepositorioNotificaciones
    {
        /// <summary>
        /// Obtiene todas las notificaciones registradas en el sistema para el usuario especificado.
        /// </summary>
        /// <param name="idUsuario">ID de usuario.</param>
        /// <returns>Notificaciones guardadas en la base de datos.</returns>
        public List<Notificacion> ObtenerTodos(int idUsuario, DateTime? limit)
        {
            var notificaciones = _contexto.Notificaciones
                .Where(x => x.IdUsuario == idUsuario && (!limit.HasValue || x.FechaCreacion < limit))
                .Take(7)
                .ToList();

            return notificaciones
                .Select(x => new Notificacion(x.Id, x.IdUsuario, x.Mensaje, x.FechaLectura, x.FechaCreacion))
                .ToList();
        }

        /// <summary>
        /// Crea una nueva notificación.
        /// </summary>
        /// <param name="notificacion">Notificación a crear.</param>
        /// <returns>ID de la notificación creada.</returns>
        public void Agregar(Models.Dtos.Notificacion notificacion)
        {
            var nuevaNotificacion = new Models.Notificacion
            {
                IdUsuario = notificacion.IdUsuario,
                Mensaje = notificacion.Mensaje,
                FechaCreacion = notificacion.FechaCreacion,
            };

            _contexto.Notificaciones
                .Add(nuevaNotificacion);

            _contexto.SaveChanges();
        }

        /// <summary>
        /// Establece la fecha de lectura de la notificación
        /// </summary>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <param name="limit">Fecha hasta donde deben marcarse las notificaciones como leídas</param>
        public void Modificar(int idUsuario, DateTime limit)
        {
            var notificaciones = _contexto.Notificaciones
                .Where(x => x.IdUsuario == idUsuario && x.FechaCreacion <= limit);

            foreach (var notificacion in notificaciones)
            {
                notificacion.FechaLectura = DateTime.UtcNow;
            }

            _contexto.Update(notificaciones);

            _contexto.SaveChanges();
        }
    }
}
