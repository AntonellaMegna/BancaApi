using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BancaApi.ActionFilter
{
    public class ExFilter : IExceptionFilter
    {
        private readonly ILogger<ExFilter> _logger;


        public ExFilter(ILogger<ExFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {

            var errorResponse = new ErrorHandler("INTERNAL_SERVER_ERROR", context.Exception.Message, "");

            var statusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError(context.Exception, $"new exception arrived: {context.Exception.GetType()}");

            context.Result = new ContentResult
            {
                Content = JsonSerializer.Serialize(errorResponse),
                ContentType = "application/json",
                StatusCode = statusCode
            };
        }
    }
}
