namespace GestorCV.API.Models.Dtos;

public sealed class Perfil
{
    public Perfil(int id, string nombre) 
    { 
        Id = id;
        Nombre = nombre;
    }

    public Perfil(int id)
    {
        Id = id;
    }

    public int Id { get; private set; }

    public string Nombre { get; private set; }
}
