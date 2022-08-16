using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SuggestionApplication.Infrastructure.Interceptors
{
    // This ingterceptor needs to be registered.
    // Does nothing at this point
    public class DurationInterceptor : IInterceptor
    {
        private readonly ILogger<DurationInterceptor> _logger;

        public DurationInterceptor(ILogger<DurationInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                invocation.Proceed();
            }
            finally
            {
                sw.Stop();

                _logger.LogInformation("{MethodName} took {Duration}ms", invocation.Method.Name, sw.ElapsedMilliseconds);
            }
        }
    }
}
