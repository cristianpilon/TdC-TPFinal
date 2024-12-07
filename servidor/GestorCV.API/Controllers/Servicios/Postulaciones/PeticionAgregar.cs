using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Infraestructura;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Postulaciones
{
    public class PeticionAgregar : PeticionTransaccionableBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionAgregar(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var nuevaPostulacion = new Models.Dtos.Postulacion(ParametrosPeticion.IdEmpleo, ParametrosPeticion.IdUsuario);
            var respuestaNuevaPostulacion = ((IRepositorioPostulaciones)Repositorio).Agregar(nuevaPostulacion);

            EnviarCorreoUsuario(respuestaNuevaPostulacion);

            return new Resultado { Id = respuestaNuevaPostulacion.Id };
        }

        private void EnviarCorreoUsuario(RepositorioPostulaciones.RespuestaAgregarPostulacion respuestaNuevaPostulacion)
        {
            ServicioCorreo.Instancia.EnviarCorreo(
                AppConfiguration.SmtpCorreoOrigen,
                ParametrosPeticion.CorreoUsuario,
                $"{respuestaNuevaPostulacion.Usuario} - Nueva postulación a empleo '{respuestaNuevaPostulacion.Empleo}'",
                $"Estimado {respuestaNuevaPostulacion.Usuario}, se ha publicado la propuesta al empleo {respuestaNuevaPostulacion.Empleo}. En breve la organización revisará su perfil e informará los resultados. Muchas Gracias");

        }

        public override List<ValidacionException.Validacion> Validar()
        {
            var validaciones = new List<ValidacionException.Validacion>();

            if (ParametrosPeticion.IdUsuario == 0)
            {
                var validacion = new ValidacionException.Validacion("Debe ingresar un usuario válido");
                validaciones.Add(validacion);
            }

            if (ParametrosPeticion.IdEmpleo == 0)
            {
                var validacion = new ValidacionException.Validacion("Debe ingresar un empleo válido");
                validaciones.Add(validacion);
            }

            return validaciones;
        }

        public class Resultado : IResultado
        {
            public int Id { get; set; }
        }

        public class Parametros
        {
            public Parametros(int idEmpleo, int idUsuario, string correoUsuario)
            {
                IdEmpleo = idEmpleo;
                IdUsuario = idUsuario;
                CorreoUsuario = correoUsuario;
            }
            public int IdEmpleo { get; private set; }

            public int IdUsuario { get; private set; }

            public string CorreoUsuario { get; private set; }
        }
    }
}
