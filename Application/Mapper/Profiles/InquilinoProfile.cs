using Application.Requests.Inquilino;
using Application.Responses.Inquilino;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mapper.Profiles
{
    public class InquilinoProfile : Profile
    {
        public InquilinoProfile()
        {
            CreateMap<CreateInquilinoRequest, Inquilino>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsAtivo, opt => opt.Ignore())
                .ForMember(dest => dest.Aluguels, opt => opt.Ignore());

            CreateMap<UpdateInquilinoRequest, Inquilino>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsAtivo, opt => opt.Ignore())
                .ForMember(dest => dest.Aluguels, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Inquilino, InquilinoUpdateResponse>();

            CreateMap<Inquilino, InquilinoResponse>()
                .ForMember(dest => dest.TotalAlugueis, opt => opt.MapFrom(src =>
                    (src.Aluguels ?? new List<Aluguel>()).Count))
                .ForMember(dest => dest.AlugueisAtivos, opt => opt.MapFrom(src =>
                    (src.Aluguels ?? new List<Aluguel>()).Count(a => a.IsAtivo == true && a.Status == AluguelStatusesEnum.EmAndamento)))
                .ForMember(dest => dest.Aluguels, opt => opt.MapFrom(src =>
                    src.Aluguels ?? new List<Aluguel>()));
        }
    }
}
