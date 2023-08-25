using Microsoft.AspNetCore.Mvc;
using Omnitech.Service;
using System.Threading.Tasks;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using Omnitech.Dtos;
using Omnitech.Models;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using OpenXmlPowerTools;

namespace Omnitech.Controllers
{
    public class ProblemicSalesLogsController : BaseController
    {
        private readonly PrintService _printService;
        private readonly PrintTimerService _printTimerService;
        private readonly Tps575UrlService _tps575UrlService;
        private readonly UserMenuManager _userMenuManager;

        public ProblemicSalesLogsController(PrintService printService, PrintTimerService printTimerService, Tps575UrlService tps575UrlService, UserMenuManager userMenuManager) : base(userMenuManager)
        {
            _printService = printService;
            _printTimerService = printTimerService;
            _tps575UrlService = tps575UrlService;
            _userMenuManager = userMenuManager;
        }

        public async Task<IActionResult> Index()
        {
            string username = HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(username))
                throw new Exception($"Not authorized");

            Tps575Url tpsUrl = await _tps575UrlService.GetUrlByUsernameAsync(username);

            if (tpsUrl == null || tpsUrl == default || string.IsNullOrEmpty(tpsUrl.URL))
                throw new Exception("Url not found!");

            List<SalesLogs> salesLogs = await _printService.GetProblemicSalesLogsAsync();

            return CreateActionResultInstance(salesLogs.Where(x=>x.IP_REQUEST==tpsUrl.URL).ToList());
        }



        public async Task<IActionResult> Print(int recno, string url)
        {
            try
            {
                await _printService.PrintProblemicSalesLogsByRecnoAsync(recno, url);

                return Json(new { responseText = "SUCCESS", status = 200 });
            }

            catch (Exception exp)
            {
                ViewBag.Message = exp.Message;

                return Json(new { responseText = exp.Message, status = 500 });
            }
        }

    }
}
