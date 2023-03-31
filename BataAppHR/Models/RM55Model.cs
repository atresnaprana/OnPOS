using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BataAppHR.Models
{
    public class RM55Model
    {
        public string Batch { get; set; }
        public string Comp { get; set; }
        public string Article { get; set; }
        public string Cat { get; set; }
        public int Seq { get; set; }
        public string DC_TOT { get; set; }
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
        public string Rec { get; set; }
        public string Price { get; set; }
        public string Cost { get; set; }
        public string Ext { get; set; }

    }
}
