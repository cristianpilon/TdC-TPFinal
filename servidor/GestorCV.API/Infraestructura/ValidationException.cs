using System;
using System.Collections.Generic;
using static GestorCV.API.Infraestructura.ValidacionException;

namespace GestorCV.API.Infraestructura
{
    public class ValidacionException : Exception
    {
        public static void LanzarValidacionSimple(string mensaje, string campo = "")
        {
            var validaciones = new List<Validacion>();

            var validacion = new Validacion(mensaje, campo);
            validaciones.Add(validacion);

            throw new ValidacionException(validaciones);
        }


        public class Validacion
        {
            public Validacion(string mensaje, string campo = "")
            {
                Mensaje = mensaje;
                Campo = campo;
            }

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
