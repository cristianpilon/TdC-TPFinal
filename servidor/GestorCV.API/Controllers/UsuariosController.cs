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
        [HttpPost("validar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Validar(PeticionValidarUsuario.Parametros parametros)
        {
            var peticion = new PeticionValidarUsuario();
            var resultado = peticion.Procesar(parametros);

            if (resultado.Validaciones.Count > 0)
            {
                return BadRequest(resultado.Validaciones);
            }

            return Ok(new { resultado.Rol });
        }
    }
}
