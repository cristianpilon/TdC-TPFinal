﻿using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Postulaciones;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostulacionesController : AppController
    {
        [HttpPost("{idEmpleo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Agregar(int idEmpleo)
        {
            var idUsuario = UsuarioId;
            var correoUsuario = UsuarioCorreo;
            var parametros = new PeticionAgregar.Parametros(idEmpleo, idUsuario, correoUsuario);

            var peticion = new PeticionAgregar(parametros, new RepositorioPostulaciones());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerTodos()
        {
            int? idUsuario = null;

            if (UsuarioRol.Nombre != "Administrador")
            {
                idUsuario= UsuarioId;
            }

            var parametros = new PeticionObtenerTodos.Parametros(idUsuario);

            var peticion = new PeticionObtenerTodos(parametros, new RepositorioPostulaciones());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Obtener(int id)
        {
            var parametros = new PeticionObtener.Parametros(id, UsuarioRol.Nombre);
            
            var peticion = new PeticionObtener(parametros, new RepositorioPostulaciones());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }

        [HttpPut("{idPostulacion}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Modificar(int idPostulacion, PeticionModificar.Parametros parametros)
        {
            parametros.IdPostulacion = idPostulacion;

            var peticion = new PeticionModificar(parametros, new RepositorioPostulaciones());
            var resultado = EjecutorPeticiones.Ejecutar(peticion);

            return Ok(resultado);
        }
    }
}
