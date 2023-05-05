namespace Omnitech.Models
{
    public class Item
    {
        public string itemName { get; set; }
        public int itemCodeType { get; set; }
        public string itemCode { get; set; }
        public int itemQuantityType { get; set; }
        public double itemQuantity { get; set; }
        public double itemPrice { get; set; }
        public double itemSum { get; set; }
        public double itemVatPercent { get; set; }
        public double discount { get; set; }
    }
}
