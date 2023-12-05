using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos;

public sealed class Rol : EntidadConAcceso
{
    public Rol(int id, string nombre, List<Acceso> accesos) 
        : base(accesos)
    { 
        Id = id;
        Nombre = nombre;
    }

    public int Id { get; private set; }

    public string Nombre { get; private set; }
}
