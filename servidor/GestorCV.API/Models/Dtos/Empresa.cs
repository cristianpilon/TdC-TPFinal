namespace GestorCV.API.Models.Dtos;

public sealed class Empresa
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Logo { get; set; }

    public Empresa(int id, string nombre)
    {
        this.Id = id;
        this.Nombre = nombre;
    }

    public Empresa(int id, string nombre, string logo)
    {
        this.Id = id;
        this.Nombre = nombre;
        this.Logo = logo;
    }

    public Empresa(int id)
    {
        this.Id = id;
        this.Nombre = string.Empty;
    }
}
