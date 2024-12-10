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
            Resultado.CursoItem curso = null;

            var perfiles = repositorioPerfiles.ObtenerTodos();
            var etiquetas = repositorioEtiquetas.ObtenerTodos();
            var empresas = repositorioEmpresas.ObtenerTodos();

            if (ParametrosPeticion.Id > 0)
            {
                var cursoDto = ((IRepositorioCursos)Repositorio).Obtener(ParametrosPeticion.Id);
                curso = new Resultado.CursoItem
                {
                    Titulo = cursoDto.Titulo,
                    Mensaje = cursoDto.Mensaje,
                    Fecha = cursoDto.Fecha,
                    IdEmpresa = cursoDto.Empresa.Id,
                    Etiquetas = cursoDto.Etiquetas.Select(x => x.Id),
                    Perfiles = cursoDto.Perfiles.Select(x => x.Id),
                };
            }

            return new Resultado
            {
                Curso = curso,
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
