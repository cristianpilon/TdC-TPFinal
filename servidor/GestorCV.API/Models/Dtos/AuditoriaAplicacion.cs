using System;

namespace GestorCV.API.Models.Dtos
{
    public sealed class AuditoriaAplicacion
    {
        public AuditoriaAplicacion(string usuario, string ruta, string accion, DateTime fecha)
        {
            Usuario = usuario;
            Ruta = ruta;
            Accion = accion;
            Fecha = fecha;
        }

        public string Usuario { get; set; }

        public string Ruta { get; set; }

        public string Accion { get; set; }

        public DateTime Fecha { get; set; }
    }
}
