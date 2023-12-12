using GestorCV.API.Infraestructura;
using Microsoft.EntityFrameworkCore;

namespace GestorCV.API.Models;

public partial class GestorCurriculumsContext : DbContext
{
    public GestorCurriculumsContext()
    {
    }

    public GestorCurriculumsContext(DbContextOptions<GestorCurriculumsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Curriculum> Curriculums { get; set; }

    public virtual DbSet<Empleo> Empleos { get; set; }

    public virtual DbSet<Etiqueta> Etiquetas { get; set; }

    public virtual DbSet<EtiquetasEmpleo> EtiquetasEmpleos { get; set; }

    public virtual DbSet<Formulario> Formularios { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<GruposPermiso> GruposPermisos { get; set; }

    public virtual DbSet<Notificacion> Notificaciones { get; set; }

    public virtual DbSet<Perfil> Perfiles { get; set; }

    public virtual DbSet<PerfilesEmpleo> PerfilesEmpleos { get; set; }

    public virtual DbSet<PerfilesUsuario> PerfilesUsuarios { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Postulacion> Postulaciones { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<RolesGrupo> RolesGrupos { get; set; }

    public virtual DbSet<RolesPermiso> RolesPermisos { get; set; }

    public virtual DbSet<RutaFormulario> RutasFormularios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosGrupo> UsuariosGrupos { get; set; }

    public virtual DbSet<UsuariosPermiso> UsuariosPermisos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(AppConfiguration.ConnString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleo>(entity =>
        {
            entity.Property(e => e.Descripcion).IsRequired();
            entity.Property(e => e.FechaPublicacion).HasColumnType("datetime");
            entity.Property(e => e.HorariosLaborales).IsRequired();
            entity.Property(e => e.ModalidadTrabajo).IsRequired();
            entity.Property(e => e.Remuneracion).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TipoTrabajo).IsRequired();
            entity.Property(e => e.Titulo).IsRequired();
            entity.Property(e => e.Ubicacion).IsRequired();
        });

        modelBuilder.Entity<Etiqueta>(entity =>
        {
            entity.Property(e => e.Nombre).IsRequired();
        });

        modelBuilder.Entity<EtiquetasEmpleo>(entity =>
        {
            entity.HasOne(d => d.IdEmpleoNavigation).WithMany(p => p.EtiquetasEmpleos)
                .HasForeignKey(d => d.IdEmpleo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EtiquetasEmpleos_Empleos");

            entity.HasOne(d => d.IdEtiquetaNavigation).WithMany(p => p.EtiquetasEmpleos)
                .HasForeignKey(d => d.IdEtiqueta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EtiquetasEmpleos_Etiquetas");
        });

        modelBuilder.Entity<Formulario>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nombre).IsRequired();
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.Property(e => e.Nombre).IsRequired();
        });

        modelBuilder.Entity<GruposPermiso>(entity =>
        {
            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.GruposPermisos)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GruposPermisos_Grupos");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.GruposPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GruposPermisos_Permisos");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaLectura).HasColumnType("datetime");
            entity.Property(e => e.Mensaje).IsRequired();

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Usuarios");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.Property(e => e.Nombre).IsRequired();
        });

        modelBuilder.Entity<PerfilesEmpleo>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdEmpleoNavigation).WithMany(p => p.PerfilesEmpleos)
                .HasForeignKey(d => d.IdEmpleo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PerfilesEmpleos_Empleos");

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.PerfilesEmpleos)
                .HasForeignKey(d => d.IdPerfil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PerfilesEmpleos_Perfiles");
        });

        modelBuilder.Entity<PerfilesUsuario>(entity =>
        {
            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.PerfilesUsuarios)
                .HasForeignKey(d => d.IdPerfil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PerfilesUsuarios_Perfiles");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.PerfilesUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PerfilesUsuarios_Usuarios");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.Property(e => e.Accion).IsRequired();
            entity.Property(e => e.Nombre).IsRequired();

            entity.HasOne(d => d.IdFormularioNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.IdFormulario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Permisos_Formularios");
        });

        modelBuilder.Entity<Postulacion>(entity =>
        {
            entity.Property(e => e.Estado).IsRequired();

            entity.HasOne(d => d.IdEmpleoNavigation).WithMany(p => p.Postulaciones)
                .HasForeignKey(d => d.IdEmpleo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postulaciones_Empleos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Postulaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postulaciones_Usuarios");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nombre).IsRequired();
        });

        modelBuilder.Entity<RolesGrupo>(entity =>
        {
            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.RolesGrupos)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolesGrupos_Grupos");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolesGrupos)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolesGrupos_Rol");
        });

        modelBuilder.Entity<RolesPermiso>(entity =>
        {
            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolesPermisos_Permisos");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolesPermisos_Rol");
        });

        modelBuilder.Entity<RutaFormulario>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Ruta).IsRequired();

            entity.HasOne(d => d.IdFormularioNavigation).WithMany()
                .HasForeignKey(d => d.IdFormulario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RutasFormularios_Formularios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Apellido).IsRequired();
            entity.Property(e => e.Correo).IsRequired();
            entity.Property(e => e.EnlaceActivacion).IsRequired();
            entity.Property(e => e.EnlaceFechaActivacion).HasColumnType("datetime");
            entity.Property(e => e.FechaActivacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre).IsRequired();
            entity.Property(e => e.Password).IsRequired();

            entity.HasOne(d => d.IdCurriculumNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdCurriculum)
                .HasConstraintName("FK_Usuarios_Curriculums");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Rol");
        });

        modelBuilder.Entity<UsuariosGrupo>(entity =>
        {
            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.UsuariosGrupos)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuariosGrupos_Grupos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuariosGrupos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuariosGrupos_Usuarios");
        });

        modelBuilder.Entity<UsuariosPermiso>(entity =>
        {
            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.UsuariosPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuariosPermisos_Permisos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuariosPermisos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuariosPermisos_Usuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
