using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;

namespace GestorCV.API.Controllers.Servicios.Notificaciones
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
            var nuevaNotificacion = new Models.Dtos.Notificacion(ParametrosPeticion.IdUsuario, ParametrosPeticion.Mensaje);
            ((IRepositorioNotificaciones)Repositorio).Agregar(nuevaNotificacion);

            return new Resultado();
        }

        public class Resultado : IResultado
        {}

        public class Parametros
        {
            public Parametros(int idUsuario, string mensaje)
            {
                IdUsuario = idUsuario;
                Mensaje = mensaje;
            }

            public int IdUsuario { get; private set; }

            public string Mensaje { get; private set; }
        }
    }
}
