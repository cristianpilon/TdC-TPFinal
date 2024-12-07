using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.AuditoriasAplicacion
{
    public class PeticionObtenerTodos : PeticionBase
    {

        public PeticionObtenerTodos(IRepositorio repositorio)
                : base(repositorio)
        {
        }

        public override IResultado Procesar()
        {
            var auditoriasAplicacion = ((IRepositorioAuditoriasAplicacion)Repositorio).ObtenerTodos();

            return new Resultado { AuditoriasAplicacion = auditoriasAplicacion };
        }

        public class Resultado : IResultado
        {
            public List<Models.Dtos.AuditoriaAplicacion> AuditoriasAplicacion { get; set; }
        }
    }
}
