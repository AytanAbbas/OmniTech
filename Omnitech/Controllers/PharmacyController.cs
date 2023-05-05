using Microsoft.AspNetCore.Mvc;
using Omnitech.Dtos;
using Omnitech.Models;
using Omnitech.Service;
using OpenXmlPowerTools;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Omnitech.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly PharmacyInvoiceService _pharmacyInvoiceService;
        private readonly Tps575UrlService _tps575UrlService;

        public PharmacyController(PharmacyInvoiceService pharmacyInvoiceService, Tps575UrlService tps575UrlService)
        {
            _pharmacyInvoiceService = pharmacyInvoiceService;
            _tps575UrlService = tps575UrlService;
        }

        public async Task<IActionResult> Index()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            PharmacyInvoiceDto pharmacyInvoiceDto = new PharmacyInvoiceDto
            {
                PharmacyInvoiceInfos = await _pharmacyInvoiceService.GetPharmacyInvoiceInfosByStartDateAndEndDateAsync(startDate, endDate),
                FoodSupplementItems = await _pharmacyInvoiceService.GetAllFoodSuppLementItemsAsync(),

            };

            return View(pharmacyInvoiceDto);
        }

        public async Task<IActionResult> GetByStartDateAndEndDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate < Convert.ToDateTime("01.01.2000") || endDate < Convert.ToDateTime("01.01.2000"))
                    throw new Exception("tarix duzgun deyil");

                if (startDate > endDate)
                    throw new Exception("Baslanic tarix bitme tarixinden boyuk ola bilmez");

                PharmacyInvoiceDto pharmacyInvoiceDto = new PharmacyInvoiceDto
                {
                    PharmacyInvoiceInfos = await _pharmacyInvoiceService.GetPharmacyInvoiceInfosByStartDateAndEndDateAsync(startDate, endDate)
                };

                PharmacyInvoiceInfo pharmacyInvoiceInfo = pharmacyInvoiceDto.PharmacyInvoiceInfos.FirstOrDefault();

                pharmacyInvoiceDto.PharmacyInvoiceDetails = await _pharmacyInvoiceService.GetPharmacyInvoiceDetailsBySourceIndexAndDateAsync(pharmacyInvoiceInfo.ANBAR, pharmacyInvoiceInfo.TARIX);
                return PartialView("_PharmacyInvoiceInfoPartial", pharmacyInvoiceDto);
            }

            catch (Exception exp)
            {
                ViewBag.Message = exp.Message;

                return Json(new { responseText = exp.Message, status = 500 });
            }
        }

        public async Task<IActionResult> GetPharmacyInvoiceDetailsBySourceIndexAndDate(int sourceIndex, DateTime date)
        {
            try
            {
                if (sourceIndex == 0)

                    throw new Exception("anbar secilmeyib");

                if (date < Convert.ToDateTime("01.01.2000"))
                    throw new Exception("tarix duzgun deyil");


                PharmacyInvoiceDto pharmacyInvoiceDto = new PharmacyInvoiceDto
                {
                    PharmacyInvoiceDetails = await _pharmacyInvoiceService.GetPharmacyInvoiceDetailsBySourceIndexAndDateAsync(sourceIndex, date),
                };

                return PartialView("_PharmacyInvoiceDetailPartial", pharmacyInvoiceDto);
            }

            catch (Exception exp)
            {
                return Json(new { responseText = exp.Message, status = 500 });
               
            }
        }

        public async Task<IActionResult> GetPharmacyInvoiceReplacedItemSum(int sourceIndex, DateTime date)
        {
            try
            {
                if (sourceIndex == 0)

                    throw new Exception("anbar secilmeyib");

                if (date < Convert.ToDateTime("01.01.2000"))
                    throw new Exception("tarix duzgun deyil");


                PharmacyInvoiceDto pharmacyInvoiceDto = new PharmacyInvoiceDto
                {
                    PharmacyInvoiceItemReplaceResponse = await _pharmacyInvoiceService.GetPharmacyInvoiceItemReplaceResponseByDateAndSourceIndex(sourceIndex, date)
                };

                return PartialView("_PharmacyInvoiceItemReplacePartial", pharmacyInvoiceDto);
            }

            catch (Exception exp)
            {
                return Json(new { responseText = exp.Message, status = 500 });
            }
        }

        public async Task<IActionResult> ReplacePharmacyInvoiceItem(int sourceIndex, DateTime date)
        {
            try
            {
                if (sourceIndex == 0)

                    throw new Exception("anbar secilmeyib");

                if (date < Convert.ToDateTime("01.01.2000"))
                    throw new Exception("tarix duzgun deyil");

                PharmacyInvoiceDto pharmacyInvoiceDto = new PharmacyInvoiceDto
                {
                    PharmacyInvoiceItemReplaceResponse = await _pharmacyInvoiceService.ReplacePharmacyInvoiceItemAsync(sourceIndex, date)
                };

                return PartialView("_PharmacyInvoiceItemReplacePartial", pharmacyInvoiceDto);
            }

            catch (Exception exp)
            {
                return Json(new { responseText = exp.Message, status = 500 });
            }
        }

        public async Task<IActionResult> AddFoodSupplementItem(int sku, DateTime date, int sourceIndex, double quantity)
        {
            try
            {
                if (sku == 0)

                    throw new Exception("Mal secilmeyib");

                if (date < Convert.ToDateTime("01.01.2000"))
                    throw new Exception("tarix duzgun deyil");


                if (sourceIndex == 0)

                    throw new Exception("anbar secilmeyib");

                if (quantity == 0)

                    throw new Exception("mal sayi daxil edilmeyib");


                await _pharmacyInvoiceService.AddFoodSupplementItemAsync(sku, date, sourceIndex, quantity);

                PharmacyInvoiceDto pharmacyInvoiceDto = new PharmacyInvoiceDto
                {
                    PharmacyInvoiceDetails = await _pharmacyInvoiceService.GetPharmacyInvoiceDetailsBySourceIndexAndDateAsync(sourceIndex, date),
                };

                return PartialView("_PharmacyInvoiceDetailPartial", pharmacyInvoiceDto);
            }

            catch (Exception exp)
            {
                return Json(new { responseText = exp.Message, status = 500 });
            }
        }

        public async Task<IActionResult> DeletePharmacyInvoiceDetailLine(int logicalref)
        {
            try
            {
                if (logicalref == 0)

                    throw new Exception("Mal secilmeyib");

                await _pharmacyInvoiceService.DeletePharmacyInvoiceDetailLine(logicalref);
                return Json(new { result = 200 });
            }

            catch (Exception exp)
            {
                return Json(new { responseText = exp.Message, status = 500 });
            }
        }
    }
}