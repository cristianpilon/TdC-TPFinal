using GestorCV.API.Models;
using GestorCV.API.Repositorios.Base;
using GestorCV.API.Repositorios.Interfaces;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    public interface IRepositorioPermisos : IRepositorio
    {
        public Permiso Obtener(string formulario, string accion);
    }

    /// <summary>
    /// Repositorio de permisos.
    /// </summary>
    public sealed class RepositorioPermisos : RepositorioBase, IRepositorioPermisos
    {
        public Permiso Obtener(string formulario, string accion) 
        {
            return _contexto.Permisos
                .FirstOrDefault(p => p.IdFormularioNavigation.Nombre == formulario && p.Accion == accion);
        }
    }
}
