using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class EtiquetasCurso
{
    public int Id { get; set; }

    public int IdCurso { get; set; }

    public int IdEtiqueta { get; set; }

    public virtual Curso IdCursoNavigation { get; set; }

    public virtual Etiqueta IdEtiquetaNavigation { get; set; }
}
