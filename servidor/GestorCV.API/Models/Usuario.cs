using System;
using System.Collections.Generic;

namespace GestorCV.API.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string Correo { get; set; }

    public string Password { get; set; }

    public bool Activo { get; set; }

    public string EnlaceActivacion { get; set; }

    public DateTime EnlaceFechaActivacion { get; set; }

    public DateTime? FechaActivacion { get; set; }

    public int? IdCurriculum { get; set; }

    public int IdRol { get; set; }

    public int? IdEmpresa { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; }

    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    public virtual ICollection<Empleo> Empleos { get; set; } = new List<Empleo>();

    public virtual ICollection<EtiquetasUsuario> EtiquetasUsuarios { get; set; } = new List<EtiquetasUsuario>();

    public virtual Curriculum IdCurriculumNavigation { get; set; }

    public virtual Rol IdRolNavigation { get; set; }

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    public virtual ICollection<PerfilesUsuario> PerfilesUsuarios { get; set; } = new List<PerfilesUsuario>();

    public virtual ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();

    public virtual ICollection<UsuariosGrupo> UsuariosGrupos { get; set; } = new List<UsuariosGrupo>();

    public virtual ICollection<UsuariosPermiso> UsuariosPermisos { get; set; } = new List<UsuariosPermiso>();
}
