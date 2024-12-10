using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Empleos
{
    public class PeticionObtenerTodosAvanzado : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        public PeticionObtenerTodosAvanzado(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
        }

        public override IResultado Procesar()
        {
            var empleos = ((IRepositorioEmpleos)Repositorio).ObtenerTodosAvanzado(
                ParametrosPeticion.Empresa, 
                ParametrosPeticion.Titulo,
                ParametrosPeticion.Ubicacion,
                ParametrosPeticion.TipoUbicacion,
                ParametrosPeticion.FechaDesde,
                ParametrosPeticion.Etiquetas,
                ParametrosPeticion.Perfiles);

            return new Resultado { Empleos = empleos };
        }

        public class Resultado : IResultado
        {
            public List<Models.Dtos.Empleo> Empleos { get; set; }
        }

        public class Parametros
        {
            public string Titulo { get; set; }

            public string Empresa { get; set; }

            public string Ubicacion { get; set; }

            public string TipoUbicacion { get; set; }

            public DateTime? FechaDesde { get; set; }

            public IEnumerable<int> Etiquetas { get; set; }

            public IEnumerable<int> Perfiles { get; set; }
        }
    }
}
