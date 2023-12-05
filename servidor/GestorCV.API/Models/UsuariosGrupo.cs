using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class UsuariosGrupo
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdGrupo { get; set; }

    public virtual Grupo IdGrupoNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }
}
