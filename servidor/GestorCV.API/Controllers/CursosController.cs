using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios;
using GestorCV.API.Controllers.Servicios.Cursos;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursosController : AppController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerTodos()
        {
            var peticion = new PeticionObtenerTodos(new PeticionObtenerTodos.Parametros(), new RepositorioCursos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Obtener(int id)
        {
            var parametros = new PeticionObtener.Parametros(id);
            var peticion = new PeticionObtener(parametros, new RepositorioCursos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
