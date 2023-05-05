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

        public async Task<IActionResult> Index(string fakturaName, int anbar, DateTime date, int reseptCount)
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


                string result = await _pharmacyInvoicePrintService.SendKassaAsync(anbar, date, fakturaName, reseptCount);

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
