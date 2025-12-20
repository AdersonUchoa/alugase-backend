using Application.Requests.Imovel;
using Application.Responses.Imovel;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mapper.Profiles
{
    public class ImovelProfile : Profile
    {
        public ImovelProfile()
        {
            CreateMap<CreateImovelRequest, Imovel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsAtivo, opt => opt.Ignore())
                .ForMember(dest => dest.Aluguels, opt => opt.Ignore());

            CreateMap<UpdateImovelRequest, Imovel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsAtivo, opt => opt.Ignore())
                .ForMember(dest => dest.Aluguels, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Imovel, ImovelResponse>()
                .ForMember(dest => dest.TotalAlugueis, opt => opt.MapFrom(src =>
                    (src.Aluguels ?? new List<Aluguel>()).Count))
                .ForMember(dest => dest.AlugueisAtivos, opt => opt.MapFrom(src =>
                    (src.Aluguels ?? new List<Aluguel>()).Count(a =>
                        a.IsAtivo == true && a.Status == AluguelStatusesEnum.EmAndamento)))
                .ForMember(dest => dest.Aluguels, opt => opt.MapFrom(src =>
                    src.Aluguels ?? new List<Aluguel>()));
        }
    }
}
