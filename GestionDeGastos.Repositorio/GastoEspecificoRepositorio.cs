using GestionDeGastos;
using Microsoft.EntityFrameworkCore;

namespace GestionDeGastos.Repositorio
{
    public interface IGastoEspecificoRepositorio
    {
        Task<Gasto> ObtenerGastoPorID(int id);
        Task<Categorium> ObtenerCategoriaDeUnGasto(Gasto gasto);
        Task<MetodoDePago> ObtenerMetodoDePagoDeUnGasto(Gasto gasto);
        Task<Ticket> ObtenerTicketDeUnGasto(Gasto gasto);
        Task<List<Item>> ObtenerItemsDeUnGasto(Gasto gasto);
    }
    public class GastoEspecificoRepositorio : IGastoEspecificoRepositorio
    {
        private readonly GestionDeGastosBdContext _contexto;

        public GastoEspecificoRepositorio(GestionDeGastosBdContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }
        public async Task<Gasto> ObtenerGastoPorID(int id)
        {
            var gasto = await _contexto.Gastos.FirstOrDefaultAsync(g => g.IdGasto == id);
            //if (gasto == null)
            //{
            //    throw new Exception("Gasto no encontrado");
            //}
            return gasto;
        }
        public async Task<Categorium> ObtenerCategoriaDeUnGasto(Gasto gasto)
        {
            var categoria = await _contexto.Gastos
                                        .Where(g => g.IdGasto == gasto.IdGasto)
                                        .Select(g => g.IdCategoriaNavigation)
                                        .FirstOrDefaultAsync();

            //if (categoria == null)
            //{
            //    throw new Exception("Categoria no encontrada");
            //}
            return categoria;
        }
        public async Task<MetodoDePago> ObtenerMetodoDePagoDeUnGasto(Gasto gasto)
        {
            var metodoDePago = await _contexto.Gastos
                                        .Where(g => g.IdGasto == gasto.IdGasto)
                                        .Select(g => g.IdMetodoPagoNavigation)
                                        .FirstOrDefaultAsync();

            //if (metodoDePago == null)
            //{
            //    throw new Exception("Metodo de pago no encontrado");
            //}
            return metodoDePago;
        }
        public async Task<Ticket> ObtenerTicketDeUnGasto(Gasto gasto)
        {
            var ticket = await _contexto.Gastos
                                    .Where(g => g.IdGasto == gasto.IdGasto)
                                    .Select(g => g.IdTicketNavigation)
                                    .FirstOrDefaultAsync();
            //if (ticket == null)
            //{
            //    throw new Exception("Este gasto no posee un ticket");
            //}
            return ticket;
        }
        public async Task<List<Item>> ObtenerItemsDeUnGasto(Gasto gasto)
        {
            var ticket = await ObtenerTicketDeUnGasto(gasto);
            //if (ticket == null)
            //{
            //    throw new Exception("No hay items asignados a este gasto ya que tampoco hay un ticket");
            //}
            if (ticket != null && ticket.IdTicket != 0)
            {
                var items = await _contexto.Items
                                    .Where(i => i.IdTicket == ticket.IdTicket)
                                    .ToListAsync();
                return items;
            }
            //if (items == null)
            //{
            //    throw new Exception("No hay items asignados a este gasto");
            //}
            return new List<Item>();
        }
    }
}