using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<RolesGrupo> RolesGrupos { get; set; } = new List<RolesGrupo>();

    public virtual ICollection<RolesPermiso> RolesPermisos { get; set; } = new List<RolesPermiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
