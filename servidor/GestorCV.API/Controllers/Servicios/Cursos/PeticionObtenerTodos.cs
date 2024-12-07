using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Cursos
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
            var cursos = ((IRepositorioCursos)Repositorio).ObtenerTodos();

            return new Resultado { Cursos = cursos };
        }

        public class Resultado : IResultado
        {
            public List<Models.Dtos.Curso> Cursos { get; set; }
        }

        public class Parametros
        {}
    }
}
