using System;

namespace GestorCV.API.Models.Dtos;

public sealed class Postulacion
{
    private const string EstadoPendiente = "Pendiente";

    public Postulacion(int idEmpleo, int idUsuario, string estado = EstadoPendiente)
    {
        Estado = estado;
        IdEmpleo = idEmpleo;
        IdUsuario = idUsuario;
    }

    public Postulacion(int idEmpleo, int idUsuario, string estado, DateTime fecha, Models.Empleo empleo, Models.Usuario usuario)
    {
        Estado = estado;
        IdEmpleo = idEmpleo;
        IdUsuario = idUsuario;
        Fecha = fecha;
        Empleo = new Empleo(empleo.Id, empleo.Empresa, empleo.Titulo, empleo.Ubicacion, empleo.FechaPublicacion);
        Usuario = new Usuario(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Correo);
    }

    public int Id { get; private set; }

    public int IdEmpleo { get; private set; }

    public int IdUsuario { get; private set; }

    public string Estado { get; private set; }

    public DateTime Fecha { get; private set; }

    public Empleo Empleo { get; private set; }

    public Usuario Usuario { get; private set; }
}
