using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Curso
{
    public int Id { get; set; }

    public string Titulo { get; set; }

    public string Mensaje { get; set; }

    public DateTime Fecha { get; set; }

    public int IdEmpresa { get; set; }

    public int IdUsuarioCreador { get; set; }

    public virtual ICollection<EtiquetasCurso> EtiquetasCursos { get; set; } = new List<EtiquetasCurso>();

    public virtual Empresa IdEmpresaNavigation { get; set; }

    public virtual Usuario IdUsuarioCreadorNavigation { get; set; }

    public virtual ICollection<PerfilesCurso> PerfilesCursos { get; set; } = new List<PerfilesCurso>();
}
