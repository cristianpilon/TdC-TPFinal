using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Reportes;
using GestorCV.API.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GestorCV.API.Controllers
{
    [ApiController]
    [Route("reportes")]
    public class ReportesController : AppController
    {
        [HttpGet("empleos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Empleos()
        {
            var peticion = new PeticionEmpleos(new PeticionEmpleos.Parametros(UsuarioNombre, null), new RepositorioEmpleos());
            var resultado = (PeticionEmpleos.Resultado)EjecutorPeticiones.Ejecutar(peticion);

            string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), resultado.Archivo);

            if (!System.IO.File.Exists(rutaArchivo))
            {
                // 404 si no encuentra el archivo
                return NotFound();
            }

            var fileInfo = new System.IO.FileInfo(rutaArchivo);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Envia archivo
            return File(System.IO.File.ReadAllBytes(rutaArchivo), "application/pdf", fileInfo.Name);
        }
    }
}
