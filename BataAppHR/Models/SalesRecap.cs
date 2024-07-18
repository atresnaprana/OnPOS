namespace OnPOS.Models
{
    public class SalesRecap
    {
        public int store_id { get; set; }
        public string transdate { get; set; }
        public string article { get; set; }
        public string itemdescription { get; set; }
        public string category { get; set; }
        public string invoice { get; set; }
        public string discounttype { get; set; }
        public int discount { get; set; }
        public int articleprice { get; set; }
        public string department { get; set; }
        public int qty { get; set; }
        public int amount { get; set; }
        public string sz { get; set; }

    }
}
