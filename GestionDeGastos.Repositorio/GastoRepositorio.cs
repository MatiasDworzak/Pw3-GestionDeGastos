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
        //Task<Gasto> ObtenerUltimoGastoAsync();

        Task<List<Gasto>> ObtenerGastosPorRangoDeFechasAsync(DateOnly fechaInicio, DateOnly fechaFin);
        Task<List<Gasto>> ObtenerGastosPorMesAsync(int mes, int año);
        Task<List<Gasto>> ObtenerUltimosTresGastosPorUsuarioAsync(int idUsuario);
        Task<List<Gasto>> ObtenerGastosPorUsuarioAsync(int idUsuario);

        Task<Gasto> ObtenerGastoPorId(int IdGasto);
        Task CrearGasto(Gasto Gasto);
        Task ActualizarGasto(Gasto Gasto);

    }
    public class GastoRepositorio : IGastoRepositorio
    {
        private readonly GestionDeGastosBdContext _context;

        public GastoRepositorio(GestionDeGastosBdContext context)
        {
            _context = context;
        }
        public async Task ActualizarGasto(Gasto Gasto)
        {
            Gasto GastoEncontrado = await ObtenerGastoPorId(Gasto.IdGasto);

            if (GastoEncontrado == null)
            {
                throw new Exception("Gasto no encontrado");
            }

            _context.Gastos.Update(Gasto);
            await _context.SaveChangesAsync();
        }

        public async Task CrearGasto(Gasto Gasto)
        {
            _context.Gastos.Add(Gasto);
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

        public async Task<List<Gasto>> ObtenerGastosPorMesAsync(int mes, int año)
        {
            return await _context.Gastos
                .Where(g => g.Fecha.Month == mes && g.Fecha.Year == año)
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
    }




}