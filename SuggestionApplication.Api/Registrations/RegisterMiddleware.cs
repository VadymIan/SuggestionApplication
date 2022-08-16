using SuggestionApplication.Api.Middleware;

namespace SuggestionApplication.Api.Registrations
{
    public static class RegisterMiddleware
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
