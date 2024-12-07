using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Models.Dtos;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;

namespace GestorCV.API.Controllers.Servicios.Curriculums
{
    public class PeticionModificar: PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionModificar(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var curriculum = new Curriculum(ParametrosPeticion.Curriculum);
            ((IRepositorioCurriculums)Repositorio).Modificar(ParametrosPeticion.UsuarioId.Value, curriculum);

            return new Resultado();
        }

        public class Resultado : IResultado
        {
        }

        public class Parametros
        {
            public int? UsuarioId { get; set; }

            public ModificarCurriculum Curriculum { get; set; }
        }
    }
}
