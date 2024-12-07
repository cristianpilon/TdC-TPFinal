﻿using GestorCV.API.Controllers.Base;
using GestorCV.API.Controllers.Servicios.Interfaces;
using GestorCV.API.Repositorios;
using GestorCV.API.Repositorios.Interfaces;
using System.Collections.Generic;

namespace GestorCV.API.Controllers.Servicios.Empleos
{
    public class PeticionObtener : PeticionBase
    {
        public Parametros ParametrosPeticion { get; private set; }

        private readonly IRepositorioPerfiles repositorioPerfiles;
        private readonly IRepositorioEtiquetas repositorioEtiquetas;

        public PeticionObtener(Parametros parametros, IRepositorio repositorio)
                : base(repositorio)
        {
            ParametrosPeticion = parametros;
            repositorioPerfiles = new RepositorioPerfiles();
            repositorioEtiquetas = new RepositorioEtiquetas();
        }

        public override IResultado Procesar()
        {
            var empleo = ((IRepositorioEmpleos)Repositorio).Obtener(ParametrosPeticion.Id);

            var perfiles = repositorioPerfiles.ObtenerTodos();
            var etiquetas = repositorioEtiquetas.ObtenerTodos();

            return new Resultado { Empleo = empleo, Perfiles = perfiles, Etiquetas = etiquetas };
        }

        public class Resultado : IResultado
        {
            public Models.Dtos.Empleo Empleo { get; set; }

            public List<Models.Dtos.Etiqueta> Etiquetas { get; set; }

            public List<Models.Dtos.Perfil> Perfiles { get; set; }
        }

        public class Parametros
        {
            public Parametros(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
        }
    }
}
