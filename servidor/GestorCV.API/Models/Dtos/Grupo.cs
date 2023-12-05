using System;
using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos
{
    public sealed class Grupo : Acceso
    {
        public Grupo(int id, string nombre, List<Acceso> accesos)
            : base(id, nombre)
        {
            Accesos = accesos;
        }

        public List<Acceso> Accesos { get; private set; }

        public override bool ValidarRuta(string ruta, string accion)
        {
            bool rutaValida = false;

            foreach (var acceso in Accesos)
            {
                // Si alguno de los accesos del grupo autoriza, el usuario puede continuar
                rutaValida = rutaValida || acceso.ValidarRuta(ruta, accion);
            }

            return rutaValida;
        }

        /// <summary>
        /// Obtiene todos los accesos exclusivos para el frontend de los grupos y permisos hijos.
        /// </summary>
        /// <returns>Lista de rutas de frontend con el tipo de permiso admitido.</returns>
        public override List<Acceso.PermisoFrontend> ObtenerPermisosFrontend()
        {
            List<Acceso.PermisoFrontend> permisosFrontends = new();

            foreach (var acceso in Accesos)
            {
                // Busco en todos los grupos y permisos hijos los permisos para el frontend
                permisosFrontends.AddRange(acceso.ObtenerPermisosFrontend());
            }

            return permisosFrontends;
        }
    }
}
