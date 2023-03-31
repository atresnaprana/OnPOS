using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace BataAppHR.Models
{
    public class dbSalesOrderDtl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int id_order { get; set; }
        public string article { get; set; }
        public string size { get; set; }
        public int qty { get; set; }
        public decimal? price { get; set; }
        public decimal? disc { get; set; }
        public string is_disc_perc { get; set; }
        public decimal? disc_amt { get; set; }
        public decimal? final_price { get; set; }

        public int Size_1 { get; set; }
        public int Size_2 { get; set; }
        public int Size_3 { get; set; }
        public int Size_4 { get; set; }
        public int Size_5 { get; set; }
        public int Size_6 { get; set; }
        public int Size_7 { get; set; }
        public int Size_8 { get; set; }
        public int Size_9 { get; set; }
        public int Size_10 { get; set; }
        public int Size_11 { get; set; }
        public int Size_12 { get; set; }
        public int Size_13 { get; set; }

        [NotMapped]
        public int id_customer { get; set; }
        [NotMapped]
        public string EMP_CODE { get; set; }
        [NotMapped]
        public decimal? TOTAL_ORDER { get; set; }
        [NotMapped]
        public int TOTAL_QTY { get; set; }
        [NotMapped]
        public string STATUS { get; set; }
        [NotMapped]
        public string APPROVAL_1 { get; set; }
        [NotMapped]
        public string APPROVAL_2 { get; set; }
        [NotMapped]
        public DateTime? ORDER_DATE { get; set; }
        [NotMapped]
        public string picking_no { get; set; }
        [NotMapped]
        public decimal? weighttage { get; set; }
        [NotMapped]
        public decimal? discountperarticle { get; set; }
        [NotMapped]
        public decimal? articlevaltotal{ get; set; }
        [NotMapped]
        public decimal? articlevalpiece { get; set; }
        [NotMapped]
        public decimal? creditval { get; set; }
    }
}
