using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;

namespace GestorCV.API.Controllers.Servicios.Base
{
    public abstract class PeticionTransaccionableBase : PeticionBase
    {
        public override IResultado ProcesarExtendido() 
        {
            try
            {
                Repositorio.IniciarTransaccion();
                
                var resultado = base.Procesar();

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
