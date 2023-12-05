using System.Collections.Generic;

namespace GestorCV.API.Models.Dtos
{
    public sealed class Formulario
    {
        public Formulario(int id, string nombre, List<RutaFormulario> rutasFormulario) 
        {
            Id = id;
            Nombre = nombre;
            RutasFormulario = rutasFormulario;
        }

        public int Id { get; private set; }

        public string Nombre { get; private set; }

        public List<RutaFormulario> RutasFormulario { get; private set; }
    }
}
