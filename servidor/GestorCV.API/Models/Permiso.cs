using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Permiso
{
    public int Id { get; set; }

    public string Accion { get; set; }

    public int IdFormulario { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<GruposPermiso> GruposPermisos { get; set; } = new List<GruposPermiso>();

    public virtual Formulario IdFormularioNavigation { get; set; }

    public virtual ICollection<RolesPermiso> RolesPermisos { get; set; } = new List<RolesPermiso>();

    public virtual ICollection<UsuariosPermiso> UsuariosPermisos { get; set; } = new List<UsuariosPermiso>();
}
