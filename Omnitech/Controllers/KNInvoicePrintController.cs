using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Omnitech.Dtos;
using Omnitech.Models;
using Omnitech.Service;
using OpenXmlPowerTools;
using System;
using System.Threading.Tasks;

namespace Omnitech.Controllers
{

    public class KNInvoicePrintController : Controller
    {
        private readonly KNInvoicePrintService _knInvoicePrintService;
        private readonly PrintTimerService _printTimerService;
        private readonly KNInvoiceService _kNInvoiceService;
        private readonly Tps575UrlService _tps575UrlService;

        public KNInvoicePrintController(KNInvoicePrintService knInvoicePrintService, PrintTimerService printTimerService, KNInvoiceService kNInvoiceService, Tps575UrlService tps575UrlService)
        {
            _knInvoicePrintService = knInvoicePrintService;
            _printTimerService = printTimerService;
            _kNInvoiceService = kNInvoiceService;
            _tps575UrlService = tps575UrlService;
        }

        public async Task<IActionResult> Index(int invId, double mebleg, double difference, string faktura)
        {

            try
            {
                if (invId == 0)
                    throw new Exception("Qaime secilmeyib");

                if (mebleg == 0)
                    throw new Exception("Mebleg 0 ola bilmez");

                if (mebleg > difference)
                    throw new Exception("Mebleq ferqden  boyuk ola bilmez");

                if (string.IsNullOrEmpty(faktura))
                    throw new Exception("Faktura secilmeyib");

                string result = await _knInvoicePrintService.SendKassaAsync(invId, mebleg, faktura);


                if (!result.Equals("SUCCESS"))
                    throw new Exception(result);



                KNInvoiceDto knInvoiceDto = new KNInvoiceDto
                {
                    KNInvoiceDetails = await _kNInvoiceService.GetKNInvoiceDetailsByInvIdAndMeblegAsync(invId, mebleg)
                };

                return PartialView("_KNInvoiceDetailPartial", knInvoiceDto);
            }

            catch (Exception exp)
            {
                ViewBag.Message = exp.Message;

                 return Json(new { responseText = exp.Message, status = 500 });
            }

        }
        //public async Task<IActionResult> ZReport()
        //{
        //    await _knInvoicePrintService.ZReportAsync();

        //    return Json(new { responseText = "" });

        //}
    }
}
