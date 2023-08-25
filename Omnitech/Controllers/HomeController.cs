using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omnitech.Models;
using Omnitech.Service;
using OpenXmlPowerTools;
using System;
using System.Threading.Tasks;

namespace Omnitech.Controllers
{
    public class HomeController : BaseController
    {
        private readonly Tps575UrlService _tps575UrlService;
        private readonly OmnitechPrintService _omnitechPrintService;
        private readonly UserMenuManager _userMenuManager;


        public HomeController(UserMenuManager userMenuManager,Tps575UrlService tps575UrlService) : base(userMenuManager)
        {
           _userMenuManager = userMenuManager;
            _tps575UrlService = tps575UrlService;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            
            return CreateActionResultInstance();
        }

        public async Task<IActionResult> CloseShift()
        {
            try
            {
                string username = HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(username))
                    throw new Exception($"Not authorized");

                Tps575Url tpsUrl = await _tps575UrlService.GetUrlByUsernameAsync(username);

                if (tpsUrl == null || tpsUrl == default || string.IsNullOrEmpty(tpsUrl.URL))
                    throw new Exception("Url not found!");

                await OmnitechPrintService.ZReportAsync(tpsUrl.URL);
                return Json(new { responseText = "SUCCESS", status = 200 });
            }

            catch (Exception exp)
            {
                return Json(new { responseText = exp.Message, status = 500 });
            }
        }

    }
}
