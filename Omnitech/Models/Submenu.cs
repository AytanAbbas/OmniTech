namespace Omnitech.Models
{
    public class Submenu
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int MenuId { get; set; }
    }
}
