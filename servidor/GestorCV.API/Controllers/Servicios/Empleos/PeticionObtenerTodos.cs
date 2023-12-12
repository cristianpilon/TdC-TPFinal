using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Controllers.Servicios.Empleos
{
    public class PeticionObtenerTodos : PeticionBase
    {
        public PeticionObtenerTodos(IRepositorio repositorio)
                : base(repositorio)
        {
        }

        public override IResultado Procesar()
        {
            var empleos = ((IRepositorioEmpleos)Repositorio).ObtenerTodos();

            return new Resultado { Empleos = empleos };
        }

        public class Resultado : IResultado
        {
            public List<Models.Dtos.Empleo> Empleos { get; set; }
        }
    }
}
