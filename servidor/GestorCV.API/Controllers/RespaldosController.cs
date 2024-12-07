using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios;
using GestorCV.API.Controllers.Servicios.Respaldos;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RespaldosController : AppController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerTodos()
        {
            var peticion = new PeticionObtenerTodos(new RepositorioRespaldos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Agregar(PeticionAgregar.Parametros parametros)
        {
            var peticion = new PeticionAgregar(parametros, new RepositorioRespaldos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Modificar(PeticionModificar.Parametros parametros)
        {
            var peticion = new PeticionModificar(parametros, new RepositorioRespaldos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Eliminar(PeticionEliminar.Parametros parametros)
        {
            var peticion = new PeticionEliminar(parametros, new RepositorioRespaldos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
