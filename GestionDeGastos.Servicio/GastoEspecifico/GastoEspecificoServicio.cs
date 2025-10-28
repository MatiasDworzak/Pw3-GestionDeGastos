using GestionDeGastos.AccesoADatos;
using GestionDeGastos.Repositorio;

namespace GestionDeGastos.Servicio.GastoEspecifico
{
    public interface IGastoEspecificoServicio
    {
        Task<Gasto> ObtenerGastoPorID(int id);
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
