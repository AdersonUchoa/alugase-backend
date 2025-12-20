using Application.Requests.Aluguel;
using Application.Responses.Aluguel;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper.Profiles
{
    public class AluguelProfile : Profile
    {
        public AluguelProfile()
        {
            CreateMap<CreateAluguelRequest, Aluguel>();

            CreateMap<UpdateAluguelRequest, Aluguel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Aluguel, AluguelResponse>();
        }
    }
}
