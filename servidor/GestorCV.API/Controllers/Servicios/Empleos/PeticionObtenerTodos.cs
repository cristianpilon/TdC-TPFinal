using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Empleos
{
    public class PeticionObtenerTodos : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionObtenerTodos(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var empleos = ((IRepositorioEmpleos)Repositorio).ObtenerTodos(ParametrosPeticion.Criterio, ParametrosPeticion.IdUsuario);

            return new Resultado { Empleos = empleos };
        }

        public class Resultado : IResultado
        {
            public List<Models.Dtos.Empleo> Empleos { get; set; }
        }

        public class Parametros
        {
            public int? IdUsuario { get; set; }

            public Parametros(string criterio)
            {
                Criterio = criterio;
            }

            public string Criterio { get; private set; }
        }
    }
}
