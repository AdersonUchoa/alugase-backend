using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlugaSe.WebAPI.IoC
{
    public static class InfrastructureDependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDatabaseContexts(services, configuration);
            AddRepositories(services);
        }

        private static void AddDatabaseContexts(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

            services.AddDbContext<AlugaSeContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSingleton<DapperContext>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IAdministradorRepository, AdministradorRepository>();
            services.AddScoped<IInquilinoRepository, InquilinoRepository>();
            services.AddScoped<IImovelRepository, ImovelRepository>();
            services.AddScoped<IAluguelRepository, AluguelRepository>();
        }
    }
}