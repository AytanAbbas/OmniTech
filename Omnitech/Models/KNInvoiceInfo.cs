using System;

namespace Omnitech.Models
{
    public class KNInvoiceInfo
    {
        public int INV_ID { get; set; }
        public string FAKTURANO { get; set; }
        public DateTime TARIX { get; set; }
        public string CARI_HESAB_KODU { get; set; }
        public string CARI_HESAB_ADI { get; set; }
        public double FAKTURA_MEBLEGI { get; set; }
        public double QEBZ_MEBLEGI { get; set; }
        public double CAP_EDILMIS_MEBLEG { get; set; }
        public int CAP_SAYI { get; set; }
        public int setr_sayi { get; set; }
        public string FICSAL_DOCUMENT { get; set; }
    }
}
