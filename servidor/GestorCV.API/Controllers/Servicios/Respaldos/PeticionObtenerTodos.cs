using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;
using GestorCV.API.Repositorios;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Controllers.Servicios.Respaldos
{
    public class PeticionObtenerTodos : PeticionBase
    {
        public PeticionObtenerTodos(IRepositorio repositorio)
                : base(repositorio)
        {
        }

        public override IResultado Procesar()
        {
            var respaldos = ((IRepositorioRespaldos)Repositorio).ObtenerTodos();
            var resultadoRespaldos = respaldos.Select(x => new Resultado.Respaldo 
            {
                Fecha = x.Fecha.ToString("dd/MM/yyyy hh:mm:ss"),
                Tipo = x.Tipo,
            }).ToList();

            return new Resultado { Respaldos = resultadoRespaldos };
        }

        public class Resultado : IResultado
        {
            public class Respaldo
            {
                public string Fecha { get; set; }

                public string Tipo { get; set; }
            }

            public List<Respaldo> Respaldos { get; set; }
        }
    }
}
