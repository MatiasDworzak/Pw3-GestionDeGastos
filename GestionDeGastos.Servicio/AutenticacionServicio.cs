using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GestionDeGastos.Models;
using GestionDeGastos.Repositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Seguridad;
using Validacion;

namespace GestionDeGastos.Servicio
{
   public interface IAutenticacionServicio
   {
      Task<Usuario?> RegistrarUsuarioAsync(RegistroViewModel model);
      Task<Usuario?> ValidarUsuarioAsync(CredencialViewModel model);
   }


   internal class AutenticacionServicio : IAutenticacionServicio
   {
      private readonly IUsuarioRepositorio _usuarioRepositorio;

      private readonly IPasswordHasher _hasher;

      private readonly IMapper _mapper;

      public AutenticacionServicio(IUsuarioRepositorio usuarioRepositorio,
         IPasswordHasher hasher,
         IMapper maper)
      {
         _usuarioRepositorio = usuarioRepositorio;
         _hasher = hasher;
         _mapper = maper;
      }

      public async Task<Usuario?> RegistrarUsuarioAsync(RegistroViewModel model)
      {
         ValidacionUsuario.Registro(model);
         var usuarioExistente = await _usuarioRepositorio.GetByEmailAsync(model.Correo);
         
         if(usuarioExistente == null)
         {
            return null;
         }

         string contraseniaHash = _hasher.Hash(model.Contrasenia);
         var nuevoUsuario = _mapper.Map<Usuario>(model);
         nuevoUsuario.Contrasenia = contraseniaHash;
         await _usuarioRepositorio.AddAsync(nuevoUsuario);
         return nuevoUsuario;

      }

      public async Task<Usuario?> ValidarUsuarioAsync(CredencialViewModel model)
      {
         ValidacionUsuario.Login(model);
         var usuario = await _usuarioRepositorio.GetByEmailAsync(model.Correo);
         if( usuario == null)
         {
            return  null;
         }
         bool contraseniaValidada = _hasher.Verificar(model.Contrasenia,usuario.Contrasenia);

         return contraseniaValidada ? usuario : null;

      }
   }
}
