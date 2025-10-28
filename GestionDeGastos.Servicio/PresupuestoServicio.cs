using GestionDeGastos.Repositorio;

namespace GestionDeGastos.Servicios
{
    public interface IPresupuestoServicio
    {
        Task<Presupuesto> ObtenerPresupuestoActualAsync();
        Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync();
        decimal ObtenerPresupuestoConPorcentaje();
        Task<Presupuesto?> ObtenerPresupuestoPorIdAsync(int id);
    }
    public class PresupuestoServicio : IPresupuestoServicio
    {
        private readonly IPresupuestoRepositorio _repositorio;

        public PresupuestoServicio(IPresupuestoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Presupuesto?> ObtenerPresupuestoPorIdAsync(int id)
        {
            return await _repositorio.ObtenerPresupuestoPorId(id);
        }

        public async Task<Presupuesto> ObtenerPresupuestoActualAsync()
        {
           return await _repositorio.ObtenerPresupuestoPorId(1);
        }

        public async Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync()
        {
           return await _repositorio.ObtenerTodosLosPresupuestosAsync();
        }

        public decimal ObtenerPresupuestoConPorcentaje()
        {
            var presupuesto = _repositorio.ObtenerUltimoPresupuestoAsync().Result;
            if (presupuesto == null || presupuesto.MontoLimite == 0)
            {
                return 0;
            }

            var porcentajeGastado = (presupuesto.MontoActualGastado / presupuesto.MontoLimite) * 100;
            return Math.Min((decimal)porcentajeGastado, 100);
        }
    }
}
