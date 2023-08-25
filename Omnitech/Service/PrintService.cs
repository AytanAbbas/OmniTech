using Omnitech.Dal.AdoNet;
using Omnitech.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using static Omnitech.Utilities.Enums;
using OpenXmlPowerTools;

namespace Omnitech.Service
{
    public class PrintService
    {
        private static readonly object LockAll = new object();
        private static readonly object LockSingle = new object();

        int jobId = (int)JobsPermission.PharmacyPrintService;

        private readonly SalesLogsRepository _salesLogsRepository;
        private readonly JobPermissionRepository _jobPermissionRepository;
        private readonly Tps575LogsRepository _tps575LogsRepository;
        public PrintService(SalesLogsRepository salesLogsRepository, JobPermissionRepository jobPermissionRepository, Tps575LogsRepository tps575LogsRepository)
        {
            _salesLogsRepository = salesLogsRepository;
            _jobPermissionRepository = jobPermissionRepository;
            _tps575LogsRepository = tps575LogsRepository;
        }

        public async Task PrintAllAsync()
        {
            lock (LockAll)
            {
                // PrintAsync().GetAwaiter().GetResult();
            }
        }

        public async Task PrintAsync(string fakturaName, OmnitechLoginResponse loginResponse, string url)
        {
            DateTime now = DateTime.Now;
            try
            {
                List<SalesLogs> salesLogsList = await _salesLogsRepository.GetSalesLogsByFakturaNameAsync(fakturaName);

                OmnitechShiftResponse omnitechShiftResponse = await OmnitechPrintService.CheckShiftAsync(loginResponse, url);
               

                foreach (var salesLogs in salesLogsList)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(salesLogs.RESPONSE_TPS575))
                            await PrintSingleAsync(salesLogs, loginResponse, omnitechShiftResponse, url);
                    }

                    catch (Exception exp)
                    {
                        try
                        {
                            string logMsg = $"{salesLogs.RECNO} --> {salesLogs.FAKTURA_NAME} --> {exp.Message}";

                            await _jobPermissionRepository.AddJobExceptionAsync(jobId, logMsg);
                        }

                        catch (Exception exp2)
                        {
                            string expMsg = exp2.Message;
                        }
                    }

                }
            }


            catch (Exception exp)
            {
                await _jobPermissionRepository.AddJobExceptionAsync(jobId, exp.Message);
            }
        }


        public async Task AddTps575LogsAsync(Tps575Logs tps575Logs)
        {
            await _tps575LogsRepository.AddTps575LogsAsync(tps575Logs);

        }

        public async Task PrintSingleAsync(SalesLogs salesLogs, OmnitechLoginResponse loginResponse, OmnitechShiftResponse omnitechShiftResponse,string url)
        {
            SalesLogs salesLogsFromDb = await _salesLogsRepository.GetSalesLogsByRecnoAsync(salesLogs.RECNO);

            if (salesLogsFromDb == null)
                throw new Exception($@"{salesLogs.RECNO} --> {salesLogs.FAKTURA_NO} --> qaime tapilmadi");

            else
            {
                if (!string.IsNullOrEmpty(salesLogsFromDb.FICSAL_DOCUMENT))
                    throw new Exception($@"{salesLogs.RECNO} --> {salesLogs.FAKTURA_NO} --> Bu qaime capa gedib");
            }

            OmnitechOpenShiftResponse omnitechOpenShiftResponse = null;

            if (!omnitechShiftResponse.shiftStatus)
            {
                omnitechOpenShiftResponse = await OmnitechPrintService.OpenShiftAsync(loginResponse, url);

                Tps575Logs tps575Logs = new Tps575Logs
                {
                    FAKTURA_NAME = salesLogs.FAKTURA_NAME,
                    insert_date = DateTime.Now,
                    request = StandartJsons.OpenShiftJson(loginResponse),
                    response = System.Text.Json.JsonSerializer.Serialize(omnitechOpenShiftResponse)
                };

                await AddTps575LogsAsync(tps575Logs);
            }


            string responseText = await OmnitechPrintService.InvokeAsync(salesLogs.REQUEST_TPS575,url);

            OmnitechSaleResponse omnitechSaleResponse = System.Text.Json.JsonSerializer.Deserialize<OmnitechSaleResponse>(responseText);

            salesLogs.RESPONSE_TPS575 = responseText;
            salesLogs.FICSAL_DOCUMENT_LONG = omnitechSaleResponse.long_id;

            Tps575Logs tps575LogsForSale = new Tps575Logs
            {
                FAKTURA_NAME = salesLogs.FAKTURA_NAME,
                insert_date = DateTime.Now,
                request = salesLogs.REQUEST_TPS575,
                response = responseText
            };

            await AddTps575LogsAsync(tps575LogsForSale);

            await _salesLogsRepository.UpdateResponseAsync(salesLogs.RECNO, responseText);

            await _salesLogsRepository.AddLogAsync(salesLogs.RECNO, salesLogs.FAKTURA_NO,salesLogs.REQUEST_TPS575, responseText);
        }
   
        public async Task<List<SalesLogs>> GetProblemicSalesLogsAsync()
        {
            return await _salesLogsRepository.GetProblemicSalesLogsForPrintAsync();
        }

        public async Task PrintProblemicSalesLogsByRecnoAsync(int recno,string url)
        {
            SalesLogs salesLogs = null;
            try
            {
                salesLogs = await _salesLogsRepository.GetSalesLogsByRecnoAsync(recno);


                if (!string.IsNullOrEmpty(salesLogs.RESPONSE_TPS575))
                {

                    OmnitechSaleResponse omnitechSaleResponse = System.Text.Json.JsonSerializer.Deserialize<OmnitechSaleResponse>(salesLogs.RESPONSE_TPS575);

                    if (omnitechSaleResponse.code != 0)
                    {
                        OmnitechLoginResponse loginResponse = await OmnitechPrintService.Login(url);

                        OmnitechShiftResponse omnitechShiftResponse = await OmnitechPrintService.CheckShiftAsync(loginResponse, url);

                        await PrintSingleAsync(salesLogs, loginResponse, omnitechShiftResponse,url);
                    }
                }

                else
                {
                    OmnitechLoginResponse loginResponse = await OmnitechPrintService.Login(url);

                    OmnitechShiftResponse omnitechShiftResponse = await OmnitechPrintService.CheckShiftAsync(loginResponse,url);

                    await PrintSingleAsync(salesLogs, loginResponse, omnitechShiftResponse, url);
                }

            }

            catch (Exception exp)
            {
                string logMsg = $"{salesLogs.RECNO} --> {salesLogs.FAKTURA_NAME} --> {exp.Message}";

                await _jobPermissionRepository.AddJobExceptionAsync(jobId, logMsg);
            }
        }
    }
}
