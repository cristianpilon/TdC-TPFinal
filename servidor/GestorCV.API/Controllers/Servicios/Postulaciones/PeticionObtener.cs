using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Controllers.Servicios.Postulaciones
{
    public class PeticionObtener : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        private readonly IRepositorioEmpresas repositorioEmpresas;

        public PeticionObtener(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
            repositorioEmpresas = new RepositorioEmpresas();
        }

        public override IResultado Procesar()
        {
            var postulacion = ((IRepositorioPostulaciones)Repositorio).Obtener(ParametrosPeticion.Id);

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
                Estado = postulacion.Estado,
                Fecha = postulacion.Fecha,
                Curriculum = postulacion.Curriculum,
                Usuario = $"{postulacion.Usuario.Nombre} {postulacion.Usuario.Apellido} ({postulacion.Usuario.Correo})",
                Empleo = new Resultado.EmpleoItem
                {
                    Titulo = postulacion.Empleo.Titulo,
                    Mensaje = postulacion.Empleo.Descripcion,
                    Ubicacion = postulacion.Empleo.Ubicacion,
                    ModalidadTrabajo = postulacion.Empleo.ModalidadTrabajo,
                    TipoTrabajo = postulacion.Empleo.TipoTrabajo,
                    HorarioLaboral = postulacion.Empleo.HorariosLaborales,
                    Remuneracion = postulacion.Empleo.Remuneracion,
                    IdEmpresa = postulacion.Empleo.Empresa.Id,
                    Destacado = postulacion.Empleo.Destacado,
                    Etiquetas = postulacion.Empleo.Etiquetas.Select(x => new Models.Dtos.Etiqueta(x.Id, x.Nombre)),
                    Perfiles = postulacion.Empleo.Perfiles.Select(x => new Models.Dtos.Perfil(x.Id, x.Nombre)),
                },
                Empresas = empresas
            };
        }

        public class Resultado : IResultado
        {
            public string Usuario { get; set; }

            public string Estado { get; set; }

            public DateTime Fecha { get; set; }

            public EmpleoItem Empleo { get; set; }

            public Models.Dtos.Curriculum Curriculum { get; set; }

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

                public IEnumerable<Models.Dtos.Etiqueta> Etiquetas { get; set; }

                public IEnumerable<Models.Dtos.Perfil> Perfiles { get; set; }
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
