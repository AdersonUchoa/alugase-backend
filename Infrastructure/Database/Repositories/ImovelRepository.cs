using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class ImovelRepository : IImovelRepository
    {
        private readonly AlugaSeContext _context;
        private readonly DbSet<Imovel> _imoveis;
        public ImovelRepository(AlugaSeContext context)
        {
            _context = context;
            _imoveis = context.Imovels;
        }

        public async Task<Imovel> AddAsync(Imovel imovel)
        {
            var entity = await _imoveis.AddAsync(imovel);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<Imovel?> GetByIdAsync(int id)
        {
            return await _imoveis
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Imovel> UpdateAsync(Imovel imovel)
        {
            _imoveis.Update(imovel);
            await _context.SaveChangesAsync();
            return imovel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var imovel = await _imoveis.FindAsync(id);
            if (imovel == null) return false;
            imovel.IsAtivo = false;
            imovel.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Imovel> Get(bool includeInactive = false, bool includeAlugueis = false, string? search = null)
        {
            var query = _imoveis.AsNoTracking();

            if (!includeInactive)
                query = query.Where(i => i.IsAtivo == true);

            if (includeAlugueis)
                query = query.Include(i => i.Aluguels);

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(i =>
                    i.Nome.Contains(searchLower, StringComparison.CurrentCultureIgnoreCase) ||
                    i.Endereco != null && i.Endereco.ToLower().Contains(searchLower) ||
                    i.Descricao != null && i.Descricao.ToLower().Contains(searchLower));
            }

            return query.OrderByDescending(i => i.CreatedAt);
        }

        public async Task<int> GetTotalImoveisAsync(bool onlyActive = true)
        {
            if (onlyActive)
                return await _imoveis.CountAsync(i => i.IsAtivo == true);

            return await _imoveis.CountAsync();
        }

        public async Task<int> GetTotalImoveisComAlugueisAtivosAsync()
        {
            return await _imoveis
                .AsNoTracking()
                .Where(i => i.IsAtivo == true &&
                    i.Aluguels.Any(a =>
                        a.IsAtivo == true &&
                        a.Status == AluguelStatusesEnum.EmAndamento))
                .CountAsync();
        }

        public async Task<List<Imovel>> GetByTipoAsync(TipoImovelEnum tipo, bool includeInactive = false)
        {
            var query = _imoveis.AsNoTracking().Where(i => i.TipoImovel == tipo);

            if (!includeInactive)
                query = query.Where(i => i.IsAtivo == true);

            return await query.ToListAsync();
        }

        public async Task<List<Imovel>> GetImoveisComAlugueisAtivosAsync()
        {
            return await _imoveis
                .AsNoTracking()
                .Include(i => i.Aluguels.Where(a =>
                    a.IsAtivo == true &&
                    a.Status == AluguelStatusesEnum.EmAndamento))
                    .ThenInclude(a => a.Inquilino)
                .Where(i => i.IsAtivo == true &&
                    i.Aluguels.Any(a =>
                        a.IsAtivo == true &&
                        a.Status == AluguelStatusesEnum.EmAndamento))
                .ToListAsync();
        }

        public async Task<List<Imovel>> GetImoveisSemAlugueisAsync()
        {
            return await _imoveis
                .AsNoTracking()
                .Where(i => i.IsAtivo == true &&
                    !i.Aluguels.Any(a => a.IsAtivo == true &&
                        a.Status == AluguelStatusesEnum.EmAndamento))
                .ToListAsync();
        }

        public async Task<Imovel?> GetImovelByAluguelIdAsync(int aluguelId, bool includeInactive = false)
        {
            var query = _context.Aluguels
                .AsNoTracking()
                .Include(a => a.Imovel)
                .Where(a => a.Id == aluguelId);

            if (!includeInactive)
                query = query.Where(a => a.Imovel.IsAtivo == true);

            var aluguel = await query.FirstOrDefaultAsync();

            return aluguel?.Imovel;
        }

        public async Task<Imovel?> GetByNomeAsync(string nome, bool includeInactive = false)
        {
            var query = _imoveis.AsNoTracking().Where(i => i.Nome.ToLower() == nome.ToLower());

            if (!includeInactive)
                query = query.Where(i => i.IsAtivo == true);
            
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByNomeAsync(string nome, int? excludeId = null)
        {
            var query = _imoveis.AsNoTracking().Where(i => i.Nome.ToLower() == nome.ToLower());
            
            if (excludeId.HasValue)
            {
                query = query.Where(i => i.Id != excludeId.Value);
            
            }
            return await query.AnyAsync();
        }
    }
}
