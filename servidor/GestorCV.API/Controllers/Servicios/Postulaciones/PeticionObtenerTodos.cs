using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Postulaciones
{
    public class PeticionObtenerTodos : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionObtenerTodos(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var postulaciones = ((IRepositorioPostulaciones)Repositorio).ObtenerTodos(ParametrosPeticion.IdUsuario);

            return new Resultado { Postulaciones = postulaciones };
        }

        public class Resultado : IResultado
        {
            public List<Models.Dtos.Postulacion> Postulaciones { get; set; }
        }

        public class Parametros
        {
            public Parametros(int? idUsuario)
            {
                IdUsuario = idUsuario;
            }

            public int? IdUsuario { get; private set; }
        }
    }
}
