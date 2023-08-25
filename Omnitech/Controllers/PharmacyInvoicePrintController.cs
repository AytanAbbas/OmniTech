using Microsoft.AspNetCore.Mvc;
using Omnitech.Models;
using Omnitech.Service;
using System;
using System.Threading.Tasks;

namespace Omnitech.Controllers
{

    public class PharmacyInvoicePrintController : Controller
    {
        private readonly PharmacyInvoicePrintService _pharmacyInvoicePrintService;
        private readonly PrintTimerService _printTimerService;
        private readonly Tps575UrlService _tps575UrlService;

        public PharmacyInvoicePrintController(PharmacyInvoicePrintService pharmacyInvoicePrintService, PrintTimerService printTimerService, Tps575UrlService tps575UrlService)
        {
            _pharmacyInvoicePrintService = pharmacyInvoicePrintService;
            _printTimerService = printTimerService;
            _tps575UrlService = tps575UrlService;
        }

        public async Task<IActionResult> Index(string fakturaName, int anbar, DateTime date, int reseptCount, double mebleg)
        {
            try
            {
                if (string.IsNullOrEmpty(fakturaName))
                    throw new Exception("Faktura secilmeyib");

                if (anbar == 0)
                throw new Exception("anbar secilmeyib");

                if (date < Convert.ToDateTime("01.01.2000"))
                    throw new Exception("tarix duzgun deyil");

                if (reseptCount == 0)
                    throw new Exception("setir sayi daxil edilmeyib");

                if (mebleg == 0)
                    throw new Exception("mebleg  daxil edilmeyib");

                string username = HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(username))
                    throw new Exception($"Not authorized");

                Tps575Url tpsUrl = await _tps575UrlService.GetUrlByUsernameAsync(username);

                if (tpsUrl == null || tpsUrl == default || string.IsNullOrEmpty(tpsUrl.URL))
                    throw new Exception("Url not found!");

                string result = await _pharmacyInvoicePrintService.SendKassaAsync(anbar, date, fakturaName, reseptCount,mebleg, tpsUrl.URL);

                if(!result.Equals("SUCCESS"))
                    throw new Exception(result);

                return Json(new { responseText = result });
            }
            catch (Exception exp)
            {
                ViewBag.Message = exp.Message;

                return Json(new { responseText = exp.Message, status = 500 });
            }
        }
    }
}
