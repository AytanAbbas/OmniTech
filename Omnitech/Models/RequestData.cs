namespace Omnitech.Models
{
    public class RequestData
    {
        public CheckData checkData { get; set; }
        public string access_token { get; set; }

        public string int_ref { get; set; }
        public TokenData tokenData { get; set; }
    }
}
