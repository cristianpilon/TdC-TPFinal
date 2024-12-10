using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Notificaciones;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
            if (parametros == null)
            {
                parametros = new PeticionObtenerTodos.Parametros();
            }
            else
            {
                if (!parametros.Limit.HasValue)
                {
                    parametros.Limit = DateTime.UtcNow;
                }
            }

            parametros.IdUsuario = UsuarioId;

            var peticion = new PeticionObtenerTodos(parametros, new RepositorioNotificaciones());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Modificar()
        {
            var parametros = new PeticionModificar.Parametros
            {
                IdUsuario = UsuarioId,
                Limit = DateTime.UtcNow
            };

            var peticion = new PeticionModificar(parametros, new RepositorioNotificaciones());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
