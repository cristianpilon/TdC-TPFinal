using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Perfil
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<PerfilesCurso> PerfilesCursos { get; set; } = new List<PerfilesCurso>();

    public virtual ICollection<PerfilesEmpleo> PerfilesEmpleos { get; set; } = new List<PerfilesEmpleo>();

    public virtual ICollection<PerfilesUsuario> PerfilesUsuarios { get; set; } = new List<PerfilesUsuario>();
}
