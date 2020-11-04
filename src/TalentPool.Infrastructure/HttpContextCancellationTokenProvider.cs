using Microsoft.AspNetCore.Http;
using System.Threading;

namespace TalentPool.Infrastructure
{
    public class HttpContextCancellationTokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public HttpContextCancellationTokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public CancellationToken Token => _contextAccessor?.HttpContext?.RequestAborted ?? CancellationToken.None;
    }
}
