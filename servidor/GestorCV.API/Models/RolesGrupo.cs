using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class RolesGrupo
{
    public int Id { get; set; }

    public int IdRol { get; set; }

    public int IdGrupo { get; set; }

    public virtual Grupo IdGrupoNavigation { get; set; }

    public virtual Rol IdRolNavigation { get; set; }
}
