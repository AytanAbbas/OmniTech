using System;

namespace Omnitech.Models
{
    public class Tps575Logs
    {
        public int ID { get; set; }
        public string FAKTURA_NAME { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public DateTime insert_date { get; set; }

    }
}
