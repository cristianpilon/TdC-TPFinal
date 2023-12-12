using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;

namespace GestorCV.API.Controllers.Servicios.Empleos
{
    public class PeticionObtener : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionObtener(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var empleo = ((IRepositorioEmpleos)Repositorio).Obtener(ParametrosPeticion.Id);

            return new Resultado { Empleo = empleo };
        }

        public class Resultado : IResultado
        {
            public Models.Dtos.Empleo Empleo { get; set; }
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
