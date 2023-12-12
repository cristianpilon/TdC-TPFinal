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

    public int Id { get; private set; }

    public int IdEmpleo { get; private set; }

    public int IdUsuario { get; private set; }

    public string Estado { get; private set; }

    public Empleo Empleo { get; private set; }

    public Usuario Usuario { get; private set; }
}
