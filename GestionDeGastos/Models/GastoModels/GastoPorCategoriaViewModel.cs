using GestionDeGastos.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;

namespace GestionDeGastos.Models.GastoModels
{
    public class GastoPorCategoriaViewModel
    {
        public List<Gasto> UltimosGastos { get; set; }
        public List<Gasto> TotalesPorCategoria { get; set; }

        // Para filtros
        public int? MesSeleccionado { get; set; }
        public int? AñoSeleccionado { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}