using GestorCV.API.Models;
using GestorCV.API.Repositorios.Base;
using System.Linq;

namespace GestorCV.API.Repositorios
{
    /// <summary>
    /// Repositorio de permisos.
    /// </summary>
    public sealed class RepositorioPermisos : RepositorioBase
    {
        public Permiso Obtener(string formulario, string accion) 
        {
            return _contexto.Permisos
                .FirstOrDefault(p => p.IdFormularioNavigation.Nombre == formulario && p.Accion == accion);
        }
    }
}
