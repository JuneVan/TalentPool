using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TalentPool.Infrastructure.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next; 
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is ArgumentNullException || ex is InvalidOperationException)
                {
                    logger.LogWarning(ex, ex.Message);
                }
                else
                {
                    logger.LogError(ex, ex.Message);
                }
                httpContext.Response.Redirect("/Error/E500");
            }
        }
    }
}
