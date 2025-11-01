using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface IPresupuestoServicio
    {
        Task<Presupuesto> ObtenerPresupuestoActualAsync(int idUsuario);
        Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync(int idUsuario);
        Task<decimal> ObtenerPresupuestoConPorcentaje(Presupuesto presupuesto);
        Task<Presupuesto?> ObtenerPresupuestoPorIdAsync(int id);
        Task<decimal> CalcularMontoActualGastado(Presupuesto presupuesto);
        Task ActualizarPresupuestoAsync(Presupuesto presupuesto, decimal nuevoMonto);
        Task CrearPresupuestoInicial(int idUsuario);
    }
    public class PresupuestoServicio : IPresupuestoServicio
    {
        private readonly IPresupuestoRepositorio _repositorioPresupuesto;
        private readonly IGastoRepositorio _gastoRepositorio;

        public PresupuestoServicio(IPresupuestoRepositorio repositorioPresupuesto, IGastoRepositorio gastoRepositorio)
        {
            _repositorioPresupuesto = repositorioPresupuesto;
            _gastoRepositorio = gastoRepositorio;
        }

        public async Task<Presupuesto?> ObtenerPresupuestoPorIdAsync(int idPresupuesto)
        {
            return await _repositorioPresupuesto.ObtenerPresupuestoPorId(idPresupuesto);
        }

        public async Task<Presupuesto> ObtenerPresupuestoActualAsync(int idUsuario)
        {
           return await _repositorioPresupuesto.ObtenerUltimoPresupuestoAsync(idUsuario);
        }

        public async Task<IEnumerable<Presupuesto>> ObtenerTodosLosPresupuestosAsync(int idUsuario)
        {
           return await _repositorioPresupuesto.ObtenerTodosLosPresupuestosAsync(idUsuario);
        }

        public async Task<decimal> ObtenerPresupuestoConPorcentaje(Presupuesto presupuesto)
        {
            if (presupuesto == null || presupuesto.MontoLimite == 0)
            {
                return 0;
            }

            var porcentajeGastado = (presupuesto.MontoActualGastado / presupuesto.MontoLimite) * 100;
            return Math.Min((decimal)porcentajeGastado, 100);
        }

        public async Task<decimal> CalcularMontoActualGastado(Presupuesto presupuesto)
        {
            if (presupuesto == null || presupuesto.MontoLimite == 0)
            {
                return 0;
            }

            List<Gasto> listaDeMontos = await _gastoRepositorio.ObtenerGastosPorMesAsync(presupuesto.Mes, presupuesto.Anio);

            decimal montoActualGastado = 0;
            foreach (var gasto in listaDeMontos)
            {
                montoActualGastado += gasto.MontoTotal.Value;
            }

            await _repositorioPresupuesto.ActualizarMontonActualGastado(presupuesto,montoActualGastado);
            return montoActualGastado;
        }

        public async Task ActualizarPresupuestoAsync(Presupuesto presupuesto, decimal nuevoMonto)
        {
            await _repositorioPresupuesto.ActualizarPresupuesto(presupuesto, nuevoMonto);
        }

        public async Task CrearPresupuestoInicial(int idUsuario)
        {
            await _repositorioPresupuesto.CrearPresupuestoInicial(idUsuario);
        }
    }
}
