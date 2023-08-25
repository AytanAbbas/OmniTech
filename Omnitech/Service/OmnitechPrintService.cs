using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using Omnitech.Models;
using Omnitech.Dal.AdoNet;
using System.Collections.Generic;
using Omnitech.Utilities;

namespace Omnitech.Service
{
    public class OmnitechPrintService
    {
        public static async Task<OmnitechLoginResponse> Login(string url)
        {
            string json = StandartJsons.LoginJson;

            string responseText = await InvokeAsync(json, url);

            return System.Text.Json.JsonSerializer.Deserialize<OmnitechLoginResponse>(responseText);
        }

        public static async Task<OmnitechShiftResponse> CheckShiftAsync(OmnitechLoginResponse omnitechLoginResponse, string url)
        {
            OmnitechRequestBase omnitechRequestBase = new OmnitechRequestBase();

            CheckData checkData = new CheckData
            {
                check_type = 14
            };

            RequestData requestData = new RequestData
            {
                checkData = checkData,
                access_token = omnitechLoginResponse.access_token
            };

            omnitechRequestBase.requestData = requestData;

            string json = System.Text.Json.JsonSerializer.Serialize(omnitechRequestBase);

            string responseText = await InvokeAsync(json, url);

            return System.Text.Json.JsonSerializer.Deserialize<OmnitechShiftResponse>(responseText);
        }

        public static async Task<OmnitechOpenShiftResponse> OpenShiftAsync(OmnitechLoginResponse omnitechLoginResponse, string url)
        {
            string json = StandartJsons.OpenShiftJson(omnitechLoginResponse);

            string responseText = await InvokeAsync(json, url);

            return System.Text.Json.JsonSerializer.Deserialize<OmnitechOpenShiftResponse>(responseText);
        }

        public static async Task ZReportAsync(string url)
        {
            OmnitechRequestBase omnitechRequestBase = new OmnitechRequestBase();

            CheckData checkData = new CheckData
            {
                check_type = 13
            };

            RequestData requestData = new RequestData
            {
                checkData = checkData,
                access_token = Login(url).GetAwaiter().GetResult().access_token
            };

            omnitechRequestBase.requestData = requestData;


            string json = System.Text.Json.JsonSerializer.Serialize(omnitechRequestBase);

            string responseText = await InvokeAsync(json, url);

        }

        public static async Task<string> InvokeAsync(string json, string url)
        {
            string responseText = "";

            try
            {
                if (string.IsNullOrEmpty(url))
                    throw new Exception("Url is empty");

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(60);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        responseText = await response.Content.ReadAsStringAsync();
                    }

                    else
                    {
                    }

                    httpClient.Dispose();
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }

            return responseText;
        }
    }
}