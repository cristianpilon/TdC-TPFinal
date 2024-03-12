using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;
using GestorCV.API.Repositorios;
using System;

namespace GestorCV.API.Controllers.Servicios.Respaldos
{
    public class PeticionModificar : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionModificar(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            ((IRepositorioRespaldos)Repositorio).Modificar(ParametrosPeticion.Fecha, ParametrosPeticion.TipoRespaldo);

            return new Resultado();
        }

        public class Resultado : IResultado
        {
        }

        public class Parametros
        {
            public DateTime Fecha { get; set; }

            public string TipoRespaldo { get; set; }
        }
    }
}
