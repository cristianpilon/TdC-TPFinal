using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class GruposPermiso
{
    public int Id { get; set; }

    public int IdGrupo { get; set; }

    public int IdPermiso { get; set; }

    public virtual Grupo IdGrupoNavigation { get; set; }

    public virtual Permiso IdPermisoNavigation { get; set; }
}
