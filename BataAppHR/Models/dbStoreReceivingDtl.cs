using System;

namespace OnPOS.Models
{
    public class dbStoreReceivingDtl
    {
        public string supply_invoice { get; set; }

        public string itm_id { get; set; }
        public string itmname { get; set; }

        public int qty { get; set; }
        public int s35 { get; set; }
        public int s36 { get; set; }
        public int s37 { get; set; }
        public int s38 { get; set; }
        public int s39 { get; set; }
        public int s40 { get; set; }
        public int s41 { get; set; }
        public int s42 { get; set; }
        public int s43 { get; set; }
        public int s44 { get; set; }
        public int s45 { get; set; }
        public string entry_user { get; set; }
        public string update_user { get; set; }
        public DateTime entry_date { get; set; }
        public DateTime update_date { get; set; }
    }
}
