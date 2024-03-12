using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;
using GestorCV.API.Repositorios;
using System.Text.RegularExpressions;

namespace GestorCV.API.Controllers.Servicios.AuditoriasAplicacion
{
    public class PeticionAgregar : PeticionBase
    {
        private const string RutaLogin = "/usuarios/validar";
        private const string AccionPost = "POST";

        public Parametros ParametrosPeticion { get; private set; }

        public PeticionAgregar(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            // Modifico la contraseña del cuerpo del mensaje para no exponerlo
            if (ParametrosPeticion.Ruta.Contains(RutaLogin) && ParametrosPeticion.Accion == AccionPost)
            {
                var rutaPasswordOculto = OcultarContraseña(ParametrosPeticion.Ruta);
                ParametrosPeticion.ModificarRuta(rutaPasswordOculto);
            }

            ((IRepositorioAuditoriasAplicacion)Repositorio).Agregar(ParametrosPeticion.IdUsuario, ParametrosPeticion.Ruta, ParametrosPeticion.Accion);

            return new Resultado();
        }

        static string OcultarContraseña(string input)
        {
            // Patrón de expresión regular para encontrar la contraseña
            string patron = @"""password""\s*:\s*""([^""]*)""";

            // Reemplazar la contraseña con "***"
            string resultado = Regex.Replace(input, patron, "***");

            return resultado;
        }

        public class Resultado : IResultado
        {
        }

        public class Parametros
        {
            internal void ModificarRuta(string nuevaRuta) {
                Ruta = nuevaRuta;
            }

            public Parametros(int? idUsuarios, string ruta, string accion)
            {
                IdUsuario = idUsuarios;
                Ruta = ruta;
                Accion = accion;
            }

            public int? IdUsuario { get; private set; }

            public string Ruta { get; private set; }

            public string Accion { get; private set; }
        }
    }
}
