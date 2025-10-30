using GestionDeGastos;
using GestionDeGastos.Repositorio;

namespace GestionDeGastos.Servicio.GastoEspecifico
{
    public interface IGastoEspecificoServicio
    {
        Task<Gasto> ObtenerGastoPorID(int id);
        Task<Categorium> ObtenerCategoriaDeUnGasto(Gasto gasto);
        Task<MetodoDePago> ObtenerMetodoDePagoDeUnGasto(Gasto gasto);
        Task<Ticket> ObtenerTicketDeUnGasto(Gasto gasto);
        Task<List<Item>> ObtenerItemsDeUnGasto(Gasto gasto);
    }

    public class GastoEspecificoServicio : IGastoEspecificoServicio
    {
        private readonly IGastoEspecificoRepositorio _repositorio;

        public GastoEspecificoServicio(IGastoEspecificoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        Task<Gasto> IGastoEspecificoServicio.ObtenerGastoPorID(int id)
        {
            return _repositorio.ObtenerGastoPorID(id);
        }
        Task<Categorium> IGastoEspecificoServicio.ObtenerCategoriaDeUnGasto(Gasto gasto)
        {
            return _repositorio.ObtenerCategoriaDeUnGasto(gasto);
        }
        Task<MetodoDePago> IGastoEspecificoServicio.ObtenerMetodoDePagoDeUnGasto(Gasto gasto)
        {
            return _repositorio.ObtenerMetodoDePagoDeUnGasto(gasto);
        }
        Task<Ticket> IGastoEspecificoServicio.ObtenerTicketDeUnGasto(Gasto gasto)
        {
            return _repositorio.ObtenerTicketDeUnGasto(gasto);
        }
        Task<List<Item>> IGastoEspecificoServicio.ObtenerItemsDeUnGasto(Gasto gasto)
        {
            return _repositorio.ObtenerItemsDeUnGasto(gasto);
        }

        //public GastoEspecificoViewModel ObtenerGastoPorID(int id)
        //{
        //    var gasto = _contexto.
        //        .Include(g => g.Items)
        //        .FirstOrDefault(g => g.Id == id);

        //    if (gasto == null)
        //    {
        //        return null;
        //    }

        //    return new GastoEspecificoViewModel
        //    {
        //        Nombre = gasto.Nombre,
        //        Monto = gasto.Monto,
        //        Categoria = gasto.Categoria,
        //        MetodoDePago = gasto.MetodoDePago,
        //        Fecha = gasto.Fecha,
        //        URLFoto = gasto.URLFoto,
        //        Items = gasto.Item.Select(i => new ItemGastoEspecificoViewModel
        //        {
        //            Cantidad = i.Cantidad,
        //            Descripcion = i.Descripcion,
        //            PrecioUnitario = i.PrecioUnitario,
        //            PrecioTotal = i.PrecioTotal
        //        }).ToList()
        //    };
        //}
    }
}
