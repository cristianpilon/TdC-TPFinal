using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios;
using GestorCV.API.Controllers.Servicios.Empleos;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpleosController : AppController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerTodos()
        {
            var peticion = new PeticionObtenerTodos(new RepositorioEmpleos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Obtener(int id)
        {
            var parametros = new PeticionObtener.Parametros(id);
            var peticion = new PeticionObtener(parametros, new RepositorioEmpleos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
