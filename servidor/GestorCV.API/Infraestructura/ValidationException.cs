using System;
using System.Collections.Generic;

namespace GestorCV.API.Infraestructura
{
    public class ValidacionException : Exception
    {
        public class Validacion
        {
            public string Campo { get; set; }
            
            public string Mensaje { get; set; }
        }

        public List<Validacion> Validaciones { get; set; }

        
        public ValidacionException(List<Validacion> validaciones)
        {
            Validaciones = validaciones;
        }
    }
}
