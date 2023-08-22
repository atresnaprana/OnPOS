
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace OnPOS.Models
{
    public class dbItemMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string COMPANY_ID { get; set; }
        public string itemid { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string itemdescription { get; set; }
        public decimal price1 { get; set; }
        public decimal price2 { get; set; }
        public decimal price3 { get; set; }
        public decimal brand { get; set; }

        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
    }
}
