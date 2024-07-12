using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Globalization;

namespace OnPOS.Models
{
    public class dbDiscount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string COMPANY_ID { get; set; }
        public string promo_name { get; set; }

        public string article { get; set; }
        public string type { get; set; }
        public int percentage { get; set; }
        public int amount { get; set; }
        public DateTime validfrom { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
        public DateTime validto { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
        public string status { get; set; }
        public string isallstore { get; set; }


        public string entry_user { get; set; }
        public DateTime entry_date { get; set; }
        public string update_user { get; set; }
        public DateTime update_date { get; set; }
        [NotMapped]
        public bool isactive { get; set; }
        [NotMapped]
        public bool isallstorebool { get; set; }
        [NotMapped]
        public List<DropDownModel> liststore { get; set; }
        [NotMapped]
        public List<dbDiscountStoreList> listpromostore { get; set; }
        [NotMapped]
        public List<int> storeidlist { get; set; }
        [NotMapped]
        public List<DropDownModel> listitems { get; set; }
        [NotMapped]
        public string errormessage { get; set; }


    }
}
