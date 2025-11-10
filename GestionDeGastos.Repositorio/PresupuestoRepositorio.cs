using GestionDeGastos.AccesoADatos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestionDeGastos.Repositorio
{
    public interface IPresupuestoRepositorio
    {
        Task<Presupuesto> ObtenerUltimoPresupuestoAsync(int idUsuario);
        Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync(int idUsuario);
        Task<Presupuesto> ObtenerPresupuestoPorId(int IdPresupuesto);
        Task CrearPresupuesto(int idUsuario, Presupuesto presupuesto);
        Task ActualizarPresupuesto(Presupuesto presupuesto, decimal nuevoMonto);
        Task<Presupuesto?> GetByIdAsync(int id);
        Task CrearPresupuestoInicial(int idUsuario);
        Task ActualizarMontonActualGastado(Presupuesto presupuesto, decimal montoActualGastado);
        Task<Presupuesto> ObtenerPresupuestoPorIdMesYAnio(int idUsuario, int month, int year);
    }
    public class PresupuestoRepositorio : IPresupuestoRepositorio
    {
        private readonly GestionDeGastosBdContext _context;

        public PresupuestoRepositorio(GestionDeGastosBdContext context)
        {
            _context = context;
        }
        public async Task ActualizarPresupuesto(Presupuesto presupuesto, decimal nuevoMonto)
        {
            Presupuesto presupuestoEncontrado = await ObtenerPresupuestoPorId(presupuesto.IdPresupuesto);

            if (presupuestoEncontrado == null)
            {
                throw new Exception("Presupuesto no encontrado");
            }

            presupuesto.MontoLimite = nuevoMonto;

            //_context.Presupuestos.Update(presupuesto);
            await _context.SaveChangesAsync();
        }

        public async Task CrearPresupuesto(int idUsuario, Presupuesto presupuesto)
        {
            await _context.Presupuestos.AddAsync(presupuesto);
            await _context.SaveChangesAsync();
        }

        public async Task<Presupuesto> ObtenerPresupuestoPorId(int IdPresupuesto)
        {
            return await _context.Presupuestos.FindAsync(IdPresupuesto);
        }

        public async Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync(int idUsuario)
        {
            //return await _context.Presupuestos.ToListAsync();
            return await _context.Presupuestos
                .Where(p => p.IdUsuario == idUsuario)
                .OrderByDescending(p => p.Anio)
                .ThenByDescending(p => p.Mes)
                .ToListAsync();
        }

        public async Task<Presupuesto?> GetByIdAsync(int id)
        => await _context.Presupuestos.FindAsync(id);

        public async Task<Presupuesto> ObtenerUltimoPresupuestoAsync(int idUsuario)
        {
            return await _context.Presupuestos
                 .Where(p => p.IdUsuario == idUsuario)
                 .OrderByDescending(p => p.Anio)
                 .ThenByDescending(p => p.Mes)
                 .FirstOrDefaultAsync();
        }

        public async Task CrearPresupuestoInicial(int idUsuario)
        {
            await _context.Presupuestos.AddAsync(new Presupuesto
            {
                IdUsuario = idUsuario,
                MontoLimite = 0,
                MontoActualGastado = 0,
                Mes = DateTime.Now.Month,
                Anio = DateTime.Now.Year
            });

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarMontonActualGastado(Presupuesto presupuesto, decimal montoActualGastado)
        {
            Presupuesto presupuestoEncontrado = await ObtenerPresupuestoPorId(presupuesto.IdPresupuesto);

            if (presupuestoEncontrado == null)
            {
                throw new Exception("Presupuesto no encontrado");
            }

            presupuesto.MontoActualGastado = montoActualGastado;

            _context.Presupuestos.Update(presupuesto);
            await _context.SaveChangesAsync();
        }

        public async Task<Presupuesto> ObtenerPresupuestoPorIdMesYAnio(int idUsuario, int month, int year)
        {
            return await _context.Presupuestos.Where(p => p.IdUsuario == idUsuario 
                                                        && p.Mes == month
                                                        && p.Anio == year)
                .FirstOrDefaultAsync();
        }
    }
}