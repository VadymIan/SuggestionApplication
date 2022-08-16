using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using SuggestionApplication.Api.Registrations;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting the web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureSerilog();
    builder.ConfigureSwagger();
    builder.ConfigureServices(builder.Configuration);
    builder.Services.AddMemoryCache();
    builder.Services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

    var app = builder.Build();

    app.UseSerilog();
    app.UseCustomMiddleware();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwaggerPage();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;