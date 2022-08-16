using Serilog;

namespace SuggestionApplication.Api.Registrations
{
    public static class RegisterSerilog
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) => configuration
                   .ReadFrom.Configuration(context.Configuration)
                   .ReadFrom.Services(services)
                   .Enrich.FromLogContext()
                   .WriteTo.File("log.txt")
                   .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().Contains("swagger"))));
        }

        public static IApplicationBuilder UseSerilog(this IApplicationBuilder builder)
        {
            builder.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });

            return builder;
        }
    }
}
