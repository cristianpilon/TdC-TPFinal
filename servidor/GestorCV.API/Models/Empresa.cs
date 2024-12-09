using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Empresa
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Logo { get; set; }

    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    public virtual ICollection<Empleo> Empleos { get; set; } = new List<Empleo>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
