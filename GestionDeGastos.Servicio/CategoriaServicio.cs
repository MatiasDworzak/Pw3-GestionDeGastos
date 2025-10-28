using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface ICategoriaServicio
    {
        Task<IEnumerable<Categorium>> ObtenerTodasLasCategoriasDelUsuarioAsync(int idUsuario);
    }
    public class CategoriaServicio : ICategoriaServicio
    {
        private readonly ICategoriaRepositorio _repositorio;

        public CategoriaServicio(ICategoriaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<IEnumerable<Categorium>> ObtenerTodasLasCategoriasDelUsuarioAsync(int idUsuario)
        {
            if (idUsuario <= 0) throw new ArgumentException("El ID de usuario no es válido.", nameof(idUsuario));
            
            return await _repositorio.ObtenerTodasLasCategoriasPorUsuarioAsync(idUsuario);
        }
    }
}
