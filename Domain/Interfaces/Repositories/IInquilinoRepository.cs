using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IInquilinoRepository
    {
        Task<Inquilino> AddAsync(Inquilino inquilino);
        Task<Inquilino> UpdateAsync(Inquilino inquilino);
        Task<bool> DeleteAsync(int id);
        Task<Inquilino?> GetByIdAsync(int id);
        IQueryable<Inquilino> Get(bool includeInactive = false, bool includeAlugueis = false, string? search = null);   
        Task<int> GetTotalInquilinosAsync(bool onlyActive = true);
        Task<int> GetTotalInquilinosComAlugueisAtivosAsync();
        Task<Inquilino?> GetByCpfAsync(string cpf);
        Task<Inquilino?> GetByEmailAsync(string email);
        Task<Inquilino?> GetByTelefoneAsync(string telefone);
        Task<bool> ExistsByCpfAsync(string cpf, int? excludeId = null);
        Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
        Task<bool> ExistsByTelefoneAsync(string telefone, int? excludeId = null);
        Task<List<Inquilino>> GetInquilinosComAlugueisAtivosAsync();
        Task<List<Inquilino>> GetInquilinosSemAlugueisAsync();
        Task<Inquilino?> GetInquilinoByAluguelIdAsync(int aluguelId, bool includeInactive = false);

    }
}
