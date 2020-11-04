using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading;

namespace TalentPool.Infrastructure
{
    public class ClaimPrincipalUserIdentifier : IUserIdentifier
    { 
        public Guid? UserId
        {

            get
            {
                var userIdValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Guid userId;
                if (!Guid.TryParse(userIdValue, out userId))
                    return null;
                return userId;
            }
        }

        protected virtual ClaimsPrincipal User
        {
            get
            {
                var user = _contextAccessor?.HttpContext?.User;
                if (user != null)
                    return user;
                return (ClaimsPrincipal)Thread.CurrentPrincipal;
            }
        }
        private readonly IHttpContextAccessor _contextAccessor;
        public ClaimPrincipalUserIdentifier(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }
}
