using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Models.Dtos;

public sealed class Empleo
{
    /// <summary>
    /// Constructor para obtener empleos
    /// </summary>
    public Empleo(int id, string titulo, string descripcion, string ubicacion, decimal? remuneracion, string modalidadTrabajo, DateTime fechaPublicacion,
        string horariosLaborales, string tipoTrabajo, Empresa empresa, bool destacado, List<Etiqueta> etiquetas, List<Perfil> perfiles)
    {
        _fechaPublicacion = fechaPublicacion;

        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
        Ubicacion = ubicacion;
        Remuneracion = remuneracion;
        ModalidadTrabajo = modalidadTrabajo;
        FechaPublicacion = fechaPublicacion;
        HorariosLaborales = horariosLaborales;
        TipoTrabajo = tipoTrabajo;
        Empresa = empresa;
        Destacado = destacado;
        Etiquetas = etiquetas;
        Perfiles = perfiles;
    }

    /// <summary>
    /// Constructor para modificar empleo
    /// </summary>
    public Empleo(int id, string titulo, string descripcion, string ubicacion, decimal? remuneracion, string modalidadTrabajo, 
        string horariosLaborales, string tipoTrabajo, IEnumerable<int> etiquetas, IEnumerable<int> perfiles)
    {
        _fechaPublicacion = DateTime.UtcNow;

        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
        Ubicacion = ubicacion;
        Remuneracion = remuneracion;
        ModalidadTrabajo = modalidadTrabajo;
        FechaPublicacion = _fechaPublicacion;
        HorariosLaborales = horariosLaborales;
        TipoTrabajo = tipoTrabajo;
        Etiquetas = etiquetas.Select(x => new Etiqueta(x, string.Empty)).ToList();
        Perfiles = perfiles.Select(x => new Perfil(x, string.Empty)).ToList();
    }

    /// <summary>
    /// Constructor para crear empleo
    /// </summary>
    public Empleo(string titulo, string descripcion, string ubicacion, decimal? remuneracion, string modalidadTrabajo,
        string horariosLaborales, string tipoTrabajo, int idEmpresa, IEnumerable<int> etiquetas, IEnumerable<int> perfiles,
        bool destacado)
    {
        _fechaPublicacion = DateTime.UtcNow;

        Titulo = titulo;
        Descripcion = descripcion;
        Ubicacion = ubicacion;
        Remuneracion = remuneracion;
        ModalidadTrabajo = modalidadTrabajo;
        FechaPublicacion = _fechaPublicacion;
        HorariosLaborales = horariosLaborales;
        TipoTrabajo = tipoTrabajo;
        Empresa = new Empresa(idEmpresa);
        Destacado = destacado;
        Etiquetas = etiquetas.Select(x => new Etiqueta(x, string.Empty)).ToList();
        Perfiles = perfiles.Select(x => new Perfil(x, string.Empty)).ToList();
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
        FechaPublicacion = fechaPublicacion;
        //Empresa = empresa;
    }

    private readonly DateTime _fechaPublicacion = DateTime.MinValue;

    public int Id { get; set; }

    public string Titulo { get; private set; }

    public string Descripcion { get; private set; }

    public string Ubicacion { get; private set; }

    public decimal? Remuneracion { get; private set; }

    public string ModalidadTrabajo { get; private set; }

    public DateTime FechaPublicacion { get; private set; }

    public string HorariosLaborales { get; private set; }

    public string TipoTrabajo { get; private set; }

    public Empresa Empresa { get; private set; }

    public bool Destacado { get; private set; }

    public bool Nuevo { get 
        {
            return (DateTime.Now - _fechaPublicacion).TotalDays < 30;
        } 
    }

    public List<Etiqueta> Etiquetas { get; private set; }

    public List<Perfil> Perfiles { get; private set; }
}
