using System;
using System.Linq;

namespace GestorCV.API.Models.Dtos;

public sealed class Postulacion
{
    private const string EstadoPendiente = "Pendiente";

    /// <summary>
    /// Contructor para agregar una postulación
    /// </summary>
    public Postulacion(int idEmpleo, int idUsuario, string estado = EstadoPendiente)
    {
        Estado = estado;
        IdEmpleo = idEmpleo;
        IdUsuario = idUsuario;
    }

    /// <summary>
    /// Constructor para obtener todos las postulaciones
    /// </summary>
    public Postulacion(int id, int idEmpleo, int idUsuario, string estado, DateTime fecha, Models.Empleo empleo, Models.Usuario usuario)
    {
        Id = id;
        Estado = estado;
        IdEmpleo = idEmpleo;
        IdUsuario = idUsuario;
        Fecha = fecha;
        Empleo = new Empleo(empleo.Id, empleo.IdEmpresaNavigation.Nombre, empleo.Titulo, empleo.Ubicacion, empleo.FechaPublicacion);
        Usuario = new Usuario(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Correo);
    }

    /// <summary>
    /// Constructor para obtener todos las postulaciones
    /// </summary>
    public Postulacion(int id, string estado, DateTime fecha, Models.Empleo empleo, Models.Usuario usuario, Models.Curriculum curriculum)
    {
        Id = id;
        Estado = estado;
        Fecha = fecha;
        Empleo = new Empleo(empleo.Id, empleo.Titulo, empleo.Descripcion, empleo.Ubicacion, empleo.Remuneracion, empleo.ModalidadTrabajo,
            empleo.FechaPublicacion, empleo.HorariosLaborales, empleo.TipoTrabajo, new Empresa(empleo.IdEmpresa), empleo.Destacado, 
            empleo.EtiquetasEmpleos.Select(x => new Etiqueta(x.IdEtiqueta, x.IdEtiquetaNavigation.Nombre)).ToList(),
            empleo.PerfilesEmpleos.Select(x => new Perfil(x.IdPerfil, x.IdPerfilNavigation.Nombre)).ToList());
        Usuario = new Usuario(usuario.Nombre, usuario.Apellido, usuario.Correo);
        Curriculum = new Curriculum(curriculum.Titulo, curriculum.Ubicacion, curriculum.ResumenProfesional,
            curriculum.ExperienciaLaboral, curriculum.Educacion, curriculum.Idiomas, curriculum.Certificados, curriculum.Intereses,
            usuario.EtiquetasUsuarios.Select(x => new Etiqueta(x.IdEtiqueta, x.IdEtiquetaNavigation.Nombre)),
            usuario.PerfilesUsuarios.Select(x => new Perfil(x.IdPerfil, x.IdPerfilNavigation.Nombre)));
    }

    public int Id { get; private set; }

    public int IdEmpleo { get; private set; }

    public int IdUsuario { get; private set; }

    public string Estado { get; private set; }

    public DateTime Fecha { get; private set; }

    public Curriculum Curriculum { get; private set; }

    public Empleo Empleo { get; private set; }

    public Usuario Usuario { get; private set; }
}
