using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Empleos
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
            var empleo = new Models.Dtos.Empleo(ParametrosPeticion.Titulo, ParametrosPeticion.Mensaje, ParametrosPeticion.Ubicacion,
                ParametrosPeticion.Remuneracion, ParametrosPeticion.ModalidadTrabajo, ParametrosPeticion.HorarioLaboral,
                ParametrosPeticion.TipoTrabajo, ParametrosPeticion.IdEmpresa, ParametrosPeticion.Etiquetas, ParametrosPeticion.Perfiles,
                ParametrosPeticion.Destacado);

            var respuestaNuevoEmpleo = ((IRepositorioEmpleos)Repositorio).Agregar(ParametrosPeticion.IdUsuario, empleo);

            return new Resultado { Id = respuestaNuevoEmpleo.Id };
        }

        public class Resultado : IResultado
        {
            public int Id { get; set; }
        }

        public class Parametros
        {
            public string RolUsuario { get; set; }

            public int IdUsuario { get; set; }

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
    }
}
