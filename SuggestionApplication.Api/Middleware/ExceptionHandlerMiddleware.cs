using Newtonsoft.Json;
using SuggestionApplication.Application.Exceptions;
using System.Net;

namespace SuggestionApplication.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _request;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate request)
        {
            _logger = logger;
            _request = request;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception ex)
            {
                await LogException(context, ex);
            }
        }

        private Task LogException(HttpContext context, Exception exception)
        {
            _logger.LogError(exception.Message);

            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = badRequestException.Message;
                    break;
                case NotFoundException notFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    result = notFoundException.Message;
                    break;
                case Exception ex:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = ex.Message;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            result = JsonConvert.SerializeObject(new { error = exception.Message });

            return context.Response.WriteAsync(result);
        }
    }
}
