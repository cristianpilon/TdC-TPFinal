using System;
using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos;

public sealed class Curso
{
    public Curso(int id, string titulo, string mensaje, DateTime fecha, List<Etiqueta> etiquetas, List<Perfil> perfiles)
    {
        Id = id;
        Titulo = titulo;
        Mensaje = mensaje;
        Fecha = fecha;
        Etiquetas = etiquetas;
        Perfiles = perfiles;
    }

    public int Id { get; set; }

    public string Titulo { get; private set; }

    public string Mensaje { get; private set; }

    public DateTime Fecha { get; set; }

    public List<Etiqueta> Etiquetas { get; private set; }

    public List<Perfil> Perfiles { get; private set; }
}
