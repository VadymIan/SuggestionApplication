using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SuggestionApplication.Infrastructure.Filters
{
    public class PerformanceFilterAsync : IAsyncActionFilter
    {
        public PerformanceFilterAsync(ILogger<PerformanceFilterAsync> logger)
        {
            _logger = logger;
        }

        private readonly ILogger<PerformanceFilterAsync> _logger;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                await next();
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("{MethodName} took {Duration}ms", context.HttpContext.Request.Path, sw.ElapsedMilliseconds);
            }
        }
    }
}
