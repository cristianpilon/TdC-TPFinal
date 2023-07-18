using System;

namespace GestorCV.API.Controllers.Servicios
{
    public class ServicioUsuarios
    {
        public class PeticionValidarUsuario
        {
            public Resultado Procesar(Parametros parametros)
            {
                var resultado = new Resultado();

                if (parametros.Password != "123456")
                {
                    resultado.Validaciones.Add(new ResultadoBase.ResultadoValidacion
                    {
                        Mensaje = "El usuario o la contraseña es incorrecta"
                    });

                    return resultado;
                }

                if (parametros.Usuario != "usuario" && parametros.Usuario != "usuarioAdmin")
                {
                    resultado.Validaciones.Add(new ResultadoBase.ResultadoValidacion
                    {
                        Mensaje = "El usuario o la contraseña es incorrecta"
                    });

                    return resultado;
                }

                var tipoUsuario = parametros.Usuario == "usuarioAdmin" ? "Admin" : "Usuario";

                return new Resultado { Rol = tipoUsuario };
            }

            public class Resultado : ResultadoBase
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
