using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Application.Models;
using SuggestionApplication.Infrastructure.Filters;
using SuggestionApplication.Infrastructure.Services;
using SuggestionApplication.Infrastructure.Mail;

namespace SuggestionApplication.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        // "IConfiguration configuration" can be injected in constructor if there is a necessity to get some data from config
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ISuggestionService, SuggestionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<PerformanceFilterAsync>();

            return services;
        }
    }
}
