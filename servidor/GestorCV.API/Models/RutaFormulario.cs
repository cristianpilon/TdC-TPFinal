using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class RutaFormulario
{
    public int Id { get; set; }

    public int IdFormulario { get; set; }

    public string Ruta { get; set; }

    public bool Backend { get; set; }

    public virtual Formulario IdFormularioNavigation { get; set; }
}
