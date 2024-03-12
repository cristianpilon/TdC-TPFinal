using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.AuditoriasAplicacion;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditoriasController : AppController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerTodos()
        {
            var peticion = new PeticionObtenerTodos(new RepositorioAuditoriaAplicacion());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
