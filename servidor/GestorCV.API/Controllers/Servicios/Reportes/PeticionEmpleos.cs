using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Controllers.Servicios.Reportes.Factoria;

namespace GestorCV.API.Controllers.Servicios.Reportes
{
    public class PeticionEmpleos : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionEmpleos(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var empleos = ((IRepositorioEmpleos)Repositorio).ObtenerTodos();
            var nombreArchivo = FactoriaReporteEmpleos.Crear(empleos, ParametrosPeticion.Usuario);

            return new Resultado { Archivo = nombreArchivo };
        }

        public class Parametros
        {
            public Parametros(string usuario)
            {
                Usuario = usuario;
            }

            public string Usuario { get; private set; }
        }

        public class Resultado : IResultado
        {
            public string Archivo { get; set; }
        }
    }
}
