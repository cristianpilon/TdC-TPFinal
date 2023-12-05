using GestorCV.API.Controllers.Servicios;
using GestorCV.API.Controllers.Servicios.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        EjecutorPeticiones EjecutorPeticiones { get; set; }

        public UsuariosController()
        {
            EjecutorPeticiones = new EjecutorPeticiones();    
        }

        [HttpPost("validar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Validar(ValidarUsuario.Parametros parametros)
        {
            var peticion = new ValidarUsuario(parametros);
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
