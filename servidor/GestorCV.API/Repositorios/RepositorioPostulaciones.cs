using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using System.Linq;
using static GestorCV.API.Repositorios.RepositorioPostulaciones;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioPostulaciones : IRepositorio
    {
        public RespuestaAgregarPostulacion Agregar(Models.Dtos.Postulacion postulacion);
    }

    /// <summary>
    /// Repositorio de postulaciones.
    /// </summary>
    public sealed class RepositorioPostulaciones : RepositorioBase, IRepositorioPostulaciones
    {
        /// <summary>
        /// Crea una nueva postulación.
        /// </summary>
        /// <param name="postulacion">Postulación a crear.</param>
        /// <returns>ID de la postulación creada.</returns>
        public RespuestaAgregarPostulacion Agregar(Models.Dtos.Postulacion postulacion)
        {
            var nuevaPostulacion = new Models.Postulacion
            {
                IdUsuario = postulacion.IdUsuario,
                IdEmpleo = postulacion.IdEmpleo,
                Estado = postulacion.Estado,
            };

            _contexto.Postulaciones
                .Add(nuevaPostulacion);

            _contexto.SaveChanges();

            var empleo = _contexto.Empleos.First(e => e.Id == postulacion.IdEmpleo);
            var usuario = _contexto.Usuarios.First(u => u.Id == postulacion.IdUsuario);

            return new RespuestaAgregarPostulacion(nuevaPostulacion.Id, empleo.Titulo, $"{usuario.Nombre} {usuario.Apellido}");
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
