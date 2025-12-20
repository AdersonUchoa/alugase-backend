using Domain.Interfaces.Seedworks;
using Infrastructure.Seedworks;

namespace AlugaSe.WebAPI.IoC
{
    public static class DomainDependencyInjection
    {
        public static void AddDomain(this IServiceCollection services)
        {
            AddSeedWorks(services);
        }

        private static void AddSeedWorks(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
