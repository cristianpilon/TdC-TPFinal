using System;
using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos;

public sealed class Empleo
{
    public Empleo(int id, string titulo, string descripcion, string ubicacion, decimal? remuneracion, string modalidadTrabajo, DateTime fechaPublicacion,
        string horariosLaborales, string tipoTrabajo, string empresa, string empresaLogo, bool destacado, List<Etiqueta> etiquetas, List<Perfil> perfiles)
    {
        _fechaPublicacion = fechaPublicacion;

        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
        Ubicacion = ubicacion;
        Remuneracion = remuneracion;
        ModalidadTrabajo = modalidadTrabajo;
        FechaPublicacion = fechaPublicacion.ToString("dd/MM/yyyy");
        HorariosLaborales = horariosLaborales;
        TipoTrabajo = tipoTrabajo;
        Empresa = empresa;
        EmpresaLogo = empresaLogo;
        Destacado = destacado;
        Etiquetas = etiquetas;
        Perfiles = perfiles;
    }

    /// <summary>
    /// Constructor para Postulaciones.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="empresa"></param>
    /// <param name="titulo"></param>
    /// <param name="ubicacion"></param>
    /// <param name="fechaPublicacion"></param>
    public Empleo(int id, string empresa, string titulo, string ubicacion, DateTime fechaPublicacion)
    {
        _fechaPublicacion = fechaPublicacion;

        Id = id;
        Titulo = titulo;
        Ubicacion = ubicacion;
        FechaPublicacion = fechaPublicacion.ToString("dd/MM/yyyy");
        Empresa = empresa;
    }

    private readonly DateTime _fechaPublicacion = DateTime.MinValue;

    public int Id { get; set; }

    public string Titulo { get; private set; }

    public string Descripcion { get; private set; }

    public string Ubicacion { get; private set; }

    public decimal? Remuneracion { get; private set; }

    public string ModalidadTrabajo { get; private set; }

    public string FechaPublicacion { get; private set; }

    public string HorariosLaborales { get; private set; }

    public string TipoTrabajo { get; private set; }

    public string Empresa { get; private set; }

    public string EmpresaLogo { get; private set; }

    public bool Destacado { get; private set; }

    public bool Nuevo { get 
        {
            return (DateTime.Now - _fechaPublicacion).TotalDays < 30;
        } 
    }

    public List<Etiqueta> Etiquetas { get; private set; }

    public List<Perfil> Perfiles { get; private set; }
}
