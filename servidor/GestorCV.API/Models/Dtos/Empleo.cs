using System;
using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos;

public sealed class Empleo
{
    public Empleo(int id, string titulo, string descripcion, string ubicacion, decimal? remuneracion, string modalidadTrabajo, DateTime fechaPublicacion,
        string horariosLaborales, string tipoTrabajo, List<Etiqueta> etiquetas, List<Perfil> perfiles)
    {
        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
        Ubicacion = ubicacion;
        Remuneracion = remuneracion;
        ModalidadTrabajo = modalidadTrabajo;
        FechaPublicacion = fechaPublicacion.ToString("dd/MM/yyyy");
        HorariosLaborales = horariosLaborales;
        TipoTrabajo = tipoTrabajo;
        Etiquetas = etiquetas;
        Perfiles = perfiles;
    }

    public int Id { get; set; }

    public string Titulo { get; private set; }

    public string Descripcion { get; private set; }

    public string Ubicacion { get; private set; }

    public decimal? Remuneracion { get; private set; }

    public string ModalidadTrabajo { get; private set; }

    public string FechaPublicacion { get; private set; }

    public string HorariosLaborales { get; private set; }

    public string TipoTrabajo { get; private set; }

    public List<Etiqueta> Etiquetas { get; private set; }

    public List<Perfil> Perfiles { get; private set; }
}
