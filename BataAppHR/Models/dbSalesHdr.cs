using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using OnPOS.Models;
using System.Collections.Generic;

namespace OnPOS.Models
{
    public class dbSalesHdr
    {
        public string invoice { get; set; }
        public string Store_id { get; set; }
        public string staff_id { get; set; }
        public DateTime transdate { get; set; }
        public int trans_amount { get; set; }
        public int trans_qty { get; set; }
        public string approval_code { get; set; }
        public string cardnum { get; set; }
        public string transtype { get; set; }
        public string update_user { get; set; }
        public DateTime update_date { get; set; }
        [NotMapped]
        public string scanitem { get; set; }
        [NotMapped]
        public string username { get; set; }
        [NotMapped]
        public List<dbSalesDtl> salesDtl { get; set; }
        [NotMapped]
        public string syserr { get; set; }
        [NotMapped]
        public string salesid { get; set; }
        [NotMapped]
        public int scanqty { get; set; }
        [NotMapped]
        public decimal? scanprice { get; set; }
        [NotMapped]
        public decimal? scandiscount { get; set; }
        [NotMapped]
        public int s33 { get; set; }
        [NotMapped]
        public int s34 { get; set; }
        [NotMapped]
        public int s35 { get; set; }
        [NotMapped]
        public int s36 { get; set; }
        [NotMapped]
        public int s37 { get; set; }
        [NotMapped]
        public int s38 { get; set; }
        [NotMapped]
        public int s39 { get; set; }
        [NotMapped]
        public int s40 { get; set; }
        public int s41 { get; set; }
        [NotMapped]
        public int s42 { get; set; }
        [NotMapped]
        public int s43 { get; set; }
        [NotMapped]
        public int s44 { get; set; }
        [NotMapped]
        public int s45 { get; set; }
        [NotMapped]
        public int s46 { get; set; }
        [NotMapped]
        public int totalstock { get; set; }

    }
}
