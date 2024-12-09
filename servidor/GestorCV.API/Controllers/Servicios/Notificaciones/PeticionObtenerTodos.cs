using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Notificaciones
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
            var notificaciones = ((IRepositorioNotificaciones)Repositorio).ObtenerTodos(ParametrosPeticion.IdUsuario, ParametrosPeticion.Limit);

            return new Resultado { Notificaciones = notificaciones };
        }

        public class Resultado : IResultado
        {
            public List<Models.Dtos.Notificacion> Notificaciones { get; set; }
        }

        public class Parametros
        {
            public int IdUsuario { get; set; }

            public DateTime? Limit { get; set; }
        }
    }
}
