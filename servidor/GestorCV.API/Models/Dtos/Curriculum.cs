using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos;

public sealed class Curriculum
{
    public Curriculum(string titulo, string ubicacion, string resumenProfesional, string experienciaLaboral, string educacion, 
        string idiomas, string certificados, string intereses, List<Etiqueta> etiquetas, List<Perfil> perfiles)
    {
        Titulo = titulo;
        Ubicacion = ubicacion;
        ResumenProfesional = resumenProfesional;
        ExperienciaLaboral = experienciaLaboral;
        Educacion = educacion;
        Idiomas = idiomas;
        Certificados = certificados;
        Intereses = intereses;
        Etiquetas = etiquetas;
        Perfiles = perfiles;
    }

    public Curriculum(ModificarCurriculum curriculum)
    {
        Titulo = curriculum.Titulo;
        Ubicacion = curriculum.Ubicacion;
        ResumenProfesional = curriculum.ResumenProfesional;
        ExperienciaLaboral = curriculum.ExperienciaLaboral;
        Educacion = curriculum.Educacion;
        Idiomas = curriculum.Idiomas;
        Certificados = curriculum.Certificados;
        Intereses = curriculum.Intereses;
        Etiquetas = curriculum.Etiquetas;
        Perfiles = curriculum.Perfiles;
    }

    public int Id { get; private set; }

    public string Titulo { get; private set; }

    public string Ubicacion { get; private set; }

    public string ResumenProfesional { get; private set; }

    public string ExperienciaLaboral { get; private set; }

    public string Educacion { get; private set; }

    public string Idiomas { get; private set; }

    public string Certificados { get; private set; }

    public string Intereses { get; private set; }

    public List<Etiqueta> Etiquetas { get; private set; }

    public List<Perfil> Perfiles { get; private set; }
}

public sealed class ModificarCurriculum
{
    public string Titulo { get; set; }

    public string Ubicacion { get; set; }

    public string ResumenProfesional { get; set; }

    public string ExperienciaLaboral { get; set; }

    public string Educacion { get; set; }

    public string Idiomas { get; set; }

    public string Certificados { get; set; }

    public string Intereses { get; set; }

    public List<Etiqueta> Etiquetas { get; set; }

    public List<Perfil> Perfiles { get; set; }
}
