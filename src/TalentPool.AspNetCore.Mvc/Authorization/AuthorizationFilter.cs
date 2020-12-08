using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Threading.Tasks;

namespace TalentPool.AspNetCore.Mvc.Authorization
{
    public class AuthorizationFilter : IAuthorizationFilter, IAsyncAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Check(context);
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Check(context);
            await Task.CompletedTask;
        }
        private void Check(AuthorizationFilterContext context)
        {
            var principal = context.HttpContext.User;
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            // 是否进行授权检查
            var isRequired = false;
            // 控制器授权
            var controllerAuthorizeAttribute = actionDescriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>();
            // 匿名授权
            var allowAnonymousAttribute = actionDescriptor.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();
            // 方法授权
            var actionAuthorizeAttribute = actionDescriptor.MethodInfo.GetCustomAttribute<AuthorizeAttribute>();
            if (controllerAuthorizeAttribute != null)
            {
                isRequired = true;
                if (allowAnonymousAttribute != null)
                    isRequired = false;
            }
            else
            {
                if (actionAuthorizeAttribute != null)
                    isRequired = true;
            }
            if (isRequired)
            {
                if (!(principal?.Identity?.IsAuthenticated ?? false))
                    context.Result = new UnauthorizedResult();
                // 权限检查
                var controllerAuthorizeCheckAttribute = actionDescriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeCheckAttribute>();
                if (controllerAuthorizeCheckAttribute != null && !principal.HasClaim(AppConstants.ClaimTypes.Permission, controllerAuthorizeCheckAttribute.Permission))
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var actionAuthorizeCheckAttribute = actionDescriptor.MethodInfo.GetCustomAttribute<AuthorizeCheckAttribute>();
                    if (actionAuthorizeCheckAttribute != null && !principal.HasClaim(AppConstants.ClaimTypes.Permission, actionAuthorizeCheckAttribute.Permission))
                    {
                        context.Result = new ForbidResult();
                    }
                }

            }

        }
    }

}
