using Microsoft.EntityFrameworkCore;

namespace GestionDeGastos.Repositorio
{
    public interface IPresupuestoRepositorio
    {
        Task<Presupuesto> ObtenerUltimoPresupuestoAsync();
        Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync();
        Task<Presupuesto> ObtenerPresupuestoPorId(int IdPresupuesto);
        Task CrearPresupuesto(Presupuesto presupuesto);
        Task ActualizarPresupuesto(Presupuesto presupuesto);

    }
    public class PresupuestoRepositorio : IPresupuestoRepositorio
    {
        private readonly GestionDeGastosBdContext _context;

        public PresupuestoRepositorio(GestionDeGastosBdContext context)
        {
            _context = context;
        }
        public async Task ActualizarPresupuesto(Presupuesto presupuesto)
        {
            Presupuesto presupuestoEncontrado = await ObtenerPresupuestoPorId(presupuesto.IdPresupuesto);

            if (presupuestoEncontrado == null)
            {
                throw new Exception("Presupuesto no encontrado");
            }

            _context.Presupuestos.Update(presupuesto);
            await _context.SaveChangesAsync();
        }

        public async Task CrearPresupuesto(Presupuesto presupuesto)
        {
            _context.Presupuestos.Add(presupuesto);
            await _context.SaveChangesAsync();
        }

        public async Task<Presupuesto> ObtenerPresupuestoPorId(int IdPresupuesto)
        {
           return await _context.Presupuestos.FirstOrDefaultAsync(p => p.IdPresupuesto == IdPresupuesto);
        }

        public async Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync()
        {
            return await _context.Presupuestos.ToListAsync();
        }

        public async Task<Presupuesto> ObtenerUltimoPresupuestoAsync()
        {
           return await _context.Presupuestos
                .OrderByDescending(p => p.Anio)
                .ThenByDescending(p => p.Mes)
                .FirstOrDefaultAsync();
        }
    }
}
