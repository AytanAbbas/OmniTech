using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Omnitech.Models;
using System.Collections.Generic;

namespace Omnitech.Dtos
{
    public class PharmacyInvoiceDto
    {
        public List<PharmacyInvoiceInfo> PharmacyInvoiceInfos { get; set; }
        public List<PharmacyInvoiceDetail> PharmacyInvoiceDetails { get; set; }
        public List<FoodSupplementItem> FoodSupplementItems { get; set; }
        public PharmacyInvoiceItemReplaceResponse PharmacyInvoiceItemReplaceResponse { get; set; }
        public List<SalesLogs> SalesLogs { get; set; }

    }
}
