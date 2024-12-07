using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;
using GestorCV.API.Repositorios;

namespace GestorCV.API.Controllers.Servicios.Respaldos
{
    public class PeticionAgregar : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionAgregar(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            ((IRepositorioRespaldos)Repositorio).Agregar(ParametrosPeticion.TipoRespaldo);

            return new Resultado();
        }

        public class Resultado : IResultado
        {
        }

        public class Parametros
        {
            public Parametros(string tipoRespaldo)
            {
                TipoRespaldo = tipoRespaldo;
            }

            public string TipoRespaldo { get; private set; }
        }
    }
}
