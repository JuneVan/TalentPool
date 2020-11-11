using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TalentPool.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : WebControllerBase
    {
        public ErrorController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
        public IActionResult E404()
        {
            return View();
        }
        public IActionResult E500()
        {
            return View();
        }
    }
}
