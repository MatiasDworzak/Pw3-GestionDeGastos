using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionDeGastos.Repositorio;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace GestionDeGastos.Servicio
{
    public interface IHomeService
    {
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        Task<Presupuesto?> ObtenerPresupuestoPorIdAsync(int id);
    }
    public class HomeServicio : IHomeService
    {
        private readonly IPresupuestoRepositorio _presupuestoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public HomeServicio(IUsuarioRepositorio iUsuarioRepo, IPresupuestoRepositorio iPresupuestoRepo)
        {
            _presupuestoRepositorio = iPresupuestoRepo;
            _usuarioRepositorio = iUsuarioRepo;
        }

        public async Task<Presupuesto?> ObtenerPresupuestoPorIdAsync(int id)
        {
            return await _presupuestoRepositorio.GetByIdAsync(id);
            
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _usuarioRepositorio.GetByIdAsync(id);
        }
    }
}
