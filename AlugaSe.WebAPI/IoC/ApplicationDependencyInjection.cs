using Application.Interfaces;
using Application.Mapper.Profiles;
using Application.Services;

namespace AlugaSe.WebAPI.IoC
{
    public static class ApplicationDependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddServices(services);
            AddAutoMapper(services);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAdministradorService, AdministradorService>();
            services.AddScoped<IInquilinoService, InquilinoService>();
            services.AddScoped<IImovelService, ImovelService>();
            services.AddScoped<IAluguelService, AluguelService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddMemoryCache();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AdministradorProfile).Assembly);
        }
    }
}