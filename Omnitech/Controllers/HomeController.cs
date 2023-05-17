using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using Omnitech.Models;
using Omnitech.Service;
using OpenXmlPowerTools;
using System;
using System.Threading.Tasks;

namespace Omnitech.Controllers
{
    public class HomeController : Controller
    {
        private readonly Tps575UrlService _tps575UrlService;
        private readonly OmnitechPrintService _omnitechPrintService;

        public HomeController(Tps575UrlService tps575UrlService, OmnitechPrintService omnitechPrintService)
        {
            _tps575UrlService = tps575UrlService;
            _omnitechPrintService = omnitechPrintService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CloseShift()
        {
            try
            {
                await OmnitechPrintService.ZReportAsync();
                return Json(new { responseText = "SUCCESS", status = 200 });
            }
            catch (Exception exp)
            {
                return Json(new { responseText = exp.Message, status = 500 });
            }
        }

    }
}
