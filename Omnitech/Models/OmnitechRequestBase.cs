namespace Omnitech.Models
{
    public class OmnitechRequestBase
    {
        public OmnitechRequestBase()
        {
            requestData = new RequestData();
        }
        public RequestData requestData { get; set; }
    }
}
