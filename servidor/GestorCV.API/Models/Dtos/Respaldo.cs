using System;

namespace GestorCV.API.Models.Dtos;

public sealed class Respaldo
{
    public Respaldo(DateTime fecha, string tipo)
    {
        Fecha = fecha;
        Tipo = tipo;
    }

    public DateTime Fecha { get; private set; }

    public string Tipo { get; private set; }
}
