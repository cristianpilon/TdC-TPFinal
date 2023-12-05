using GestorCV.API.Repositorios.Interfaces;

namespace GestorCV.API.Controllers.Servicios.Interfaces
{
    public interface IPeticion
    {
        public IRepositorio Repositorio { get; }

        public IResultado ProcesarExtendido();
        
        public IResultado Procesar();

        public void Validar();
    }
}
