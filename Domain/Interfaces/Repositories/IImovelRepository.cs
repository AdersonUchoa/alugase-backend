
using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.Repositories
{
    public interface IImovelRepository
    {
        Task<Imovel> AddAsync(Imovel imovel);
        Task<Imovel> UpdateAsync(Imovel imovel);
        Task<bool> DeleteAsync(int id);
        Task<Imovel?> GetByIdAsync(int id);
        IQueryable<Imovel> Get(bool includeInactive = false, bool includeAlugueis = false, string? search = null);
        Task<int> GetTotalImoveisAsync(bool onlyActive = true);
        Task<int> GetTotalImoveisComAlugueisAtivosAsync();
        Task<Imovel?> GetByNomeAsync(string nome, bool includeInactive = false);
        Task<bool> ExistsByNomeAsync(string nome, int? excludeId = null);
        Task<List<Imovel>> GetByTipoAsync(TipoImovelEnum tipo, bool includeInactive = false);
        Task<List<Imovel>> GetImoveisComAlugueisAtivosAsync();
        Task<List<Imovel>> GetImoveisSemAlugueisAsync();
        Task<Imovel?> GetImovelByAluguelIdAsync(int aluguelId, bool includeInactive = false);
        Task<bool> IsDisponivelAsync(int imovelId, DateOnly data);
    }
}
