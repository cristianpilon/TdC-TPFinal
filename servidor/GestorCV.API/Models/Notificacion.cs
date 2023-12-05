using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Notificacion
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Mensaje { get; set; }

    public DateTime? FechaLectura { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }
}
