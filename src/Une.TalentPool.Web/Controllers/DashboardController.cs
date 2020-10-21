using Microsoft.AspNetCore.Mvc;

namespace Une.TalentPool.Web.Controllers
{
    public class DashboardController : Controller
    { 
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
