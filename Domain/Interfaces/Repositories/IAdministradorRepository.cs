using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IAdministradorRepository
    {
        Task<Administrador> AddAsync(Administrador administrador);
        Task<Administrador> UpdateAsync(Administrador administrador);
        Task<bool> DeleteAsync(int id);
        Task<Administrador?> GetByIdAsync(int id);
        IQueryable<Administrador> Get(bool includeInactive = false, string? search = null);
        Task<int> GetTotalAdministradoresAsync(bool onlyActive = true);
        Task<Administrador?> GetByLoginAsync(string login);
        Task<bool> ExistsByLoginAsync(string login, int? excludeId = null);
    }
}
