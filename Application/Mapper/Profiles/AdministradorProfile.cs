using Application.Requests.Administrador;
using Application.Responses.Administrador;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper.Profiles
{
    public class AdministradorProfile : Profile
    {
        public AdministradorProfile()
        {
            CreateMap<CreateAdminRequest, Administrador>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsAtivo, opt => opt.Ignore());

            CreateMap<UpdateAdminRequest, Administrador>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsAtivo, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Administrador, AdministradorResponse>(); 
        }
    }
}
