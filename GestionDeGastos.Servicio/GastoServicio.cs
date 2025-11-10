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
        Task ActualizarGastoAsync(Gasto gasto);
    }
    public class GastoServicio : IGastoServicio
    {
        private readonly IGastoRepositorio _gastoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMetodoDePagoRepositorio _metodoDePagoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public GastoServicio(IGastoRepositorio gastoRepositorio, ICategoriaRepositorio categoriaRepositorio, IMetodoDePagoRepositorio metodoDePagoRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _gastoRepositorio = gastoRepositorio;
            _categoriaRepositorio = categoriaRepositorio;
            _metodoDePagoRepositorio = metodoDePagoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }
       
        public async Task AgregarGastoAsync(Gasto gasto)
        {

            // que el usuario del gasto exista en la base de datos 
            if (await _usuarioRepositorio.GetByIdAsync(gasto.IdUsuario) == null)
                throw new ArgumentException("El usuario asociado al gasto no existe.");

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

            await _gastoRepositorio.AgregarGastoAsync(gasto);
        }
        public async Task ActualizarGastoAsync(Gasto gasto)
        {
            await _gastoRepositorio.ActualizarGastoAsync(gasto);
        }
    }
}
