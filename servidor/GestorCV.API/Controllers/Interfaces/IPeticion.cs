using GestorCV.API.Infraestructura;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Interfaces
{
    public interface IPeticion
    {
        public IRepositorio Repositorio { get; }

        public IResultado ProcesarExtendido();
        
        public IResultado Procesar();

        public List<ValidacionException.Validacion> Validar();
    }
}
