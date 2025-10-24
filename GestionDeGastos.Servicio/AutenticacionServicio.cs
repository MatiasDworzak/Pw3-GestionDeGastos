
using AutoMapper;
using GestionDeGastos.Repositorio;
using GestionDeGastos.Servicio.DTOs;
using GestionDeGastos.Servicio.Seguridad;
using Validacion;


namespace GestionDeGastos.Servicio
{
   public interface IAutenticacionServicio
   {
      Task<Usuario?> RegistrarUsuarioAsync(RegistrarUsuarioDto dto);
      Task<Usuario?> ValidarUsuarioAsync(ValidarUsuarioDto dto);
   }


   internal class AutenticacionServicio : IAutenticacionServicio
   {
      private readonly IUsuarioRepositorio _usuarioRepositorio;

      private readonly IContraseniaHasher _hasher;

      private readonly IMapper _mapper;

      public AutenticacionServicio(IUsuarioRepositorio usuarioRepositorio,
         IContraseniaHasher hasher,
         IMapper maper)
      {
         _usuarioRepositorio = usuarioRepositorio;
         _hasher = hasher;
         _mapper = maper;
      }

      public async Task<Usuario?> RegistrarUsuarioAsync(RegistrarUsuarioDto dto)
      {
         ValidacionUsuario.Registro(dto);
         var usuarioExistente = await _usuarioRepositorio.GetByEmailAsync(dto.Correo);
         
         if(usuarioExistente == null)
         {
            return null;
         }

         string contraseniaHash = _hasher.Hash(dto.Contrasenia);
         var nuevoUsuario = _mapper.Map<Usuario>(dto);
         nuevoUsuario.Contrasenia = contraseniaHash;
         await _usuarioRepositorio.AddAsync(nuevoUsuario);
         return nuevoUsuario;

      }

      public async Task<Usuario?> ValidarUsuarioAsync(ValidarUsuarioDto dto)
      {
         ValidacionUsuario.Login(dto);
         var usuario = await _usuarioRepositorio.GetByEmailAsync(dto.Correo);
         if( usuario == null)
         {
            return  null;
         }
         bool contraseniaValidada = _hasher.Verificar(dto.Contrasenia, dto.Contrasenia);

         return contraseniaValidada ? usuario : null;

      }
   }
}
