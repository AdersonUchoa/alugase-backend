using Domain.Interfaces.Seedworks;
using Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seedworks
{
    public sealed class UnitOfWork : IDisposable, IUnitOfWork
    {
        //private readonly DapperContext _dapperContext; Verificar se é necessário
        private readonly AlugaSeContext _entityFrameworkContext;
        private bool _isDisposed;
        public UnitOfWork(AlugaSeContext entityFrameworkContext)
        {
            _entityFrameworkContext = entityFrameworkContext;
            _isDisposed = false;
        }

        //public MySqlConnection GetDapperConnection() => _dapperContext.GetConnection(); Verificar se é necessário

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default) =>
            _entityFrameworkContext.Database.BeginTransactionAsync(cancellationToken);

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _entityFrameworkContext.SaveChangesAsync(cancellationToken);
            await _entityFrameworkContext.Database.CommitTransactionAsync(cancellationToken);
        }

        public void Dispose() => Disposing(true);

        private void Disposing(bool disposing)
        {
            if (_isDisposed && disposing)
                _entityFrameworkContext.Dispose();

            _isDisposed = true;
        }

        public DbContext GetEfDbContext() => _entityFrameworkContext;

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _entityFrameworkContext.Database.RollbackTransactionAsync(cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _entityFrameworkContext.SaveChangesAsync(cancellationToken);
    }
}
