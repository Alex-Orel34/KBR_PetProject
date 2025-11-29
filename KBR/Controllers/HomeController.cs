using Microsoft.AspNetCore.Mvc;

namespace KBR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

