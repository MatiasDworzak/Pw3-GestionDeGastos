using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionDeGastos.Models;
using GestionDeGastos.Repositorio;
using Microsoft.AspNetCore.Identity;
using Seguridad;

namespace GestionDeGastos.Servicio
{
   public interface IAutenticacionService
   {
      Task<Usuario?> RegistrarUsuarioAsync(CredencialViewModel model);
      Task<Usuario?> ValidarUsuarioAsync(CredencialViewModel model);
   }


   internal class AutenticacionService : IAutenticacionService
   {
      private readonly IUsuarioRepositorio _usuarioRepositorio;
      private readonly IPasswordHasher _hasher;
      

      public AutenticacionService(IUsuarioRepositorio usuarioRepositorio)
      {
         _usuarioRepositorio = usuarioRepositorio;
      }

      public Task<Usuario?> RegistrarUsuarioAsync(CredencialViewModel model)
      {
         throw new NotImplementedException();
      }

      public Task<Usuario?> ValidarUsuarioAsync(CredencialViewModel model)
      {
         throw new NotImplementedException();
      }
   }
}
