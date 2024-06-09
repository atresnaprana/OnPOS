using System;

namespace OnPOS.Models
{
    public class dbStoreStockModel
    {
        public string storeid { get; set; }
        public string itmid { get; set; }
        public string itmname { get; set; }
        public string area { get; set; }
        public string row { get; set; }
        public string rack { get; set; }
        public string racklvl { get; set; }
        public string bin_id { get; set; }
        public int qty { get; set; }
        public int s33 { get; set; }
        public int s34 { get; set; }
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
        public DateTime? lastrcvdate { get; set; }
        public DateTime? lastoutdate { get; set; }
    }
}
