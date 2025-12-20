using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly AlugaSeContext _context;
        private readonly DbSet<Administrador> _administradores;
        public AdministradorRepository(AlugaSeContext context)
        {
            _context = context;
            _administradores = context.Administradors;
        }

        public async Task<Administrador> AddAsync(Administrador administrador)
        {
            var entity = await _administradores.AddAsync(administrador);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<Administrador?> GetByIdAsync(int id)
        {
            return await _administradores
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Administrador> UpdateAsync(Administrador administrador)
        {
            _administradores.Update(administrador);
            await _context.SaveChangesAsync();
            return administrador;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var administrador = await _administradores.FindAsync(id);
            if (administrador == null) return false;

            administrador.IsAtivo = false;
            administrador.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Administrador> Get(bool includeInactive = false, string? search = null)
        {
            var query = _administradores.AsNoTracking();

            if (!includeInactive)
                query = query.Where(a => a.IsAtivo == true);

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(a => 
                    a.Login.ToLower().Contains(searchLower)
                );
            }

            return query.OrderByDescending(a => a.CreatedAt);
        }

        public async Task<Administrador?> GetByLoginAsync(string login)
        {
            return await _administradores
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Login == login && a.IsAtivo == true);
        }

        public async Task<bool> ExistsByLoginAsync(string login, int? excludeId = null)
        {
            var query = _administradores.AsNoTracking()
                .Where(a => a.Login == login && a.IsAtivo == true);

            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<int> GetTotalAdministradoresAsync(bool onlyActive = true)
        {
            var query = _administradores.AsNoTracking();

            if (onlyActive)
                query = query.Where(a => a.IsAtivo == true);

            return await query.CountAsync();
        }
    }
}
