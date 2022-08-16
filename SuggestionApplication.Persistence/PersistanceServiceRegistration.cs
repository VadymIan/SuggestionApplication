using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Persistence.Repositories;

namespace SuggestionApplication.Persistence
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SuggestionApplicationDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("Azure")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ISuggestionRepository, SuggestionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
