using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using Omnitech.Models;
using Omnitech.Service;
using System.Threading.Tasks;

namespace Omnitech.Controllers
{
    public class HomeController : Controller
    {
        private readonly Tps575UrlService _tps575UrlService;

        public HomeController(Tps575UrlService tps575UrlService)
        {
            _tps575UrlService = tps575UrlService;
        }

        public IActionResult Index()
        {
            return View();
        }


        
    }
}
