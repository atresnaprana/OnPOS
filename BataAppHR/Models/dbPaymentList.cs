using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BataAppHR.Models
{
    public class dbPaymentList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int id_order { get; set; }
        public int id_customer { get; set; }
        public string BANK { get; set; }
        public string REF_ID { get; set; }
        public string FILE_PAYMENT_NAME { get; set; }
        public byte[] FILE_PAYMENT { get; set; }
        public decimal? TOTAL_PAY { get; set; }
        public DateTime? PAYMENT_DATE { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
        [NotMapped]
        public List<dbSalesOrder> ddorder { get; set; }
        [NotMapped]
        public List<dbCustomer> ddCustomer { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileTransaction { get; set; }
        [NotMapped]
        public int id_orderCust { get; set; }
    
    }
}
