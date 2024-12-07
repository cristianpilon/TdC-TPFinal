using GestorCV.API.Models;

public partial class EtiquetasUsuario
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdEtiqueta { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }

    public virtual Etiqueta IdEtiquetaNavigation { get; set; }
}