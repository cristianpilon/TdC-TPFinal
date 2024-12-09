using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Empleos
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
            bool? destacado = null;

            if (ParametrosPeticion.RolUsuario == "Administrador")
            {
                destacado = ParametrosPeticion.Destacado;
            }

            var empleo = new Models.Dtos.Empleo(ParametrosPeticion.IdEmpleo, ParametrosPeticion.Titulo, ParametrosPeticion.Mensaje, ParametrosPeticion.Ubicacion,
                ParametrosPeticion.Remuneracion, ParametrosPeticion.ModalidadTrabajo, ParametrosPeticion.HorarioLaboral,
                ParametrosPeticion.TipoTrabajo, ParametrosPeticion.Etiquetas, ParametrosPeticion.Perfiles);

            ((IRepositorioEmpleos)Repositorio).Modificar(ParametrosPeticion.IdEmpleo, empleo, destacado);

            return new Resultado();
        }

        public class Resultado : IResultado
        {
        }

        public class Parametros
        {
            public int IdEmpleo { get; set; }

            public string RolUsuario { get; set; }

            public Models.Dtos.Empresa EmpresaUsuario { get; set; }

            public int IdUsuario { get; set; }

            public string Titulo { get; set; }

            public string Mensaje { get; set; }

            public string Ubicacion { get; set; }

            public string ModalidadTrabajo { get; set; }

            public string TipoTrabajo { get; set; }

            public string HorarioLaboral { get; set; }

            public decimal? Remuneracion { get; set; }

            public bool Destacado { get; set; }

            public int IdEmpresa { get; set; }

            public IEnumerable<int> Perfiles { get; set; }

            public IEnumerable<int> Etiquetas { get; set; }
        }
    }
}
