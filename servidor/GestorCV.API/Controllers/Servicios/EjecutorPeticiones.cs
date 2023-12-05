using GestorCV.API.Controllers.Servicios.Interfaces;

namespace GestorCV.API.Controllers.Servicios
{
    public class EjecutorPeticiones
    {
        public IResultado Ejecutar(IPeticion peticion)
        {
            // Ejecuta las validaciones de la petición
            peticion.Validar();

            // Procesa y retorna el resultado de la petición
            return peticion.ProcesarExtendido();
        }
    }
}
