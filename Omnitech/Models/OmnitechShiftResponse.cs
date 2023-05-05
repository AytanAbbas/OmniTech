namespace Omnitech.Models
{
    public class OmnitechShiftResponse
    {
        public int code { get; set; }
        public string desc { get; set; }
        public string message { get; set; }
        public string serial { get; set; }
        public bool shiftStatus { get; set; }
        public dynamic shift_open_time { get; set; }
    }
}
