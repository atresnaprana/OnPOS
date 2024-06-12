using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OnPOS.Models
{
    public class dbSalesDtl
    {
       
        public int trans_id { get; set; }
        public string invoice { get; set; }
        public DateTime transdate { get; set; }
        public string item_id { get; set; }
        public string cat { get; set; }
        public string subcat { get; set; }
        public string price { get; set; }
        public decimal? disc_per { get; set; }
        public decimal? disc_prc { get; set; }
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
        public int s46 { get; set; }
        
        public string update_user { get;set; }
        public DateTime update_date { get; set; }


    }
}
