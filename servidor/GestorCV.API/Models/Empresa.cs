using System.Collections.Generic;

namespace GestorCV.API.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Logo { get; set; }

        public virtual ICollection<Empleo> Empleos { get; set; } = new List<Empleo>();

        public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();
    }
}
