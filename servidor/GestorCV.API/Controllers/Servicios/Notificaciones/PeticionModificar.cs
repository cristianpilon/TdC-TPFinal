using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System;

namespace GestorCV.API.Controllers.Servicios.Notificaciones
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
            ((IRepositorioNotificaciones)Repositorio).Modificar(ParametrosPeticion.IdUsuario, ParametrosPeticion.Limit);

            return new Resultado();
        }

        public class Resultado : IResultado
        {
        }

        public class Parametros
        {
            public int IdUsuario { get; set; }

            public DateTime Limit { get; set; }
        }
    }
}
