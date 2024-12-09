using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Controllers.Servicios.Cursos
{
    public class PeticionObtener : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        private readonly IRepositorioPerfiles repositorioPerfiles;
        private readonly IRepositorioEtiquetas repositorioEtiquetas;
        private readonly IRepositorioEmpresas repositorioEmpresas;

        public PeticionObtener(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
            repositorioPerfiles = new RepositorioPerfiles();
            repositorioEtiquetas = new RepositorioEtiquetas();
            repositorioEmpresas = new RepositorioEmpresas();
        }

        public override IResultado Procesar()
        {
            var curso = ((IRepositorioCursos)Repositorio).Obtener(ParametrosPeticion.Id);

            var perfiles = repositorioPerfiles.ObtenerTodos();
            var etiquetas = repositorioEtiquetas.ObtenerTodos();
            var empresas = repositorioEmpresas.ObtenerTodos();

            return new Resultado
            {
                Curso = new Resultado.CursoItem
                {
                    Titulo = curso.Titulo,
                    Mensaje = curso.Mensaje,
                    Fecha = curso.Fecha,
                    IdEmpresa = curso.Empresa.Id,
                    Etiquetas = curso.Etiquetas.Select(x => x.Id),
                    Perfiles = curso.Perfiles.Select(x => x.Id),
                },
                Perfiles = perfiles,
                Etiquetas = etiquetas,
                Empresas = empresas
            };
        }

        public class Resultado : IResultado
        {
            public CursoItem Curso { get; set; }

            public List<Models.Dtos.Etiqueta> Etiquetas { get; set; }

            public List<Models.Dtos.Perfil> Perfiles { get; set; }

            public List<Models.Dtos.Empresa> Empresas { get; set; }

            public class CursoItem
            {
                public string Titulo { get; set; }

                public string Mensaje { get; set; }

                public DateTime Fecha { get; set; }

                public int IdEmpresa { get; set; }

                public IEnumerable<int> Perfiles { get; set; }

                public IEnumerable<int> Etiquetas { get; set; }
            }
        }

        public class Parametros
        {
            public Parametros(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }
    }
}
