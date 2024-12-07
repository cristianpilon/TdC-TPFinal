using System;

namespace GestorCV.API.Models
{
    public partial class AuditoriaAplicacion
    {
        public int Id { get; set; }

        public int? IdUsuario { get; set; }

        public string Ruta { get; set; }

        public string Accion { get; set; }

        public DateTime Fecha { get; set; }
    }
}
