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

    public class KNInvoicePrintService : OmnitechPrintService
    {
        int jobId = (int)JobsPermission.PharmacyPrintService;

        private readonly KNInvoiceSalesLogsRepository _salesLogsRepository;
        private readonly JobPermissionRepository _jobPermissionRepository;
        private readonly PrintService _printService;

        public KNInvoicePrintService(KNInvoiceSalesLogsRepository salesLogsRepository, JobPermissionRepository jobPermissionRepository, PrintService printService)
        {
            _salesLogsRepository = salesLogsRepository;
            _jobPermissionRepository = jobPermissionRepository;
            _printService = printService;
        }

        public async Task<string> SendKassaAsync(int invId, double mebleg, string faktura)
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

                await _salesLogsRepository.SendKassaAsync(invId, mebleg, loginResponse.access_token, url, faktura);
            }

            catch (Exception exp)
            {
                string logMsg = $"{invId}  --> {faktura} --> {mebleg} --> {exp.Message}";

                await _jobPermissionRepository.AddJobExceptionAsync(jobId, logMsg);

                result = logMsg;
            }

            return result;
        }
    }
}
