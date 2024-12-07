using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Curriculums;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurriculumsController : AppController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Obtener()
        {
            var idUsuario = UsuarioId;
            var peticion = new PeticionObtener(new PeticionObtener.Parametros(idUsuario), new RepositorioCurriculums());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Modificar(PeticionModificar.Parametros parametros)
        {
            var idUsuario = UsuarioId;
            parametros.UsuarioId = idUsuario;
            var peticion = new PeticionModificar(parametros, new RepositorioCurriculums());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
