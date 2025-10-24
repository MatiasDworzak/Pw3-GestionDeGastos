
using AutoMapper;
using GestionDeGastos.Servicio.DTOs;

namespace GestionDeGastos.Servicio
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<RegistrarUsuarioDto, Usuario>()
            .ForMember(dest => dest.Email,opt => opt.MapFrom(src => src.Correo))
            .ForMember(dest => dest.Contrasenia, opt => opt.Ignore())
            .ForMember(dest => dest.IdUsuario, opt => opt.Ignore());
      }
   }
}
