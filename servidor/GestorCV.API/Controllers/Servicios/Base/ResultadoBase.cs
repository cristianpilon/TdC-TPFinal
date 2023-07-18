using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorCV.API.Controllers.Servicios
{
    public class ResultadoBase
    {
        public List<ResultadoValidacion> Validaciones { get; set; } = new List<ResultadoValidacion>();

        public class ResultadoValidacion
        {
            public string Campo { get; set; }

            public string Mensaje { get; set; }
        }
    }
}
