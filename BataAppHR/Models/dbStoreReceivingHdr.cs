using System.Collections.Generic;
using System;

namespace OnPOS.Models
{
    public class dbStoreReceivingHdr
    {
        public string supply_invoice { get; set; }
        public string storeid { get; set; }
        public string storename { get; set; }
        public DateTime estimatearrival { get; set; }
        public string totaldayestimate { get; set; }
        public string status { get; set; }
        public string entry_user { get; set; }
        public string update_user { get; set; }
        public DateTime entry_date { get; set; }
        public DateTime update_date { get; set; }
        public List<dbStoreReceivingDtl> dtltbl { get; set; }

    }
}
