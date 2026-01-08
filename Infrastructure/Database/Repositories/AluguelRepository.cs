using Domain.Entities;
using Domain.Enums;
using Domain.Filters;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class AluguelRepository : IAluguelRepository
    {
        private readonly AlugaSeContext _context;
        private readonly DbSet<Aluguel> _alugueis;
        public AluguelRepository(AlugaSeContext context)
        {
            _context = context;
            _alugueis = context.Aluguels;
        }

        public async Task<Aluguel> AddAsync(Aluguel aluguel)
        {
            var entity = await _alugueis.AddAsync(aluguel);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<Aluguel?> GetByIdAsync(int id)
        {
            return await _alugueis
                .Include(a => a.Imovel)
                .Include(a => a.Inquilino)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Aluguel> UpdateAsync(Aluguel aluguel)
        {
            _alugueis.Update(aluguel);
            await _context.SaveChangesAsync();
            return aluguel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var aluguel = await _alugueis.FindAsync(id);
            if (aluguel == null) return false;

            aluguel.IsAtivo = false;
            aluguel.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Aluguel> Get(AluguelFilterRequest request, bool includeInactive = false, bool includeImoveis = false, bool includeInquilinos = false, string? search = null)
        {
            var query = _alugueis.AsNoTracking();

            if (!includeInactive)
                query = query.Where(i => i.IsAtivo == true);

            if (includeImoveis)
                query = query.Include(a => a.Imovel);

            if (includeInquilinos)
                query = query.Include(a => a.Inquilino);

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();

                query = query.Where(a =>
                    (a.Inquilino != null && a.Inquilino.Nome.ToLower().Contains(searchLower)) ||
                    (a.Imovel != null && a.Imovel.Nome != null && a.Imovel.Nome.ToLower().Contains(searchLower))
                );
            }

            if (request.PeriodoInicio.HasValue)
            {
                query = query.Where(a => a.DataFim >= request.PeriodoInicio.Value);
            }

            if (request.PeriodoFim.HasValue)
            {
                query = query.Where(a => a.DataInicio <= request.PeriodoFim.Value);
            }

            if (request.ValorMin.HasValue)
                query = query.Where(a => a.Valor >= request.ValorMin.Value);

            if (request.ValorMax.HasValue)
                query = query.Where(a => a.Valor <= request.ValorMax.Value);

            if (request.Status != null && request.Status.Any())
                query = query.Where(a => request.Status.Contains(a.Status));

            if (request.MetodosPagamento != null && request.MetodosPagamento.Any())
                query = query.Where(a => request.MetodosPagamento.Contains(a.MetodoDePagamento));

            return query.OrderByDescending(a => a.CreatedAt);
        }

        public async Task<int> GetTotalAlugueisAsync(bool onlyActive = true)
        {
            if (onlyActive)
                return await _alugueis.CountAsync(i => i.IsAtivo == true);

            return await _alugueis.CountAsync();
        }

        public async Task<List<Aluguel>> GetAlugueisPorPagamentoAsync(MetodoPagamentoEnum pag, bool includeInactive = false)
        {
            var query = _alugueis.AsNoTracking().Where(a => a.MetodoDePagamento == pag);

            if (!includeInactive)
                query = query.Where(a => a.IsAtivo == true);

            return await query.ToListAsync();
        }

        public async Task<List<Aluguel>> GetAlugueisPorStatusAsync(AluguelStatusesEnum status, bool includeInactive = false)
        {
            var query = _alugueis.AsNoTracking().Where(a => a.Status == status);

            if (!includeInactive)
                query = query.Where(a => a.IsAtivo == true);

            return await query.ToListAsync();
        }

        public async Task<List<Aluguel>> GetAlugueisPorInquilinoIdAsync(int inquilinoId, bool includeInactive = false)
        {
            var query = _alugueis.AsNoTracking().Where(a => a.InquilinoId == inquilinoId);

            if (!includeInactive)
                query = query.Where(a => a.IsAtivo == true);

            return await query.ToListAsync();
        }

        public async Task<List<Aluguel>> GetAlugueisPorImovelIdAsync(int imovelId, bool includeInactive = false)
        {
            var query = _alugueis.AsNoTracking().Where(a => a.ImovelId == imovelId);

            if (!includeInactive)
                query = query.Where(a => a.IsAtivo == true);

            return await query.ToListAsync();
        }
    }
}
