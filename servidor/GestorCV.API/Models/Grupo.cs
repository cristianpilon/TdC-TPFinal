using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Grupo
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public int? IdGrupoPadre { get; set; }

    public virtual ICollection<GruposPermiso> GruposPermisos { get; set; } = new List<GruposPermiso>();

    public virtual ICollection<RolesGrupo> RolesGrupos { get; set; } = new List<RolesGrupo>();

    public virtual ICollection<UsuariosGrupo> UsuariosGrupos { get; set; } = new List<UsuariosGrupo>();
}
