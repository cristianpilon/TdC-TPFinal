using GestorCV.API.Controllers.Servicios.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Infraestructura;
using GestorCV.API.Infraestructura.Seguridad;
using GestorCV.API.Repositorios;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Usuarios
{
        public class ValidarUsuario : PeticionBase
        {
            public Parametros ParametrosPeticion { get; set; }

            public ValidarUsuario(Parametros parametros)
            {
                ParametrosPeticion = parametros;
            }

            public override IResultado Procesar()
            {
                var repositorio = new RepositorioUsuarios();
                var usuario = repositorio.Autenticar(ParametrosPeticion.Usuario, ParametrosPeticion.Password);

                if (usuario == null)
                {
                    ValidacionException.LanzarValidacionSimple("El usuario o la contraseña es incorrecta");
                }

                return new Resultado { Token = FactoriaTokens.Crear(usuario) };
            }

            public override void Validar()
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

                throw new ValidacionException(validaciones);
            }

            public class Resultado : IResultado
            {
                public string Token { get; set; }
            }

            public class Parametros
            {
                public string Usuario { get; set; }

                public string Password { get; set; }
            }
        }
}
