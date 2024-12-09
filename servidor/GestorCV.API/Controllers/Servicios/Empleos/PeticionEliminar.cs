using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios.Interfaces;
using GestorCV.API.Repositorios;
using System;

namespace GestorCV.API.Controllers.Servicios.Empleos
{
    public class PeticionEliminar : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionEliminar(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            ((IRepositorioEmpleos)Repositorio).Eliminar(ParametrosPeticion.Id);
            return new Resultado();
        }

        public class Resultado : IResultado
        {
        }

        public class Parametros
        {
            public int Id { get; set; }
        }
    }
}
