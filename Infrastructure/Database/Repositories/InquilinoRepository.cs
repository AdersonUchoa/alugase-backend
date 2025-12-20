using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class InquilinoRepository : IInquilinoRepository
    {
        private readonly AlugaSeContext _context;
        private readonly DbSet<Inquilino> _inquilinos;
        public InquilinoRepository(AlugaSeContext context)
        {
            _context = context;
            _inquilinos = context.Inquilinos;
        }

        public async Task<Inquilino> AddAsync(Inquilino inquilino)
        {
            var entity = await _inquilinos.AddAsync(inquilino);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<Inquilino?> GetByIdAsync(int id)
        {
            return await _inquilinos
                .Include(i => i.Aluguels)
                    .ThenInclude(a => a.Imovel)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inquilino> UpdateAsync(Inquilino inquilino)
        {
            _inquilinos.Update(inquilino);
            await _context.SaveChangesAsync();
            return inquilino;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var inquilino = await _inquilinos.FindAsync(id);
            if (inquilino == null) return false;

            inquilino.IsAtivo = false;
            inquilino.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Inquilino> Get(bool includeInactive = false, bool includeAlugueis = false, string? search = null)
        {
            var query = _inquilinos.AsNoTracking();

            if (!includeInactive)
                query = query.Where(i => i.IsAtivo == true);

            if (includeAlugueis)
                query = query.Include(i => i.Aluguels)
                    .ThenInclude(a => a.Imovel);

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(i =>
                    i.Nome.ToLower().Contains(searchLower) ||
                    (i.Email != null && i.Email.ToLower().Contains(searchLower)) || 
                    (i.Cpf != null && i.Cpf.Contains(search)) ||
                    i.Telefone.Contains(search) 
                );
            }

            return query.OrderByDescending(i => i.CreatedAt);
        }

        public async Task<Inquilino?> GetByCpfAsync(string cpf)
        {
            return await _inquilinos
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Cpf == cpf && i.IsAtivo == true);
        }

        public async Task<Inquilino?> GetByEmailAsync(string email)
        {
            return await _inquilinos
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Email == email && i.IsAtivo == true);
        }

        public async Task<Inquilino?> GetByTelefoneAsync(string telefone)
        {
            return await _inquilinos
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Telefone == telefone && i.IsAtivo == true);
        }

        public async Task<bool> ExistsByCpfAsync(string cpf, int? excludeId = null)
        {
            var query = _inquilinos.AsNoTracking()
                .Where(i => i.Cpf == cpf && i.IsAtivo == true);

            if (excludeId.HasValue)
                query = query.Where(i => i.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
        {
            var query = _inquilinos.AsNoTracking()
                .Where(i => i.Email == email && i.IsAtivo == true);

            if (excludeId.HasValue)
                query = query.Where(i => i.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsByTelefoneAsync(string telefone, int? excludeId = null)
        {
            var query = _inquilinos.AsNoTracking()
                .Where(i => i.Telefone == telefone && i.IsAtivo == true);
            if (excludeId.HasValue)
                query = query.Where(i => i.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<List<Inquilino>> GetInquilinosComAlugueisAtivosAsync()
        {
            return await _inquilinos
                .AsNoTracking()
                .Include(i => i.Aluguels.Where(a => 
                    a.IsAtivo == true && 
                    a.Status == AluguelStatusesEnum.EmAndamento))
                    .ThenInclude(a => a.Imovel)
                .Where(i => i.IsAtivo == true && 
                    i.Aluguels.Any(a => 
                        a.IsAtivo == true && 
                        a.Status == AluguelStatusesEnum.EmAndamento))
                .OrderBy(i => i.Nome)
                .ToListAsync();
        }

        public async Task<List<Inquilino>> GetInquilinosSemAlugueisAsync()
        {
            return await _inquilinos
                .AsNoTracking()
                .Where(i => i.IsAtivo == true &&
                    !i.Aluguels.Any(a =>
                        a.IsAtivo == true &&
                        a.Status == AluguelStatusesEnum.EmAndamento))
                .OrderBy(i => i.Nome)
                .ToListAsync();
        }

        public async Task<int> GetTotalInquilinosAsync(bool onlyActive = true)
        {
            var query = _inquilinos.AsNoTracking();

            if (onlyActive)
                query = query.Where(i => i.IsAtivo == true);

            return await query.CountAsync();
        }

        public async Task<int> GetTotalInquilinosComAlugueisAtivosAsync()
        {
            return await _inquilinos
                .AsNoTracking()
                .Where(i => i.IsAtivo == true &&
                    i.Aluguels.Any(a =>
                        a.IsAtivo == true &&
                        a.Status == AluguelStatusesEnum.EmAndamento))
                .CountAsync();
        }

        public async Task<Inquilino?> GetInquilinoByAluguelIdAsync(int aluguelId, bool includeInactive = false)
        {
            var query = _context.Aluguels
                .AsNoTracking()
                .Include(a => a.Inquilino)
                .Where(a => a.Id == aluguelId);

            if (!includeInactive)
                query = query.Where(a => a.Inquilino.IsAtivo == true);

            var aluguel = await query.FirstOrDefaultAsync();

            return aluguel?.Inquilino;
        }
    }
}
