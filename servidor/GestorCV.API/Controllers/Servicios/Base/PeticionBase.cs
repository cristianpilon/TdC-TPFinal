using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;

namespace GestorCV.API.Controllers.Servicios.Base
{
    public abstract class PeticionBase : IPeticion
    {
        public IRepositorio Repositorio { get; set; }

        public virtual IResultado Procesar()
        {
            throw new System.NotImplementedException();
        }

        public virtual IResultado ProcesarExtendido()
        {
            return Procesar();
        }

        public abstract void Validar();
    }
}
