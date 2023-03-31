using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BataAppHR.Models
{
    public class dbsalescreditlog
    {
        public string invno { get; set; }
        public int year { get; set; }
        public decimal? valuecredit { get; set; }
        public string isused { get; set; }
        public string edp { get; set; }
    }
}
