namespace GestorCV.API.Models.Dtos;

public sealed class Etiqueta
{
    public Etiqueta(int id, string nombre) 
    { 
        Id = id;
        Nombre = nombre;
    }

    public Etiqueta(int id)
    {
        Id = id;
    }

    public int Id { get; private set; }

    public string Nombre { get; private set; }
}
