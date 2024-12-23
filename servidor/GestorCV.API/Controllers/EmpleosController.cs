﻿using GestorCV.API.Controllers.Base;
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("busqueda-avanzada")]
        public IActionResult ObtenerTodosAvanzado(PeticionObtenerTodosAvanzado.Parametros parametros)
        {
            var peticion = new PeticionObtenerTodosAvanzado(parametros, new RepositorioEmpleos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerTodos([FromQuery] string criterio)
        {
            var parametros = new PeticionObtenerTodos.Parametros(criterio);

            if (UsuarioRol.Nombre == "Reclutador")
            {
                parametros.IdUsuario = UsuarioId;
            }

            var peticion = new PeticionObtenerTodos(parametros, new RepositorioEmpleos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("sugeridos")]
        public IActionResult ObtenerTodosSeguridos()
        {
            var idUsuario = UsuarioId;

            var peticion = new PeticionObtenerTodosSugeridos(new PeticionObtenerTodosSugeridos.Parametros(idUsuario), new RepositorioEmpleos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("etiquetas-perfiles")]
        public IActionResult ObtenerTodosEtiquetasPerfiles()
        {
            var peticion = new PeticionObtenerTodosEtiquetasPerfiles(new PeticionObtenerTodosEtiquetasPerfiles.Parametros(), null);
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Obtener(int id)
        {
            var parametros = new PeticionObtener.Parametros(id, UsuarioRol.Nombre);

            var peticion = new PeticionObtener(parametros, new RepositorioEmpleos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpPut("{idEmpleo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Modificar(int idEmpleo, PeticionModificar.Parametros parametros)
        {
            parametros.IdEmpleo = idEmpleo;
            parametros.RolUsuario = UsuarioRol.Nombre;

            var peticion = new PeticionModificar(parametros, new RepositorioEmpleos());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Agregar(PeticionAgregar.Parametros parametros)
        {
            parametros.IdUsuario = UsuarioId;

            if (UsuarioRol.Nombre == "Reclutador")
            {
                parametros.IdEmpresa = UsuarioEmpresa.Id;
                parametros.Destacado = false;
            }

            var peticion = new PeticionAgregar(parametros, new RepositorioEmpleos());
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
