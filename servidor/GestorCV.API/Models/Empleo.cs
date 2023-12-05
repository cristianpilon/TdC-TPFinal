using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Empleo
{
    public int Id { get; set; }

    public string Titulo { get; set; }

    public string Descripcion { get; set; }

    public string Ubicacion { get; set; }

    public decimal? Remuneracion { get; set; }

    public string ModalidadTrabajo { get; set; }

    public DateTime FechaPublicacion { get; set; }

    public string HorariosLaborales { get; set; }

    public string TipoTrabajo { get; set; }

    public virtual ICollection<EtiquetasEmpleo> EtiquetasEmpleos { get; set; } = new List<EtiquetasEmpleo>();

    public virtual ICollection<PerfilesEmpleo> PerfilesEmpleos { get; set; } = new List<PerfilesEmpleo>();

    public virtual ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();
}
