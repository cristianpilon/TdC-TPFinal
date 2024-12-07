using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Infraestructura;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace GestorCV.API.Controllers.Base
{
    [DebuggerStepThrough]
    public abstract class PeticionBase : IPeticion
    {
        public PeticionBase(IRepositorio repositorio)
        {
            Repositorio = repositorio;
        }

        public IRepositorio Repositorio { get; private set; }

        public virtual IResultado Procesar()
        {
            throw new System.NotImplementedException();
        }

        public virtual IResultado ProcesarExtendido()
        {
            return Procesar();
        }

        public virtual List<ValidacionException.Validacion> Validar()
        {
            return new List<ValidacionException.Validacion>();
        }
    }
}
