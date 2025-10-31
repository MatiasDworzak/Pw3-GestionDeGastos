using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;
using GestionDeGastos.Servicio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface IGastoServicio
    {
        Task AgregarGastoAsync(Gasto gasto);
    }
    public class GastoServicio : IGastoServicio
    {
        private readonly IGastoRepositorio _gastoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMetodoDePagoRepositorio _metodoDePagoRepositorio;

        public GastoServicio(IGastoRepositorio gastoRepositorio, ICategoriaRepositorio categoriaRepositorio, IMetodoDePagoRepositorio metodoDePagoRepositorio)
        {
            _gastoRepositorio = gastoRepositorio;
            _categoriaRepositorio = categoriaRepositorio;
            _metodoDePagoRepositorio = metodoDePagoRepositorio;
        }
        public async Task AgregarGastoAsync(Gasto gasto)
        {
            // TODO: habria que validar que el usuario exista(usar repositorio de uri para traer al usuario con el id que viene en el gasto)

            
            // que el id de la categoria corresponda con opciones que si puede elegir el usuario
            IEnumerable<Categorium> categoriasDisponiblesParaElUsuario = await _categoriaRepositorio.ObtenerTodasLasCategoriasDisponiblesParaUsuarioPorIdAsync(gasto.IdUsuario);
            bool categoriaValidaParaElUsuario = categoriasDisponiblesParaElUsuario.Any(c => c.IdCategoria == gasto.IdCategoria);

            if (!categoriaValidaParaElUsuario) 
                throw new ArgumentException("La categoría seleccionada no es válida para el usuario que quiere realizar el gasto.");

            // y que el id del metodo de pago exista
            IEnumerable<MetodoDePago> metodosDePagoExistentes = await _metodoDePagoRepositorio.ObtenerTodosLosMetodosDePagoAsync();
            bool metodoDePagoValido = metodosDePagoExistentes.Any(m => m.IdMetodoPago == gasto.IdMetodoPago);

            if (!metodoDePagoValido)
                throw new ArgumentException("El método de pago seleccionado no existe.");

            // otras validaciones de negocio pueden ir aqui

            await _gastoRepositorio.AgregarGastoAsync(gasto);
        }
    }
}
