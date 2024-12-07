namespace GestorCV.API.Models;

public partial class PerfilesCurso
{
    public int Id { get; set; }

    public int IdCurso { get; set; }

    public int IdPerfil { get; set; }

    public virtual Curso IdCursoNavigation { get; set; }

    public virtual Perfil IdPerfilNavigation { get; set; }
}
