using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Omnitech.Dal.AdoNet;
using Omnitech.Dtos;
using Omnitech.Models;
using Omnitech.Service;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omnitech.Controllers
{
    public class KNController : Controller
    {
        private readonly KNInvoiceService _kNInvoiceService;
        private readonly Tps575UrlService _tps575UrlService;

        public KNController(KNInvoiceService kNInvoiceService, Tps575UrlService tps575UrlService)
        {
            _kNInvoiceService = kNInvoiceService;
            _tps575UrlService = tps575UrlService;
        }

        public async Task<IActionResult> Index()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            KNInvoiceDto knInvoiceDto = new KNInvoiceDto
            {
                KNInvoiceInfos = await _kNInvoiceService.GetKNInvoiceInfosByStartDateAndEndDateAsync(startDate, endDate)

            };

            return View(knInvoiceDto);
        }

        public async Task<IActionResult> GetByStartDateAndEndDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate < Convert.ToDateTime("01.01.2000") || endDate < Convert.ToDateTime("01.01.2000"))
                    throw new Exception("tarix duzgun deyil");

                if (startDate > endDate)
                    throw new Exception("Baslanic tarix bitme tarixinden boyuk ola bilmez");

                KNInvoiceDto knInvoiceDto = new KNInvoiceDto
                {
                    KNInvoiceInfos = await _kNInvoiceService.GetKNInvoiceInfosByStartDateAndEndDateAsync(startDate, endDate)

                };

              
                return PartialView("_KNInvoiceInfoPartial", knInvoiceDto);
            }

            catch (Exception exp)
            {
                ViewBag.Message = exp.Message;

                return Json(new { responseText = exp.Message, status = 500 });
            }
        }

        public async Task<IActionResult> GetKNInvoiceDetailsByInvIdAndMebleg(int invId, double mebleg, double difference)
        {
            try
            {
                if (invId == 0)
                    throw new Exception("Qaime secilmeyib");

                if (mebleg == 0)
                    throw new Exception("Mebleg 0 ola bilmez");

                if (mebleg > difference)
                    throw new Exception("Mebleq Faktura mebleginden boyuk ola bilmez");

                KNInvoiceDto knInvoiceDto = new KNInvoiceDto
                {
                    KNInvoiceDetails = await _kNInvoiceService.GetKNInvoiceDetailsByInvIdAndMeblegAsync(invId, mebleg)
                };

                return PartialView("_KNInvoiceDetailPartial", knInvoiceDto);
            }

            catch (Exception exp)
            {
                return Json(new { responseText = exp.Message, status = 500 });

            }
        }
    }
}