using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GestionDeGastos.Repositorio
{
        public interface IGastoRepositorio
        {
            //Task<Gasto> ObtenerUltimoGastoAsync();
            Task<IEnumerable<Gasto>> ObtenerTodosLosGastosAsync();
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

            public async Task<IEnumerable<Gasto>> ObtenerTodosLosGastosAsync()
            {
                return await _context.Gastos.ToListAsync();
            }

            //public async Task<Gasto> ObtenerUltimoGastoAsync()
            //{
            //    return await _context.Gastos
            //         .OrderByDescending(p => p.Anio)
            //         .ThenByDescending(p => p.Mes)
            //         .FirstOrDefaultAsync();
            //}
        }
    }





