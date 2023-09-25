using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Infraestructura;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios
{
    public class ServicioUsuarios
    {
        public class PeticionValidarUsuario : IPeticion
        {
            public Parametros ParametrosPeticion { get; set; }

            public PeticionValidarUsuario(Parametros parametros)
            {
                ParametrosPeticion = parametros;
            }

            public IResultado Procesar()
            {
                var tipoUsuario = ParametrosPeticion.Usuario == "usuarioAdmin" ? "Admin" : "Usuario";

                return new Resultado { Rol = tipoUsuario };
            }

            public void Validar()
            {
                var validaciones = new List<ValidacionException.Validacion>();

                if (ParametrosPeticion.Password != "123456")
                {
                    var validacion = new ValidacionException.Validacion
                    {
                        Mensaje = "El usuario o la contraseña es incorrecta",
                    };

                    validaciones.Add(validacion);

                    throw new ValidacionException(validaciones);
                }

                if (ParametrosPeticion.Usuario != "usuario" && ParametrosPeticion.Usuario != "usuarioAdmin")
                {
                    var validacion = new ValidacionException.Validacion
                    {
                        Mensaje = "El usuario o la contraseña es incorrecta",
                    };

                    validaciones.Add(validacion);

                    throw new ValidacionException(validaciones);
                }
            }

            public class Resultado : IResultado
            {
                public string Rol { get; set; }
            }

            public class Parametros
            {
                public string Usuario { get; set; }

                public string Password { get; set; }
            }
        }
    }
}
