using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;

namespace GestorCV.API.Controllers.Base
{
    public abstract class PeticionTransaccionableBase : PeticionBase
    {
        public PeticionTransaccionableBase(IRepositorio repositorio)
            : base(repositorio) { }

        public override IResultado ProcesarExtendido()
        {
            Repositorio.IniciarTransaccion();
            
            try
            {
                var resultado = Procesar();

                Repositorio.ConfirmarTransaccion();

                return resultado;
            }
            catch (System.Exception)
            {
                Repositorio.AnularTransaccion();
                throw;
            }
        }
    }
}
