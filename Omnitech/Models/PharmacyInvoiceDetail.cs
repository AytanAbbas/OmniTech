using System;

namespace Omnitech.Models
{
    public class PharmacyInvoiceDetail
    {
        public int LOGICALREF { get; set; }
        public int SKU { get; set; }
        public DateTime TARIX { get; set; }
        public double MIQDAR { get; set; }
        public double MEBLEG { get; set; }
        public int ANBAR { get; set; }
        public string MEHSULUN_KODU { get; set; }
        public string MEHSULUN_ADI { get; set; }
        public string FIRMA { get; set; }
        public string ISTEHSALCHI { get; set; }
        public string BARCODE { get; set; }
        public string VAHID { get; set; }
        public int EDEDSAYI { get; set; }
        public int DELETED { get; set; }
    }
}
