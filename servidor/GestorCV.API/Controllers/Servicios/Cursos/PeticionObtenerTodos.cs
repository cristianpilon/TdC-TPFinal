using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

            return new Resultado
            {
                Cursos = cursos.Select(x => new Resultado.Item
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Fecha = x.Fecha,
                    Empresa = x.Empresa.Nombre,
                }).ToList()
            };
        }

        public class Resultado : IResultado
        {
            public List<Item> Cursos { get; set; }

            public class Item
            {
                public int Id { get; set; }

                public string Titulo { get; set; }

                public DateTime Fecha { get; set; }

                public string Empresa { get; set; }
            }
        }
        public class Parametros
        { }
    }
}
