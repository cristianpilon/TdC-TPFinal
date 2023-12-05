namespace GestorCV.API.Models.Dtos
{
    public sealed class RutaFormulario
    {
        public RutaFormulario(int id, string ruta, bool backend)
        {
            Id = id;
            Ruta = ruta;
            Backend = backend;
        }

        public int Id { get; private set; }

        public string Ruta { get; private set; }

        public bool Backend { get; private set; }
    }
}
