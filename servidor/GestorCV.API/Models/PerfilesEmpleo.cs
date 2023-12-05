using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class PerfilesEmpleo
{
    public int Id { get; set; }

    public int IdEmpleo { get; set; }

    public int IdPerfil { get; set; }

    public virtual Empleo IdEmpleoNavigation { get; set; }

    public virtual Perfil IdPerfilNavigation { get; set; }
}
