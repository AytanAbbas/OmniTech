using System;

namespace Omnitech.Models
{
    public class SalesLogs
    {
        public int RECNO { get; set; }
        public string FAKTURA_NO { get; set; }
        public string FAKTURA_NAME { get; set; }
        public string REQUEST_TPS575 { get; set; }
        public string RESPONSE_TPS575 { get; set; }
        public string FICSAL_DOCUMENT { get; set; }
        public int TIPI { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public int PC_NAME { get; set; }
        public string IP_REQUEST { get; set; }
        public string FICSAL_DOCUMENT_LONG { get; set; }
        public int FIRMA { get; set; }
        public double QEBZMEBLEG { get; set; }

    }
}
