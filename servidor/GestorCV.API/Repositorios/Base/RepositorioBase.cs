using GestorCV.API.Models;
using GestorCV.API.Repositorios.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GestorCV.API.Repositorios.Base
{
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
