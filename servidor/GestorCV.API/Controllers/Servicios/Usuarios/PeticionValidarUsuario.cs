using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Infraestructura;
using GestorCV.API.Infraestructura.Seguridad;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Usuarios
{
    public class PeticionValidarUsuario : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionValidarUsuario(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var usuario = ((IRepositorioUsuarios)Repositorio).Autenticar(ParametrosPeticion.Usuario, ParametrosPeticion.Password);

            if (usuario == null)
            {
                ValidacionException.LanzarValidacionSimple("El usuario o la contraseña es incorrecta");
            }

            return new Resultado { Rol = usuario.Rol.Nombre, Token = FactoriaTokens.Crear(usuario) };
        }

        public override List<ValidacionException.Validacion> Validar()
        {
            var validaciones = new List<ValidacionException.Validacion>();

            if (string.IsNullOrEmpty(ParametrosPeticion.Usuario))
            {
                var validacion = new ValidacionException.Validacion("Debe ingresar el nombre de usuario", "usuario");
                validaciones.Add(validacion);
            }

            if (string.IsNullOrEmpty(ParametrosPeticion.Password))
            {
                var validacion = new ValidacionException.Validacion("Debe ingresar la contraseña", "password");
                validaciones.Add(validacion);
            }

            return validaciones;
        }

        public class Resultado : IResultado
        {
            public string Rol { get; set; }

            public string Token { get; set; }
        }

        public class Parametros
        {
            public string Usuario { get; set; }

            public string Password { get; set; }
        }
    }
}
