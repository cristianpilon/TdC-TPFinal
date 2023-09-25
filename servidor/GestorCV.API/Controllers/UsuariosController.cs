using GestorCV.API.Controllers.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GestorCV.API.Controllers.Servicios.ServicioUsuarios;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        EjecutorPeticiones ejecutorPeticiones { get; set; }

        public UsuariosController()
        {
            ejecutorPeticiones = new EjecutorPeticiones();    
        }

        [HttpPost("validar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Validar(PeticionValidarUsuario.Parametros parametros)
        {
            var peticion = new PeticionValidarUsuario(parametros);
            var resultado = ejecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
