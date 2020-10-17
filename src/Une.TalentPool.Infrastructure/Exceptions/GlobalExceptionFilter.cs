using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Une.TalentPool.Infrastructure.Notify;

namespace Une.TalentPool.Infrastructure.Exceptions
{
    public class GlobalExceptionFilter : IExceptionFilter, IAsyncExceptionFilter, IActionFilter
    {
        public const string CookiePrefix = "van_notify";
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly INotifier _notifier;
        private readonly INotifySerializer _notifySerializer;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger,
            IMemoryCache memoryCache,
             INotifier notifier,
             INotifySerializer notifySerializer)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _notifier = notifier;
            _notifySerializer = notifySerializer;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 存储上一次请求的链接地址，用于异常处理返回目标地址
            if (context.HttpContext.Request.Method == "GET")
            {
                _memoryCache.Set($"Request.QueryString.{context.HttpContext.User.Identity.Name}", $"{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

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
                if (context.Exception is ArgumentException || context.Exception is ArgumentNullException || context.Exception is InvalidOperationException)
                {
                    _notifier.Warning(context.Exception.Message);
                    _logger.LogWarning(context.Exception, context.Exception.Message);
                }
                else
                {
                    _notifier.Error(context.Exception.Message);
                    _logger.LogError(context.Exception, context.Exception.Message);
                }
                // 添加异常消息，重定向链接
                var messageEntries = _notifier.NotifyEntries;
                context.HttpContext.Response.Cookies.Append(CookiePrefix, _notifySerializer.Serialize(messageEntries), new CookieOptions { HttpOnly = true });
                if (_memoryCache.TryGetValue($"Request.QueryString.{context.HttpContext.User.Identity.Name}", out string requestPath) && context.HttpContext.Request.Method == "POST")
                {
                    context.HttpContext.Response.Redirect(requestPath);
                }
                else
                {
                    context.HttpContext.Response.Redirect("/Error/E500");
                }
            }
        }
    }
}
