using System;
using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos;

public sealed class Usuario : EntidadConAcceso
{
    public Usuario(int id, string nombre, string apellido, string correo, Rol rol, List<Acceso> accesos)
        : base(accesos)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Rol = rol;
        Correo = correo;
    }

    public int Id { get; private set; }

    public string Nombre { get; private set; }

    public string Apellido { get; private set; }

    public string Correo { get; private set; }

    public string Password { get; private set; }

    public bool Activo { get; private set; }

    public string EnlaceActivacion { get; private set; }

    public DateTime EnlaceFechaActivacion { get; private set; }

    public DateTime? FechaActivacion { get; private set; }


    // public Curriculum Curriculum { get; private set; }

    public Rol Rol { get; private set; }

    /// <summary>
    /// Obtiene todos los accesos (tanto de usuario como del rol) exclusivos para el frontend para poder enviarlos en el token de usuario.
    /// </summary>
    /// <returns>Lista de rutas de frontend con el tipo de permiso admitido.</returns>
    public List<Acceso.PermisoFrontend> ObtenerPermisosFrontend()
    {
        List<Acceso.PermisoFrontend> permisosFrontends = new();

        foreach (var acceso in Accesos)
        {
            // Busco en todos los grupos y permisos hijos los permisos para el frontend
            permisosFrontends.AddRange(acceso.ObtenerPermisosFrontend());
        }

        foreach (var acceso in Rol.Accesos)
        {
            // Busco en todos los grupos y permisos hijos los permisos para el frontend
            permisosFrontends.AddRange(acceso.ObtenerPermisosFrontend());
        }

        return permisosFrontends;
    }

    // public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    // public virtual ICollection<PerfilesUsuario> PerfilesUsuarios { get; set; } = new List<PerfilesUsuario>();

    // public virtual ICollection<Postulacione> Postulaciones { get; set; } = new List<Postulacione>();

    // public virtual ICollection<UsuariosGrupo> UsuariosGrupos { get; set; } = new List<UsuariosGrupo>();

    // public virtual ICollection<UsuariosPermiso> UsuariosPermisos { get; set; } = new List<UsuariosPermiso>();
}
