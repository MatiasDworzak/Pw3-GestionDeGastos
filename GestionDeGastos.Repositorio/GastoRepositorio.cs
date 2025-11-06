using GestionDeGastos.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;




namespace GestionDeGastos.Repositorio
{
    public interface IGastoRepositorio
    {


        Task<List<Gasto>> ObtenerGastosPorRangoDeFechasAsync(DateOnly fechaInicio, DateOnly fechaFin);
        Task<List<Gasto>> ObtenerGastosPorMesAsync(int idUsuario, int mes, int anio);
        Task<List<Gasto>> ObtenerUltimosTresGastosPorUsuarioAsync(int idUsuario);
        Task<List<Gasto>> ObtenerGastosPorUsuarioAsync(int idUsuario);
        Task<List<Gasto>> ObtenerGastosTotalesPorCategoriaAsync(int idUsuario);
        Task<Gasto> ObtenerGastoPorId(int IdGasto);
        Task AgregarGastoAsync(Gasto gasto);
        Task ActualizarGastoAsync(Gasto Gasto);

    }
    public class GastoRepositorio : IGastoRepositorio
    {
        private readonly GestionDeGastosBdContext _context;

        public GastoRepositorio(GestionDeGastosBdContext context)
        {
            _context = context;
        }
        public async Task ActualizarGastoAsync(Gasto Gasto)
        {
            Gasto GastoEncontrado = await ObtenerGastoPorId(Gasto.IdGasto);

            if (GastoEncontrado == null)
            {
                throw new Exception("Gasto no encontrado");
            }

            _context.Gastos.Update(Gasto);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarGastoAsync(Gasto gasto)
        {
            if (gasto == null) throw new ArgumentNullException(nameof(gasto), "El gasto no puede ser null");

            await _context.Gastos.AddAsync(gasto);
            await _context.SaveChangesAsync();
        }

        public async Task<Gasto> ObtenerGastoPorId(int IdGasto)
        {
            return await _context.Gastos.FindAsync(IdGasto);
        }

        public async Task<List<Gasto>> ObtenerGastosPorUsuarioAsync(int idUsuario)
        {
            return await _context.Gastos
                .Where(g => g.IdUsuario == idUsuario)
                .OrderByDescending(g => g.Fecha)
                .ToListAsync();
        }

        public async Task<List<Gasto>> ObtenerUltimosTresGastosPorUsuarioAsync(int idUsuario)
        {
            return await _context.Gastos
                .Where(g => g.IdUsuario == idUsuario) // 🔹 filtra por el usuario
                .OrderByDescending(g => g.Fecha)      // ordena del más nuevo al más viejo
                .Take(3)                              // toma los últimos 3
                .ToListAsync();                       // devuelve la lista
        }

        public async Task<List<Gasto>> ObtenerGastosPorMesAsync(int idUsuario, int mes, int anio)
        {
            return await _context.Gastos
                .Where(g => g.IdUsuario == idUsuario && g.Fecha.Month == mes && g.Fecha.Year == anio)
                .OrderBy(g => g.Fecha)
                .ToListAsync();
        }


        public async Task<List<Gasto>> ObtenerGastosPorRangoDeFechasAsync(DateOnly fechaInicio, DateOnly fechaFin)
        {
            return await _context.Gastos
                .Where(g => g.Fecha >= fechaInicio && g.Fecha <= fechaFin)
                .OrderBy(g => g.Fecha)
                .ToListAsync();
        }



        public async Task<List<Gasto>> ObtenerGastosTotalesPorCategoriaAsync(int idUsuario)
        {
            return await _context.Gastos
                 .Where(g => g.IdUsuario == idUsuario)
                .Include(g => g.IdCategoriaNavigation)
                .GroupBy(g => new
                {
                    g.IdCategoria,
                    g.IdCategoriaNavigation.Descripcion
                })
                .Select(grupo => new Gasto
                {
                    IdCategoria = grupo.Key.IdCategoria,
                    IdCategoriaNavigation = new Categorium
                    {
                        IdCategoria = grupo.Key.IdCategoria,
                        Descripcion = grupo.Key.Descripcion
                    },
                    MontoTotal = grupo.Sum(g => g.MontoTotal)
                })
                .OrderBy(g => g.IdCategoriaNavigation.Descripcion)
                .ToListAsync();
        }


    }




}





