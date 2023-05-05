using Microsoft.AspNetCore.Mvc;

namespace Omnitech.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
