using GestorCV.API.Controllers.Servicios;
using GestorCV.API.Infraestructura.Seguridad;
using GestorCV.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers.Base
{
    public class AppController : ControllerBase
    {
        private Models.Dtos.Usuario _usuario;

        protected Models.Dtos.Usuario ObtenerUsuarioToken()
        {
            if (_usuario != null)
            {
                return _usuario;
            }

            _usuario = ManejadorTokens.ObtenerUsuarioToken(HttpContext.Request);

            return _usuario;
        }

        public EjecutorPeticiones EjecutorPeticiones { get; set; }

        public int UsuarioId
        {
            get
            {
                var usuario = ObtenerUsuarioToken();
                if (usuario == null)
                {
                    return 0;
                }

                return ObtenerUsuarioToken().Id;
            }
        }

        public Rol UsuarioRol
        {
            get
            {
                var usuario = ObtenerUsuarioToken();
                if (usuario == null)
                {
                    return null;
                }

                return ObtenerUsuarioToken().Rol;
            }
        }

        protected string UsuarioCorreo
        {
            get
            {
                var usuario = ObtenerUsuarioToken();
                if (usuario == null)
                {
                    return string.Empty;
                }

                return ObtenerUsuarioToken().Correo;
            }
        }

        protected string UsuarioNombre
        {
            get
            {
                var usuario = ObtenerUsuarioToken();
                return $"{usuario.Nombre} {usuario.Apellido} ";
            }
        }

        public Empresa UsuarioEmpresa
        {
            get
            {
                var usuario = ObtenerUsuarioToken();
                if (usuario == null)
                {
                    return null;
                }

                return usuario.Empresa;
            }
        }

        public AppController()
        {
            EjecutorPeticiones = new EjecutorPeticiones();
        }
    }
}
