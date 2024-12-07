using GestorCV.API.Models;
using GestorCV.API.Repositorios.Interfaces;
using System.Diagnostics;

namespace GestorCV.API.Repositorios.Base
{
    [DebuggerStepThrough]
    public abstract class RepositorioBase : IRepositorio
    {
        protected readonly GestorCurriculumsContext _contexto;

        public RepositorioBase()
        {
            _contexto = new GestorCurriculumsContext();
        }

        public void AnularTransaccion()
        {

        }

        public void ConfirmarTransaccion() 
        { 
            
        }

        public void IniciarTransaccion() 
        {
        
        }
    }
}
