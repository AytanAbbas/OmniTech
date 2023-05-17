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

    public class KNInvoicePrintService
    {
        int jobId = (int)JobsPermission.PharmacyPrintService;

        private readonly KNInvoiceSalesLogsRepository _salesLogsRepository;
        private readonly JobPermissionRepository _jobPermissionRepository;
        private readonly PrintService _printService;
        private readonly Tps575LogsRepository _tps575LogsRepository;

        public KNInvoicePrintService(KNInvoiceSalesLogsRepository salesLogsRepository, JobPermissionRepository jobPermissionRepository, PrintService printService, Tps575LogsRepository tps575LogsRepository)
        {
            _salesLogsRepository = salesLogsRepository;
            _jobPermissionRepository = jobPermissionRepository;
            _printService = printService;
            _tps575LogsRepository = tps575LogsRepository;
        }

        public async Task<string> SendKassaAsync(int invId, double mebleg, string faktura)
        {
            string result = "SUCCESS";
            try
            {
                if (Enums.Tps575Url == null)
                    throw new Exception("Url is empty");
               
                string url = Enums.Tps575Url.URL;

                OmnitechLoginResponse loginResponse = await OmnitechPrintService.Login();

                Tps575Logs tps575Logs = new Tps575Logs
                {
                    FAKTURA_NAME = faktura,
                    insert_date = DateTime.Now,
                    request = StandartJsons.LoginJson,
                    response = System.Text.Json.JsonSerializer.Serialize(loginResponse)
                };

                await _printService.AddTps575LogsAsync(tps575Logs);

                await _salesLogsRepository.SendKassaAsync(invId, mebleg, loginResponse.access_token, url, faktura);

                await _printService.PrintAsync(faktura, loginResponse);
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
