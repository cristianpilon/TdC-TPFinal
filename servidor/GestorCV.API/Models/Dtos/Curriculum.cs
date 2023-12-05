using System;

namespace GestorCV.API.Models.Dtos;

public sealed class Curriculum
{
    public int Id { get; private set; }

    public string Titulo { get; private set; }

    public string Ubicacion { get; private set; }

    public string ResumenProfesional { get; private set; }

    public string ExperienciaLaboral { get; private set; }

    public string Educacion { get; private set; }

    public string Idiomas { get; private set; }

    public string Certificados { get; private set; }

    public string Intereses { get; private set; }
}
