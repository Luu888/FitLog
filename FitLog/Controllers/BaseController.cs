using Microsoft.AspNetCore.Mvc;

namespace FitLog.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
