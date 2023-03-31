using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BataAppHR.Models
{
    public class trackingModel
    {
        public string edp { get; set; }
        public string invoice { get; set; }
        public string status { get; set; }
        public DateTime update_date { get; set; }

    }
}
