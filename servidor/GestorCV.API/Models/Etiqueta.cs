using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Etiqueta
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<EtiquetasEmpleo> EtiquetasEmpleos { get; set; } = new List<EtiquetasEmpleo>();

    public virtual ICollection<EtiquetasUsuario> EtiquetasUsuarios { get; set; } = new List<EtiquetasUsuario>();
}
