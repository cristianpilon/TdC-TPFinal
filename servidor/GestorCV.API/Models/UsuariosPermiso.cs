using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class UsuariosPermiso
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdPermiso { get; set; }

    public virtual Permiso IdPermisoNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }
}
