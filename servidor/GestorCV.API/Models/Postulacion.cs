using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Postulacion
{
    public int Id { get; set; }

    public int IdEmpleo { get; set; }

    public int IdUsuario { get; set; }

    public string Estado { get; set; }

    public virtual Empleo IdEmpleoNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }
}
