using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorCV.API.Models.Dtos;

public sealed class Curso
{
    public Curso(int id, string titulo, string mensaje, DateTime fecha, Empresa empresa, List<Etiqueta> etiquetas, List<Perfil> perfiles)
    {
        Id = id;
        Titulo = titulo;
        Mensaje = mensaje;
        Fecha = fecha;
        Empresa = empresa;
        Etiquetas = etiquetas;
        Perfiles = perfiles;
    }

    /// <summary>
    /// Constructor para agregar/modificacion
    /// </summary>
    public Curso(string titulo, string mensaje, Empresa empresa, IEnumerable<int> etiquetas, IEnumerable<int> perfiles)
    {
        Titulo = titulo;
        Mensaje = mensaje;
        Fecha = DateTime.UtcNow;
        Empresa = empresa;
        Etiquetas = etiquetas.Select(x => new Etiqueta(x, string.Empty)).ToList();
        Perfiles = perfiles.Select(x => new Perfil(x, string.Empty)).ToList();
    }

    public int Id { get; set; }

    public string Titulo { get; private set; }

    public string Mensaje { get; private set; }

    public DateTime Fecha { get; set; }

    public Empresa Empresa { get; set; }

    public List<Etiqueta> Etiquetas { get; private set; }

    public List<Perfil> Perfiles { get; private set; }
}