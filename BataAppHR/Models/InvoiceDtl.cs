using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataAppHR.Models
{
    public class InvoiceDtl
    {
        public string article { get; set; }
        public string projectname { get; set; }

        public int qty { get; set; }
        public decimal? price { get; set; }
       
        public string S1 { get; set; }
        public string S2 { get; set; }
        public string S3 { get; set; }
        public string S4 { get; set; }
        public string S5 { get; set; }
        public string S6 { get; set; }
        public string S7 { get; set; }
        public string S8 { get; set; }
        public string S9 { get; set; }
        public string S10 { get; set; }
        public string S11 { get; set; }
        public string S12 { get; set; }
        public string S13 { get; set; }
        public string disctype { get; set; }
        public decimal? disc { get; set; }
        public decimal? final { get; set; }
    }
}
