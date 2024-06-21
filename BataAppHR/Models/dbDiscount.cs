using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnPOS.Models
{
    public class dbDiscount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string article { get; set; }
        public string type { get; set; }
        public int percentage { get; set; }
        public int amount { get; set; }
        public string entry_user { get; set; }
        public DateTime entry_date { get; set; }
        public string update_user { get; set; }
        public DateTime update_date { get; set; }
       
    }
}
