using SuggestionApplication.Application;
using SuggestionApplication.Infrastructure;
using SuggestionApplication.Persistence;

namespace SuggestionApplication.Api.Registrations
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(configuration);
            builder.Services.AddPersistenceServices(configuration);
            builder.Services.AddAuthenticationService(configuration);
        }
    }
}
