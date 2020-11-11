using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TalentPool.Infrastructure.Exceptions
{
    public class GlobalExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        private readonly ILogger _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }


        public void OnException(ExceptionContext context)
        {
            HandleException(context);
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            HandleException(context);
            await Task.CompletedTask;
        }

        private void HandleException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                context.ExceptionHandled = true;
                if (context.Exception is OperationCanceledException) // 取消异常不处理
                {
                    return;
                }
                if (context.Exception is ArgumentException || context.Exception is ArgumentNullException || context.Exception is InvalidOperationException)
                {
                    _logger.LogWarning(context.Exception, context.Exception.Message);
                }
                else
                {
                    _logger.LogError(context.Exception, context.Exception.Message);
                }
                context.HttpContext.Response.Redirect("/Error/E500");
            }
        }
    }
}
