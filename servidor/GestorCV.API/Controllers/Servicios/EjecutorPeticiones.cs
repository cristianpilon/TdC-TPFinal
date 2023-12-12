using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Infraestructura;

namespace GestorCV.API.Controllers.Servicios
{
    public class EjecutorPeticiones
    {
        public IResultado Ejecutar(IPeticion peticion)
        {
            // Ejecuta las validaciones de la petición
            var validaciones = peticion.Validar();

            if (validaciones.Count > 0)
            {
                // Si hay validaciones, devuelvo una excepción
                throw new ValidacionException(validaciones);
            }

            // Procesa y retorna el resultado de la petición
            return peticion.ProcesarExtendido();
        }
    }
}
