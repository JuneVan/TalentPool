using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Une.TalentPool.Infrastructure.Notify
{
    public class NotifyFilter : IActionFilter, IAsyncResultFilter
    {
        public const string CookiePrefix = "van_notify";
        private readonly INotifier _notifier;
        private readonly INotifySerializer _notifySerializer;
        private NotifyEntry[] _existingEntries = Array.Empty<NotifyEntry>();
        private bool _shouldDeleteCookie;

        public NotifyFilter(
            INotifier notifier,
            INotifySerializer notifySerializer)
        {
            _notifySerializer = notifySerializer;

            _notifier = notifier;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var messages = Convert.ToString(filterContext.HttpContext.Request.Cookies[CookiePrefix]);
            if (string.IsNullOrEmpty(messages))
            {
                return;
            }

            NotifyEntry[] messageEntries = _notifySerializer.Deserialize(messages);

            if (messageEntries == null)
            {
                // An error occurred during deserialization
                _shouldDeleteCookie = true;
                return;
            }

            if (messageEntries.Length == 0)
            {
                return;
            }

            // Make the notifications available for the rest of the current request.
            _existingEntries = messageEntries;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var messageEntries = _notifier.NotifyEntries;

            // Don't touch temp data if there's no work to perform.
            if (messageEntries.Length == 0 && _existingEntries.Length == 0)
            {
                return;
            }

            // Assign values to the Items collection instead of TempData and
            // combine any existing entries added by the previous request with new ones.

            _existingEntries = messageEntries.Concat(_existingEntries).Distinct(new NotifyEntryComparer()).ToArray();

            // Result is not a view, so assume a redirect and assign values to TemData.
            // String data type used instead of complex array to be session-friendly.
            if (!(filterContext.Result is ViewResult || filterContext.Result is PageResult) && _existingEntries.Length > 0)
            {
                filterContext.HttpContext.Response.Cookies.Append(CookiePrefix, _notifySerializer.Serialize(_existingEntries), new CookieOptions { HttpOnly = true });
            }
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext filterContext, ResultExecutionDelegate next)
        {
            if (_shouldDeleteCookie)
            {
                DeleteCookies(filterContext);

                await next();
                return;
            }

            if (!(filterContext.Result is ViewResult || filterContext.Result is PageResult))
            {
                await next();
                return;
            }

            if (_existingEntries.Length == 0)
            {
                await next();
                return;
            }

          ((Controller)filterContext.Controller).ViewData["NotifyEntities"] = _existingEntries;
            DeleteCookies(filterContext);

            await next();
        }

        private void DeleteCookies(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cookies.Delete(CookiePrefix, new CookieOptions { HttpOnly = true });
        }
    }
}
