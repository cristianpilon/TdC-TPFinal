namespace GestorCV.API.Controllers.Servicios.Interfaces
{
    public interface IPeticion
    {
        public IResultado Procesar();

        public void Validar();
    }
}
