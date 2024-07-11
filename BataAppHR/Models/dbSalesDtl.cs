using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OnPOS.Models
{
    public class dbSalesDtl
    {
        public int staff_id {  get; set; }
        public int store_id { get; set; }
        public string invoice { get; set; }
        public DateTime transdate { get; set; }
        public string article { get; set; }
        public string cat { get; set; }
        public string subcat { get; set; }
        public int price { get; set; }
        public int discountcode { get; set; }
        public int disc_amount { get; set; }
        public int disc_prc { get; set; }
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
        [NotMapped]
        public string Crew { get; set; }
        [NotMapped]
        public string Article { get; set; }
        [NotMapped]
        public string size { get; set; }
        [NotMapped]
        public int pairs { get; set; }
        [NotMapped]
        public int disc {  get; set; }
        [NotMapped]
        public string disctype {  get; set; }
        [NotMapped]
        public int subtotal { get; set; }



    }
}
