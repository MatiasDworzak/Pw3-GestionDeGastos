using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GestionDeGastos.Models;

namespace GestionDeGastos.Servicio
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<RegistroViewModel, Usuario>()
            .ForMember(dest => dest.Email,
                       opt => opt.MapFrom(src => src.Correo)) 

             // Ignoramos la contraseña del ViewModel porque la hashearemos manualmente
             .ForMember(dest => dest.Contrasenia,
                         opt => opt.Ignore())          

             // Ignoramos el ID porque lo genera la BD/Repo, no viene del ViewModel
             .ForMember(dest => dest.IdUsuario,
                        opt => opt.Ignore());

         // Nota: 'Nombre' se mapea automáticamente porque los nombres coinciden.
         // 'ConfirmarContrasenia' se ignora automáticamente porque no existe en 'Usuario'.
      }
   }
}
