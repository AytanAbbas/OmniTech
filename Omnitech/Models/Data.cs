using System.Collections.Generic;

namespace Omnitech.Models
{
    public class Data
    {
        public string cashier { get; set; }
        public string currency { get; set; }
        public List<Item> items { get; set; }
        public double sum { get; set; }
        public double cashSum { get; set; }
        public double cashlessSum { get; set; }
        public double prepaymentSum { get; set; }
        public double creditSum { get; set; }
        public double bonusSum { get; set; }
        public List<VatAmount> vatAmounts { get; set; }
    }
}
