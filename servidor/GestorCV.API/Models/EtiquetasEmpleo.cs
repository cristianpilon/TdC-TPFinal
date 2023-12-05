using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class EtiquetasEmpleo
{
    public int Id { get; set; }

    public int IdEmpleo { get; set; }

    public int IdEtiqueta { get; set; }

    public virtual Empleo IdEmpleoNavigation { get; set; }

    public virtual Etiqueta IdEtiquetaNavigation { get; set; }
}
