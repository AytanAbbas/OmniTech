using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Omnitech.Models;
using System.Collections.Generic;

namespace Omnitech.Dtos
{
    public class KNInvoiceDto
    {
        public List<KNInvoiceInfo> KNInvoiceInfos { get; set; }
        public List<KNInvoiceDetail> KNInvoiceDetails { get; set; }
        public List<SalesLogs> SalesLogs { get; set; }

    }
}
