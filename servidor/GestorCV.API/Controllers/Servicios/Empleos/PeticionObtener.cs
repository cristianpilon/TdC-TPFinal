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
            var empleo = ((IRepositorioEmpleos)Repositorio).Obtener(ParametrosPeticion.Id);

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

            return new Resultado 
            { 
                Empleo = new Resultado.EmpleoItem 
                { 
                    Titulo = empleo.Titulo,
                    Mensaje = empleo.Descripcion,
                    Ubicacion = empleo.Ubicacion,
                    ModalidadTrabajo = empleo.ModalidadTrabajo,
                    TipoTrabajo = empleo.TipoTrabajo,
                    HorarioLaboral = empleo.HorariosLaborales,
                    Remuneracion = empleo.Remuneracion,
                    IdEmpresa = empleo.Empresa.Id,
                    Destacado = empleo.Destacado,
                    Etiquetas = empleo.Etiquetas.Select(x => x.Id),
                    Perfiles = empleo.Perfiles.Select(x => x.Id),
                }, 
                Perfiles = perfiles, Etiquetas = etiquetas, Empresas = empresas };
        }

        public class Resultado : IResultado
        {
            public EmpleoItem Empleo { get; set; }

            public List<Models.Dtos.Etiqueta> Etiquetas { get; set; }

            public List<Models.Dtos.Perfil> Perfiles { get; set; }

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
