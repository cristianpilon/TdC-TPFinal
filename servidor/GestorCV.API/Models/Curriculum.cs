using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Curriculum
{
    public int Id { get; set; }

    public string Titulo { get; set; }

    public string Ubicacion { get; set; }

    public string ResumenProfesional { get; set; }

    public string ExperienciaLaboral { get; set; }

    public string Educacion { get; set; }

    public string Idiomas { get; set; }

    public string Certificados { get; set; }

    public string Intereses { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
