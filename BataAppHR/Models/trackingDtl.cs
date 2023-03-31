using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BataAppHR.Models
{
    public class trackingDtl
    {
        public string invoice { get; set; }
        public string edp { get; set; }
        public decimal dus { get; set; }
        public decimal pairs { get; set; }
        public decimal val_price { get; set; }
        public string sik { get; set; }
        public string trans_no { get; set; }
        public string driver { get; set; }
        public DateTime date { get; set; }
        public string time { get; set; }

    }
}
