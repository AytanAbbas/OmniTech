namespace Omnitech.Models
{
    public class StandartJsons
    {
        public static string LoginJson
        {
            get
            {
                return @"
                    {
                    ""requestData"":{ ""checkData"":{ ""check_type"":40
                    },
                    ""name"":""SuperApi"",
                    ""password"":""123""
                    }
                    }";
            }//Kassir 1     //SuperApi 123
        }
     
        public static string OpenShiftJson(OmnitechLoginResponse omnitechLoginResponse)
        {
            OmnitechRequestBase omnitechRequestBase = new OmnitechRequestBase();

            CheckData checkData = new CheckData
            {
                check_type = 15
            };

            RequestData requestData = new RequestData
            {
                checkData = checkData,
                access_token = omnitechLoginResponse.access_token
            };

            omnitechRequestBase.requestData = requestData;


            return System.Text.Json.JsonSerializer.Serialize(omnitechRequestBase);
        }


        public static string CloseShiftJson(OmnitechLoginResponse omnitechLoginResponse)
        {
            OmnitechRequestBase omnitechRequestBase = new OmnitechRequestBase();

            CheckData checkData = new CheckData
            {
                check_type = 13
            };

            RequestData requestData = new RequestData
            {
                checkData = checkData,
                access_token = omnitechLoginResponse.access_token
            };

            omnitechRequestBase.requestData = requestData;


            return System.Text.Json.JsonSerializer.Serialize(omnitechRequestBase);
        }
       
    }
}
