using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using Omnitech.Models;
using Omnitech.Dal.AdoNet;
using System.Collections.Generic;
using OpenXmlPowerTools;
using static Omnitech.Utilities.Enums;
using System.Net;
using Omnitech.Utilities;

namespace Omnitech.Service
{

    public class PharmacyInvoicePrintService : OmnitechPrintService
    {
        int jobId = (int)JobsPermission.PharmacyPrintService;

        private readonly PharmacyInvoiceSalesLogsRepository _salesLogsRepository;
        private readonly JobPermissionRepository _jobPermissionRepository;
        private readonly PrintService _printService;

        public PharmacyInvoicePrintService(PharmacyInvoiceSalesLogsRepository salesLogsRepository, JobPermissionRepository jobPermissionRepository, PrintService printService)
        {
            _salesLogsRepository = salesLogsRepository;
            _jobPermissionRepository = jobPermissionRepository;
            _printService = printService;
        }

        public async Task<string> SendKassaAsync(int anbar, DateTime tarix, string faktura, int chekSayi)
        {
            string result = "SUCCESS";
            try
            {
                if (await _salesLogsRepository.InvoiceHasExists(faktura))
                    throw new Exception("Bu qaime capa gonderilib");

                if (Enums.Tps575Url == null)
                    throw new Exception("Url is empty");

                string url = Enums.Tps575Url.URL;

                OmnitechLoginResponse loginResponse = await Login();

                Tps575Logs tps575Logs = new Tps575Logs
                {
                    FAKTURA_NAME = faktura,
                    insert_date = DateTime.Now,
                    request = StandartJsons.LoginJson,
                    response = System.Text.Json.JsonSerializer.Serialize(loginResponse)
                };

                await _printService.AddTps575LogsAsync(tps575Logs);

                await _salesLogsRepository.SendKassaAsync(anbar, tarix, loginResponse.access_token, url, faktura, chekSayi);
            }

            catch (Exception exp)
            {
                string logMsg = $"{anbar}  --> {faktura} --> {tarix.ToString("dd.MM.yyyy")} --> {exp.Message}";

                await _jobPermissionRepository.AddJobExceptionAsync(jobId, exp.Message);

                result = exp.Message;
            }

            return result;
        }
    }
}
