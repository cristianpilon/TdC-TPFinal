using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Formulario
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
