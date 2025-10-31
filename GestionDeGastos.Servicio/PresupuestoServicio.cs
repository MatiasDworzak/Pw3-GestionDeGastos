using GestionDeGastos.Repositorio;

namespace GestionDeGastos.Servicio
{
    public interface IPresupuestoServicio
    {
        Task<Presupuesto> ObtenerPresupuestoActualAsync(int idUsuario);
        Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync(int idUsuario);
        decimal ObtenerPresupuestoConPorcentaje(int idUsuario);
        Task<Presupuesto?> ObtenerPresupuestoPorIdAsync(int id);
        Task ActualizarPresupuestoAsync(Presupuesto presupuesto, decimal nuevoMonto);
    }
    public class PresupuestoServicio : IPresupuestoServicio
    {
        private readonly IPresupuestoRepositorio _repositorio;

        public PresupuestoServicio(IPresupuestoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Presupuesto?> ObtenerPresupuestoPorIdAsync(int idPresupuesto)
        {
            return await _repositorio.ObtenerPresupuestoPorId(idPresupuesto);
        }

        public async Task<Presupuesto> ObtenerPresupuestoActualAsync(int idUsuario)
        {
           return await _repositorio.ObtenerUltimoPresupuestoAsync(idUsuario);
        }

        public async Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync(int idUsuario)
        {
           return await _repositorio.ObtenerTodosLosPresupuestosAsync(idUsuario);
        }

        public decimal ObtenerPresupuestoConPorcentaje(int idUsuario)
        {
            var presupuesto = _repositorio.ObtenerUltimoPresupuestoAsync(idUsuario).Result;
            if (presupuesto == null || presupuesto.MontoLimite == 0)
            {
                return 0;
            }

            var porcentajeGastado = (presupuesto.MontoActualGastado / presupuesto.MontoLimite) * 100;
            return Math.Min((decimal)porcentajeGastado, 100);
        }

        public async Task ActualizarPresupuestoAsync(Presupuesto presupuesto, decimal nuevoMonto)
        {
            await _repositorio.ActualizarPresupuesto(presupuesto, nuevoMonto);
        }
    }
}
