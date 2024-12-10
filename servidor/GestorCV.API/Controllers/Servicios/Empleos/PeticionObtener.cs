using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Controllers.Servicios.Empleos
{
    public class PeticionObtener : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        private readonly IRepositorioPerfiles repositorioPerfiles;
        private readonly IRepositorioEtiquetas repositorioEtiquetas;
        private readonly IRepositorioEmpresas repositorioEmpresas;
        private readonly IRepositorioCursos repositorioCursos;

        public PeticionObtener(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
            repositorioPerfiles = new RepositorioPerfiles();
            repositorioEtiquetas = new RepositorioEtiquetas();
            repositorioEmpresas = new RepositorioEmpresas();
            repositorioCursos = new RepositorioCursos();
        }

        public override IResultado Procesar()
        {
            Resultado.EmpleoItem empleo = null;
            List<Models.Dtos.Curso> cursos = null;

            if (ParametrosPeticion.Id > 0)
            {
                var empleoDto = ((IRepositorioEmpleos)Repositorio).Obtener(ParametrosPeticion.Id);
                empleo = new Resultado.EmpleoItem
                {
                    Titulo = empleoDto.Titulo,
                    Mensaje = empleoDto.Descripcion,
                    Ubicacion = empleoDto.Ubicacion,
                    ModalidadTrabajo = empleoDto.ModalidadTrabajo,
                    TipoTrabajo = empleoDto.TipoTrabajo,
                    HorarioLaboral = empleoDto.HorariosLaborales,
                    Remuneracion = empleoDto.Remuneracion,
                    IdEmpresa = empleoDto.Empresa.Id,
                    Destacado = empleoDto.Destacado,
                    Etiquetas = empleoDto.Etiquetas.Select(x => x.Id),
                    Perfiles = empleoDto.Perfiles.Select(x => x.Id),
                };
            }

            var perfiles = repositorioPerfiles.ObtenerTodos();
            var etiquetas = repositorioEtiquetas.ObtenerTodos();
            IEnumerable<Resultado.EmpresaItem> empresas = null;

            if (ParametrosPeticion.RolUsuario == "Administrador")
            {
                empresas = repositorioEmpresas.ObtenerTodos().Select(x => new Resultado.EmpresaItem
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                });
            }
            else if (ParametrosPeticion.RolUsuario == "Usuario")
            {
                cursos = repositorioCursos.ObtenerTodosPorEmpleo(ParametrosPeticion.Id);
            }

            return new Resultado
            {
                Empleo = empleo,
                Perfiles = perfiles,
                Etiquetas = etiquetas,
                Empresas = empresas,
                Cursos = cursos,
            };
        }

        public class Resultado : IResultado
        {
            public EmpleoItem Empleo { get; set; }

            public List<Models.Dtos.Etiqueta> Etiquetas { get; set; }

            public List<Models.Dtos.Perfil> Perfiles { get; set; }

            public List<Models.Dtos.Curso> Cursos { get; set; }

            public IEnumerable<EmpresaItem> Empresas { get; set; }

            public class EmpleoItem
            {
                public string Titulo { get; set; }

                public string Mensaje { get; set; }

                public string Ubicacion { get; set; }

                public string ModalidadTrabajo { get; set; }

                public string TipoTrabajo { get; set; }

                public string HorarioLaboral { get; set; }

                public decimal? Remuneracion { get; set; }

                public bool Destacado { get; set; }

                public int IdEmpresa { get; set; }

                public IEnumerable<int> Perfiles { get; set; }

                public IEnumerable<int> Etiquetas { get; set; }
            }

            public class EmpresaItem
            {
                public int Id { get; set; }

                public string Nombre { get; set; }
            }
        }

        public class Parametros
        {
            public Parametros(int id, string rolUsuario)
            {
                Id = id;
                RolUsuario = rolUsuario;
            }

            public string RolUsuario { get; private set; }

            public int Id { get; private set; }
        }
    }
}
