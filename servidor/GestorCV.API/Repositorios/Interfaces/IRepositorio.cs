namespace GestorCV.API.Repositorios.Interfaces
{
    public interface IRepositorio
    {
        public void IniciarTransaccion();

        public void ConfirmarTransaccion();

        public void AnularTransaccion();
    }
}
