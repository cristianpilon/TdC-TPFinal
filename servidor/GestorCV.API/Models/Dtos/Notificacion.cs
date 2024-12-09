using System;

namespace GestorCV.API.Models.Dtos;

public sealed class Notificacion
{
    public Notificacion(int idUsuario, string mensaje)
    {
        IdUsuario = idUsuario;
        Mensaje = mensaje;
        FechaCreacion = DateTime.UtcNow;
    }

    public Notificacion(int id, int idUsuario, string mensaje, DateTime? fechaLectura, DateTime fechaCreacion)
    {
        Id = id;
        IdUsuario = idUsuario;
        Mensaje = mensaje;
        FechaCreacion = fechaCreacion;
        FechaLectura = fechaLectura;
    }

    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Mensaje { get; set; }

    public DateTime? FechaLectura { get; set; }

    public DateTime FechaCreacion { get; set; }
}
