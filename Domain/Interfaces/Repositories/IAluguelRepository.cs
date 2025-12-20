using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.Repositories
{
    public interface IAluguelRepository
    {
        Task<Aluguel> AddAsync(Aluguel aluguel);
        Task<Aluguel> UpdateAsync(Aluguel aluguel);
        Task<bool> DeleteAsync(int id);
        Task<Aluguel?> GetByIdAsync(int id);
        IQueryable<Aluguel> Get(bool includeInactive = false, bool includeImoveis = false, bool includeInquilinos = false, string? search = null);
        Task<int> GetTotalAlugueisAsync(bool onlyActive = true);
        Task<List<Aluguel>> GetAlugueisPorPagamentoAsync(MetodoPagamentoEnum metodoPagamento, bool includeInactive = false);
        Task<List<Aluguel>> GetAlugueisPorStatusAsync(AluguelStatusesEnum status, bool includeInactive = false);
        Task<List<Aluguel>> GetAlugueisPorInquilinoIdAsync(int inquilinoId, bool includeInactive = false);
        Task<List<Aluguel>> GetAlugueisPorImovelIdAsync(int imovelId, bool includeInactive = false);
    }
}
