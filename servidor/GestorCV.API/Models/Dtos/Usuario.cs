using System;
using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos;

public sealed class Usuario : EntidadConAcceso
{
    public Usuario(int id, string nombre, string apellido, string correo, Rol rol = null, Empresa empresa = null, List<Acceso> accesos = null)
        : base(accesos)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Rol = rol;
        Correo = correo;
        Empresa = empresa;
    }

    public Usuario(string nombre, string apellido, string correo, List<Acceso> accesos = null)
        : base(accesos)
    {
        Nombre = nombre;
        Apellido = apellido;
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

    public Empresa Empresa { get; private set; }

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
}
