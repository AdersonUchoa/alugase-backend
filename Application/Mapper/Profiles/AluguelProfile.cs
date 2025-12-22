using Application.Requests.Aluguel;
using Application.Responses.Aluguel;
using Application.Responses.Imovel;
using Application.Responses.Inquilino;
using AutoMapper;
using Domain.Entities;
using Domain.Extensions;

namespace Application.Mapper.Profiles
{
    public class AluguelProfile : Profile
    {
        public AluguelProfile()
        {
            CreateMap<CreateAluguelRequest, Aluguel>();

            CreateMap<UpdateAluguelRequest, Aluguel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Aluguel, AluguelResponse>()
                .ForMember(dest => dest.MetodoDePagamento, opt => opt.MapFrom(src => src.MetodoDePagamento.Value()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value()))
                .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => src.Status.Value()))
                .ForMember(dest => dest.Inquilino, opt => opt.MapFrom(src => src.Inquilino != null
                    ? new InquilinoResponse
                    {
                        Id = src.Inquilino.Id,
                        Nome = src.Inquilino.Nome,
                        Telefone = src.Inquilino.Telefone,
                        Cpf = src.Inquilino.Cpf,
                        Email = src.Inquilino.Email
                    }
                    : null))
                .ForMember(dest => dest.Imovel, opt => opt.MapFrom(src => src.Imovel != null
                    ? new ImovelResponse
                    {
                        Id = src.Imovel.Id,
                        Nome = src.Imovel.Nome,
                        Endereco = src.Imovel.Endereco
                    }
                    : null));
        }
    }
}
