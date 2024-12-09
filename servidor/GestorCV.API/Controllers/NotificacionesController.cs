using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Notificaciones;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificacionesController : AppController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerTodos(PeticionObtenerTodos.Parametros parametros)
        {
            parametros.IdUsuario = UsuarioId;

            var peticion = new PeticionObtenerTodos(parametros, new RepositorioNotificaciones());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Modificar(PeticionModificar.Parametros parametros)
        {
            parametros.IdUsuario = UsuarioId;

            var peticion = new PeticionModificar(parametros, new RepositorioPostulaciones());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
