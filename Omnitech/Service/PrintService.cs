using Omnitech.Dal.AdoNet;
using Omnitech.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using static Omnitech.Utilities.Enums;

namespace Omnitech.Service
{
    public class PrintService : OmnitechPrintService
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
                PrintAsync().GetAwaiter().GetResult();
            }
        }

        private async Task PrintAsync()
        {
            DateTime now = DateTime.Now;
            JobPermission jobPermission = null;

            try
            {
                jobPermission = await _jobPermissionRepository.GetJobPermissionByIdAsync(jobId);

                if (jobPermission != null)
                {
                    if (jobPermission.HasPermission == 1 && jobPermission.IsRunning == 0 && jobPermission.NextRun <= now)
                    {
                        await _jobPermissionRepository.ChangePermissionAsync(jobPermission.Id, 1, jobPermission.IntervalUnit, jobPermission.IntervalType);

                        List<SalesLogs> salesLogsList = await _salesLogsRepository.GetAllSalesLogsForPrintAsync();

                        OmnitechLoginResponse loginResponse = await Login();
                        OmnitechShiftResponse omnitechShiftResponse = await CheckShiftAsync(loginResponse);

                        foreach (var salesLogs in salesLogsList)
                        {
                            try
                            {
                                if (await _jobPermissionRepository.GetHasPermissionById(jobId) == 1)
                                {
                                    lock (LockSingle)
                                    {
                                        PrintSingleAsync(salesLogs, loginResponse, omnitechShiftResponse).GetAwaiter().GetResult();
                                    }
                                }
                            }

                            catch (Exception exp)
                            {
                                try
                                {
                                    string logMsg = $"{salesLogs.RECNO} --> {salesLogs.FAKTURA_NAME} --> {exp.Message}";

                                    await _jobPermissionRepository.AddJobExceptionAsync(jobPermission.Id, exp.Message);
                                }

                                catch (Exception exp2)
                                {
                                    string expMsg = exp2.Message;
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception exp)
            {
                await _jobPermissionRepository.AddJobExceptionAsync(jobId, exp.Message);
            }

            finally
            {
                await _jobPermissionRepository.ChangePermissionAsync(jobPermission.Id, 0, jobPermission.IntervalUnit, jobPermission.IntervalType);
            }
        }


        public async Task AddTps575LogsAsync(Tps575Logs tps575Logs)
        {
            await _tps575LogsRepository.AddTps575LogsAsync(tps575Logs);

        }

        private async Task PrintSingleAsync(SalesLogs salesLogs, OmnitechLoginResponse loginResponse, OmnitechShiftResponse omnitechShiftResponse)
        {
            SalesLogs salesLogsFromDb = await _salesLogsRepository.GetAllSalesLogsByRecnoAsync(salesLogs.RECNO);

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
                omnitechOpenShiftResponse = await OpenShiftAsync(loginResponse);

                Tps575Logs tps575Logs = new Tps575Logs
                {
                    FAKTURA_NAME = salesLogs.FAKTURA_NAME,
                    insert_date = DateTime.Now,
                    request = StandartJsons.OpenShiftJson(loginResponse),
                    response = System.Text.Json.JsonSerializer.Serialize(omnitechOpenShiftResponse)
                };

                await AddTps575LogsAsync(tps575Logs);
            }

            string responseText = await InvokeAsync(salesLogs.REQUEST_TPS575);

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
        }

        //public async Task PrintProblemicSalesLogsAsync()
        //{
        //    DateTime now = DateTime.Now;
        //    JobPermission jobPermission = null;

        //    try
        //    {
        //        jobPermission = await _jobPermissionRepository.GetJobPermissionByIdAsync(jobId);

        //        if (jobPermission != null)
        //        {
        //            if (jobPermission.HasPermission == 1 && jobPermission.IsRunning == 0 && jobPermission.NextRun <= now)
        //            {
        //                await _jobPermissionRepository.ChangePermissionAsync(jobPermission.Id, 1, jobPermission.IntervalUnit, jobPermission.IntervalType);

        //                List<SalesLogs> salesLogsList = await _salesLogsRepository.GetProblemicSalesLogsForPrintAsync();

        //                OmnitechLoginResponse loginResponse = await Login();
        //                OmnitechShiftResponse omnitechShiftResponse = await CheckShiftAsync(loginResponse);

        //                foreach (var salesLogs in salesLogsList)
        //                {
        //                    try
        //                    {
        //                        if (await _jobPermissionRepository.GetHasPermissionById(jobId) == 1)
        //                        {
        //                            lock (LockSingle)
        //                            {
        //                                PrintSingleAsync(salesLogs, loginResponse, omnitechShiftResponse).GetAwaiter().GetResult();
        //                            }
        //                        }
        //                    }

        //                    catch (Exception exp)
        //                    {
        //                        try
        //                        {
        //                            string logMsg = $"{salesLogs.RECNO} --> {salesLogs.FAKTURA_NAME} --> {exp.Message}";

        //                            await _jobPermissionRepository.AddJobExceptionAsync(jobPermission.Id, exp.Message);
        //                        }

        //                        catch (Exception exp2)
        //                        {
        //                            string expMsg = exp2.Message;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception exp)
        //    {
        //        await _jobPermissionRepository.AddJobExceptionAsync(jobId, exp.Message);
        //    }

        //    finally
        //    {
        //        await _jobPermissionRepository.ChangePermissionAsync(jobPermission.Id, 0, jobPermission.IntervalUnit, jobPermission.IntervalType);
        //    }
        //}


        public async Task<List<SalesLogs>> PrintProblemicSalesLogsAsync()
        {
            List<SalesLogs> salesLogs = await _salesLogsRepository.GetProblemicSalesLogsForPrintAsync();

            return salesLogs;
        }
    }
}
