using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Cursos
{
    public class PeticionAgregar : PeticionTransaccionableBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionAgregar(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var curso = new Curso(
                ParametrosPeticion.Titulo,
                ParametrosPeticion.Mensaje,
                new Empresa(ParametrosPeticion.IdEmpresa),
                ParametrosPeticion.Etiquetas,
                ParametrosPeticion.Perfiles);

            var respuestaNuevoCurso = ((IRepositorioCursos)Repositorio).Agregar(ParametrosPeticion.IdUsuario, curso);

            return new Resultado { Id = respuestaNuevoCurso.Id };
        }

        public class Resultado : IResultado
        {
            public int Id { get; set; }
        }

        public class Parametros
        {
            public int IdUsuario { get; set; }

            public string Titulo { get; set; }

            public string Mensaje { get; set; }

            public int IdEmpresa { get; set; }

            public IEnumerable<int> Perfiles { get; set; }

            public IEnumerable<int> Etiquetas { get; set; }
        }
    }
}
