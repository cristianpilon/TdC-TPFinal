using GestorCV.API.Models;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioAuditoriasAplicacion : IRepositorio
    {
        public List<Models.Dtos.AuditoriaAplicacion> ObtenerTodos();

        public void Agregar(int? idUsuario, string ruta, string accion);
    }

    public class RepositorioAuditoriaAplicacion : RepositorioBase, IRepositorioAuditoriasAplicacion
    {
        private const string UsuarioAnonimo = "[Anónimo]";

        /// <summary>
        /// Agrega un nuevo registro de auditoría de llamada al servidor.
        /// </summary>
        /// <param name="idUsuario">Usuario que realiza el acceso (null para usuario anónimo).</param>
        /// <param name="ruta">Ruta que se accede.</param>
        /// <param name="accion">Acción que se realiza sobre la ruta.</param>
        public void Agregar(int? idUsuario, string ruta, string accion)
        {
            var nuevaAuditoria = new AuditoriaAplicacion();
            nuevaAuditoria.IdUsuario = idUsuario;
            nuevaAuditoria.Ruta = ruta;
            nuevaAuditoria.Accion = accion;
            nuevaAuditoria.Fecha = DateTime.Now;

            _contexto.Add(nuevaAuditoria);
            _contexto.SaveChanges();
        }

        /// <summary>
        /// Obtiene las auditorías de llamadas al servidor.
        /// </summary>
        /// <returns>Auditorias guardadas en base de datos.</returns>A
        public List<Models.Dtos.AuditoriaAplicacion> ObtenerTodos()
        {
            var usuarios = _contexto.Usuarios.Select(x => new { x.Id, x.Nombre }).ToList();

            Func<AuditoriaAplicacion, Models.Dtos.AuditoriaAplicacion> mapToDto = x =>
            {
                var usuario = x.IdUsuario.HasValue ? usuarios.Find(u => u.Id == x.IdUsuario).Nombre : UsuarioAnonimo;
                return new Models.Dtos.AuditoriaAplicacion(usuario, x.Ruta, x.Accion, x.Fecha);
            };

            var autoriasAplicacion = _contexto.AuditoriasAplicacion
                .Select(mapToDto)
                .OrderByDescending(x => x.Fecha)
                .ToList();

            return autoriasAplicacion;
        }
    }
}
