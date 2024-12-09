using GestorCV.API.Infraestructura.Seguridad;
using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioUsuarios : IRepositorio
    {
        public Models.Dtos.Usuario Autenticar(string nombreUsuario, string password);

        public Models.Dtos.Usuario ObtenerConAccesos(int idUsuario);
    }

    /// <summary>
    /// Repositorio de usuarios
    /// </summary>
    public sealed class RepositorioUsuarios : RepositorioBase, IRepositorioUsuarios
    {
        /// <summary>
        /// Devuelve el usuario que coincida con el nombre de usuario y contraseña provistos.
        /// </summary>
        /// <param name="nombreUsuario">Nombre de usuario a obtener</param>
        /// <param name="password">Contraseña de usuario a obtener</param>
        /// <returns>Usuario coincidente con los parametros provistos. El método devuelve 'null' si ningún usuario coincidio con el criterio.</returns>
        public Models.Dtos.Usuario Autenticar(string nombreUsuario, string password) 
        {
            var usuario = _contexto.Usuarios
                .FirstOrDefault(u => u.Correo == nombreUsuario && u.Password == ManejadorCriptografia.Codificar(password));

            if (usuario != null)
            {
                return ObtenerConAccesos(usuario.Id);
            }

            return null;
        }

        /// <summary>
        /// Devuelve el usuario que coincida con el ID provisto con la informacion para verificar acceso.
        /// </summary>
        /// <param name="idUsuario">ID de usuario a obtener</param>
        /// <returns>Usuario coincidente con el ID provisto. El método devuelve 'null' si ningún usuario coincidio con el criterio.</returns>
        public Models.Dtos.Usuario ObtenerConAccesos(int idUsuario)
        {
            var usuario = _contexto.Usuarios
                .Include(u => u.IdEmpresaNavigation)
                .Include(u => u.IdRolNavigation)
                .Include(u => u.IdRolNavigation.RolesPermisos)
                .Include(u => u.IdRolNavigation.RolesGrupos)
                .Include(u => u.UsuariosPermisos)
                .Include(u => u.UsuariosGrupos)
                .FirstOrDefault(u => u.Id == idUsuario);

            if (usuario == null)
            {
                return null;
            }

            return CrearDtoUsuarioParaValidarAccesos(usuario);
        }

        /// <summary>
        /// Crea DTO de usuario con sus accesos.
        /// </summary>
        /// <param name="usuario">Modelo de usuario a convertir.</param>
        /// <returns>DTO de usuario</returns>
        private Models.Dtos.Usuario CrearDtoUsuarioParaValidarAccesos(Models.Usuario usuario)
        {
            var accesos = new List<Acceso>();
            accesos.AddRange(ObtenerPermisosUsuario(usuario));

            var rol = CrearDtoRol(usuario.IdRolNavigation);

            Empresa empresa = null;

            if (usuario.IdEmpresa.HasValue)
            {
                empresa = new Empresa(usuario.IdEmpresa.Value, usuario.IdEmpresaNavigation.Nombre);
            }

            return new Models.Dtos.Usuario(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Correo, rol, empresa, accesos);
        }

        /// <summary>
        /// Crea DTO de rol con sus accesos.
        /// </summary>
        /// <param name="rol">Modelo de rol a convertir.</param>
        /// <returns></returns>
        private Models.Dtos.Rol CrearDtoRol(Models.Rol rol)
        {
            var accesos = new List<Acceso>();
            accesos.AddRange(ObtenerPermisosRol(rol));

            return new Models.Dtos.Rol(rol.Id, rol.Nombre, accesos);
        }

        /// <summary>
        /// Obtiene los permisos asociados a un usuario
        /// </summary>
        /// <param name="usuario">Usuario para el cuál se desea obtener los permisos</param>
        /// <returns>Permisos asociados al usuario</returns>
        private List<Models.Dtos.Permiso> ObtenerPermisosUsuario(Models.Usuario usuario)
        {
            var repositorioGrupos = new RepositorioGrupos();

            // Obtengo la ruta de los formularios asociadas a los permisos
            var formulariosIds = usuario.UsuariosPermisos.Select(u => u.IdPermisoNavigation.IdFormularioNavigation.Id);
            var rutasFormularios = _contexto.RutasFormularios.Where(rf => formulariosIds.Any(fId => fId == rf.IdFormulario)).ToList();

            var permisos = usuario.UsuariosPermisos.Select(up =>
            {
                var permiso = up.IdPermisoNavigation;
                var formulario = permiso.IdFormularioNavigation;

                var rutas = rutasFormularios.Select(rf => new Models.Dtos.RutaFormulario(rf.Id, rf.Ruta, rf.Backend)).ToList();
                var nuevoFormulario = new Models.Dtos.Formulario(formulario.Id, formulario.Nombre, rutas);
                return new Models.Dtos.Permiso(
                    permiso.Id,
                    string.Empty,
                    permiso.Accion,
                    nuevoFormulario);
            }).ToList();

            // obtengo los permisos asociados a los grupos del usuario
            foreach (var usuarioGrupo in usuario.UsuariosGrupos)
            {
                permisos.AddRange(repositorioGrupos.ObtenerPermisos(usuarioGrupo.IdGrupo));
            }

            return permisos;
        }

        /// <summary>
        /// Obtiene los permisos asociados a un rol
        /// </summary>
        /// <param name="rol">Rol para el cuál se desea obtener los permisos</param>
        /// <returns>Permisos asociados al rol</returns>
        private List<Models.Dtos.Permiso> ObtenerPermisosRol(Models.Rol rol)
        {
            var repositorioGrupos = new RepositorioGrupos();

            // Obtengo la ruta de los formularios asociadas a los permisos
            var formulariosIds = rol.RolesPermisos.Select(u => u.IdPermisoNavigation.IdFormularioNavigation.Id);
            var rutasFormularios = _contexto.RutasFormularios.Where(rf => formulariosIds.Any(fId => fId == rf.IdFormulario)).ToList();

            var permisos = rol.RolesPermisos.Select(up =>
            {
                var permiso = up.IdPermisoNavigation;
                var formulario = permiso.IdFormularioNavigation;

                var rutas = rutasFormularios.Select(rf => new Models.Dtos.RutaFormulario(rf.Id, rf.Ruta, rf.Backend)).ToList();
                var nuevoFormulario = new Models.Dtos.Formulario(formulario.Id, formulario.Nombre, rutas);
                return new Models.Dtos.Permiso(
                    permiso.Id,
                    string.Empty,
                    permiso.Accion,
                    nuevoFormulario);
            }).ToList();

            // obtengo los permisos asociados a los grupos del rol
            foreach (var usuarioGrupo in rol.RolesGrupos)
            {
                permisos.AddRange(repositorioGrupos.ObtenerPermisos(usuarioGrupo.IdGrupo));
            }

            return permisos;
        }
    }
}
