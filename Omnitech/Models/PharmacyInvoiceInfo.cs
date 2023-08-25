using System;

namespace Omnitech.Models
{
    public class PharmacyInvoiceInfo
    {
        public DateTime TARIX { get; set; }
        public int ANBAR { get; set; }
        public string FAKTURA { get; set; }
         public double SATISH { get; set; }
        public double IADE { get; set; }
        public double NET_SATISH { get; set; }
        //public int SETR_SAY { get; set; }
        //public double CEMI_MAL_SAYI_IADE_CIXILMIS { get; set; }
        //public double CEMI_MEBLEG_IADE_CIXILMIS { get; set; }
        //public double IADE_MEBLEG_CEMI { get; set; }
        //public double IADE_MEBLEG_CEMI_GUNLUK_SATISH { get; set; }
        //public double QADAGA_SATISH { get; set; }
        //public double QADAGA_IADE { get; set; }
        public string KASSA_GONDERILME { get; set; }
        public string APTEKIN_ADI { get; set; }
        public string fiscal { get; set; }
        public double mebleg_cap { get; set; }
        public DateTime CAP_TARIXI { get; set; }
    }
}
